# 🌍 World Countries Explorer

A full-stack web app for exploring the countries of the world: browse and filter countries, explore them on an interactive map, keep personal travel lists, test yourself with quizzes, share experiences with other users, and get travel recommendations from an AI advisor.

Final project for the **Server-Side** and **Client-Side Development** courses at Ruppin Academic Center (2026).

## ✨ Features
- **Country catalog**: search and filter by continent, language, currency and more, with a details page for each country
- **Interactive world map**: click any country to open its details
- **Personal lists**: mark countries as visited or wishlist and manage your lists
- **Quizzes**: geography quizzes with scoring, attempt history and points
- **Shares**: post and browse travel experiences per country
- **AI Country Advisor**: ask travel questions answered by OpenAI (GPT)
- **User accounts**: register, login, profile, languages and password change
- **Admin dashboard**: lock users, manage sharing permissions, moderate shares, view statistics

## 🛠 Tech Stack
| Layer | Technologies |
|------|--------------|
| Client | HTML5, CSS3, JavaScript, jQuery / AJAX |
| Server | C#, ASP.NET Core Web API (.NET 6), Swagger |
| Database | Microsoft SQL Server, stored procedures |
| External APIs | OpenAI API, external countries data API |

## 🏗 Architecture
```
client/  (HTML + JS pages)  ──AJAX/JSON──►  server/  ASP.NET Core Web API
                                               ├─ Controllers  (REST endpoints)
                                               ├─ Models       (business logic)
                                               ├─ DAL          (DBservices → SQL Server stored procedures)
                                               └─ Services     (OpenAI, countries API import)
```

## 📁 Project Structure
```
client/                 Front end: one page per screen
  index.html            Countries catalog
  country.html          Country details
  Map.html              Interactive world map
  MyLists.html          Visited / wishlist
  Quiz.html             Quizzes
  Shares.html           Community shares
  AIAdvisor.html        AI travel advisor
  Admin.html            Admin dashboard
  common.js, ajaxCalls.js
server/
  CountriesServers.slnx
  CountriesServers/     ASP.NET Core Web API
```

## 🔌 Main API Endpoints
| Resource | Endpoints |
|---------|-----------|
| `api/Users` | Register, Login, Logout, profile |
| `api/Countries` | list + filters, details, import, CRUD |
| `api/Lists` | add / update / remove countries in user lists |
| `api/Quiz` | quizzes, questions, start, submit, attempts, points |
| `api/Shares` | create, edit, delete, by country |
| `api/Admin` | users, lock, share permissions, statistics |
| `api/AI` | Ask the AI advisor |

## 🚀 Running Locally
1. Open `server/CountriesServers.slnx` in Visual Studio.
2. Create `server/CountriesServers/appsettings.Local.json` (git-ignored) with your own values:
   ```json
   {
     "ConnectionStrings": { "myProjDB": "Data Source=...;Initial Catalog=...;User ID=...;Password=..." },
     "OpenAI": { "ApiKey": "sk-..." }
   }
   ```
3. Run the server and open `/swagger` to try the API.
4. In `client/common.js`, set `SERVER` to your server URL and open `client/index.html` (e.g. with Live Server).

## 🌿 Branches
- `main`: stable, released version
- `develop`: integration branch
- `ServerSide`: original server-side development history
- `clientSide`: original client-side development history

## 👥 Team
- **Adam Elmalek**
- **Farah** ([@farah412](https://github.com/farah412))

Originally developed at [cgroup372026/CountriesProject](https://github.com/cgroup372026/CountriesProject).
