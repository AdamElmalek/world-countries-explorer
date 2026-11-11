using CountriesServers.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace CountriesServers.Services
{
    public class AIService
    {
        private static readonly HttpClient httpClient =
            new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(90)
            };

        private readonly IConfiguration configuration;

        public AIService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async Task<AIAnswerResponse> AskCountryAdvisor(
            AIQuestionRequest request)
        {
            if (request == null)
            {
                throw new ArgumentException("Request is required");
            }

            request.Question =
                (request.Question ?? string.Empty).Trim();

            if (request.UserId <= 0)
            {
                throw new ArgumentException(
                    "A valid user is required"
                );
            }

            if (request.CountryId <= 0)
            {
                throw new ArgumentException(
                    "A valid country is required"
                );
            }

            if (request.Question.Length < 3 ||
                request.Question.Length > 500)
            {
                throw new ArgumentException(
                    "The question must contain between 3 and 500 characters"
                );
            }

            // Verify the user.
            User userModel = new User();
            User? user = userModel.GetById(request.UserId);

            if (user == null)
            {
                throw new ArgumentException(
                    "User was not found"
                );
            }

            if (user.IsLocked)
            {
                throw new UnauthorizedAccessException(
                    "User is locked"
                );
            }

            // Read the selected country from SQL Server.
            Country countryModel = new Country();
            Country? country =
                countryModel.GetById(request.CountryId);

            if (country == null)
            {
                throw new ArgumentException(
                    "Country was not found"
                );
            }

            // First try an environment variable.
            // Otherwise, read from appsettings.json.
            string apiKey =
                Environment.GetEnvironmentVariable(
                    "OPENAI_API_KEY"
                )
                ?? configuration["OpenAI:ApiKey"]
                ?? string.Empty;

            if (string.IsNullOrWhiteSpace(apiKey))
            {
                throw new InvalidOperationException(
                    "The AI service is not configured. " +
                    "Add the OpenAI API key on the server."
                );
            }

            string model =
                configuration["OpenAI:Model"]
                ?? "gpt-5-mini";

            string languages =
                country.LanguageList != null &&
                country.LanguageList.Count > 0
                    ? string.Join(
                        ", ",
                        country.LanguageList.Select(
                            language =>
                                language.LanguageName
                        )
                    )
                    : country.Languages;

            string currencies =
                country.CurrencyList != null &&
                country.CurrencyList.Count > 0
                    ? string.Join(
                        ", ",
                        country.CurrencyList.Select(
                            currency =>
                                currency.CurrencyName +
                                " (" +
                                currency.CurrencyCode +
                                ")"
                        )
                    )
                    : country.Currencies;

            string countryData =
                "Country: " + country.Name + "\n" +
                "Native name: " + country.NativeName + "\n" +
                "Capital: " + country.Capital + "\n" +
                "Region: " + country.Region + "\n" +
                "Subregion: " + country.Subregion + "\n" +
                "Population: " + country.Population + "\n" +
                "Area in square kilometres: " +
                country.Area + "\n" +
                "Languages: " + languages + "\n" +
                "Currencies: " + currencies + "\n" +
                "Demonym: " + country.Demonym;

            object payload = new
            {
                model = model,

                // Do not save the API response on OpenAI.
                store = false,

                // GPT-5 uses reasoning tokens.
                // Low effort leaves more tokens for the answer.
                reasoning = new
                {
                    effort = "low"
                },

                // Includes both reasoning and visible answer tokens.
                max_output_tokens = 1600,

                input = new object[]
                {
                    new
                    {
                        role = "developer",

                        content = new object[]
                        {
                            new
                            {
                                type = "input_text",

                                text =
                                    "You are an AI country and travel " +
                                    "advisor inside a student project. " +

                                    "Use the supplied database facts " +
                                    "as the factual basis. " +

                                    "Give a clear, practical and concise " +
                                    "answer of no more than 500 words. " +

                                    "Organize travel plans by day when " +
                                    "the user asks for an itinerary. " +

                                    "Do not claim that visa rules, " +
                                    "safety warnings, prices, opening " +
                                    "hours or political conditions are " +
                                    "current. Tell the user to verify " +
                                    "time-sensitive information."
                            }
                        }
                    },

                    new
                    {
                        role = "user",

                        content = new object[]
                        {
                            new
                            {
                                type = "input_text",

                                text =
                                    countryData +
                                    "\n\nUser question: " +
                                    request.Question
                            }
                        }
                    }
                }
            };

            string requestJson =
                JsonSerializer.Serialize(payload);

            using HttpRequestMessage httpRequest =
                new HttpRequestMessage(
                    HttpMethod.Post,
                    "https://api.openai.com/v1/responses"
                );

            httpRequest.Headers.Authorization =
                new AuthenticationHeaderValue(
                    "Bearer",
                    apiKey
                );

            httpRequest.Content =
                new StringContent(
                    requestJson,
                    Encoding.UTF8,
                    "application/json"
                );

            using HttpResponseMessage providerResponse =
                await httpClient.SendAsync(httpRequest);

            string responseJson =
                await providerResponse.Content
                    .ReadAsStringAsync();

            if (!providerResponse.IsSuccessStatusCode)
            {
                string providerMessage =
                    ReadErrorMessage(responseJson);

                throw new InvalidOperationException(
                    "The AI provider returned an error: " +
                    providerMessage
                );
            }

            string refusal =
                ReadRefusal(responseJson);

            if (!string.IsNullOrWhiteSpace(refusal))
            {
                throw new InvalidOperationException(
                    "The AI provider refused the request: " +
                    refusal
                );
            }

            string answer =
                ReadOutputText(responseJson);

            if (string.IsNullOrWhiteSpace(answer))
            {
                string details =
                    ReadEmptyResponseDetails(responseJson);

                throw new InvalidOperationException(
                    "The AI provider returned no visible answer. " +
                    details
                );
            }

            return new AIAnswerResponse
            {
                CountryId = country.CountryId,
                CountryName = country.Name,
                Question = request.Question,
                Answer = answer.Trim(),
                Model = model,
                GeneratedAt = DateTime.UtcNow
            };
        }

        private string ReadOutputText(
            string responseJson)
        {
            using JsonDocument document =
                JsonDocument.Parse(responseJson);

            JsonElement root =
                document.RootElement;

            List<string> textParts =
                new List<string>();

            /*
             * Some response formats may expose an aggregated
             * output_text property.
             */
            if (root.TryGetProperty(
                    "output_text",
                    out JsonElement aggregatedText) &&
                aggregatedText.ValueKind ==
                    JsonValueKind.String)
            {
                string? value =
                    aggregatedText.GetString();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    textParts.Add(value);
                }
            }

            if (!root.TryGetProperty(
                    "output",
                    out JsonElement output) ||
                output.ValueKind != JsonValueKind.Array)
            {
                return string.Join(
                    "\n",
                    textParts
                );
            }

            foreach (JsonElement outputItem
                     in output.EnumerateArray())
            {
                if (!outputItem.TryGetProperty(
                        "content",
                        out JsonElement content) ||
                    content.ValueKind !=
                        JsonValueKind.Array)
                {
                    continue;
                }

                foreach (JsonElement contentItem
                         in content.EnumerateArray())
                {
                    string contentType =
                        contentItem.TryGetProperty(
                            "type",
                            out JsonElement typeElement)
                            ? typeElement.GetString()
                                ?? string.Empty
                            : string.Empty;

                    if (contentType != "output_text")
                    {
                        continue;
                    }

                    if (!contentItem.TryGetProperty(
                            "text",
                            out JsonElement textElement))
                    {
                        continue;
                    }

                    string? text =
                        textElement.GetString();

                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        textParts.Add(text);
                    }
                }
            }

            return string.Join(
                "\n",
                textParts.Distinct()
            );
        }

        private string ReadRefusal(
            string responseJson)
        {
            using JsonDocument document =
                JsonDocument.Parse(responseJson);

            JsonElement root =
                document.RootElement;

            if (!root.TryGetProperty(
                    "output",
                    out JsonElement output) ||
                output.ValueKind != JsonValueKind.Array)
            {
                return string.Empty;
            }

            foreach (JsonElement outputItem
                     in output.EnumerateArray())
            {
                if (!outputItem.TryGetProperty(
                        "content",
                        out JsonElement content) ||
                    content.ValueKind !=
                        JsonValueKind.Array)
                {
                    continue;
                }

                foreach (JsonElement contentItem
                         in content.EnumerateArray())
                {
                    string contentType =
                        contentItem.TryGetProperty(
                            "type",
                            out JsonElement typeElement)
                            ? typeElement.GetString()
                                ?? string.Empty
                            : string.Empty;

                    if (contentType != "refusal")
                    {
                        continue;
                    }

                    if (contentItem.TryGetProperty(
                            "refusal",
                            out JsonElement refusalElement))
                    {
                        return refusalElement.GetString()
                            ?? string.Empty;
                    }
                }
            }

            return string.Empty;
        }

        private string ReadEmptyResponseDetails(
            string responseJson)
        {
            try
            {
                using JsonDocument document =
                    JsonDocument.Parse(responseJson);

                JsonElement root =
                    document.RootElement;

                string status =
                    root.TryGetProperty(
                        "status",
                        out JsonElement statusElement)
                        ? statusElement.GetString()
                            ?? "unknown"
                        : "unknown";

                string reason = string.Empty;

                if (root.TryGetProperty(
                        "incomplete_details",
                        out JsonElement incompleteDetails) &&
                    incompleteDetails.ValueKind ==
                        JsonValueKind.Object &&
                    incompleteDetails.TryGetProperty(
                        "reason",
                        out JsonElement reasonElement))
                {
                    reason =
                        reasonElement.GetString()
                        ?? string.Empty;
                }

                int reasoningTokens = 0;

                if (root.TryGetProperty(
                        "usage",
                        out JsonElement usage) &&
                    usage.ValueKind ==
                        JsonValueKind.Object &&
                    usage.TryGetProperty(
                        "output_tokens_details",
                        out JsonElement outputDetails) &&
                    outputDetails.ValueKind ==
                        JsonValueKind.Object &&
                    outputDetails.TryGetProperty(
                        "reasoning_tokens",
                        out JsonElement reasoningElement) &&
                    reasoningElement.TryGetInt32(
                        out int parsedReasoningTokens))
                {
                    reasoningTokens =
                        parsedReasoningTokens;
                }

                string details =
                    "Status: " + status + ".";

                if (!string.IsNullOrWhiteSpace(reason))
                {
                    details +=
                        " Reason: " + reason + ".";
                }

                if (reasoningTokens > 0)
                {
                    details +=
                        " Reasoning tokens used: " +
                        reasoningTokens +
                        ".";
                }

                return details;
            }
            catch
            {
                return "The response could not be parsed.";
            }
        }

        private string ReadErrorMessage(
            string responseJson)
        {
            try
            {
                using JsonDocument document =
                    JsonDocument.Parse(responseJson);

                JsonElement root =
                    document.RootElement;

                if (root.TryGetProperty(
                        "error",
                        out JsonElement error) &&
                    error.TryGetProperty(
                        "message",
                        out JsonElement message))
                {
                    return message.GetString()
                        ?? "Unknown AI provider error";
                }
            }
            catch
            {
                // The provider did not return valid JSON.
            }

            return "Unknown AI provider error";
        }
    }
}