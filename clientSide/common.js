const SERVER = "https://localhost:7242";
const usersApi = SERVER + "/api/Users";
const countriesApi = SERVER + "/api/Countries";
const listsApi = SERVER + "/api/Lists";
const sharesApi = SERVER + "/api/Shares";
const quizApi = SERVER + "/api/Quiz";
const adminApi = SERVER + "/api/Admin";
const aiApi = SERVER + "/api/AI";



function getLoggedUser() {
    let userStr = localStorage.getItem("user");

    if (userStr == null) {
        return null;
    }

    return JSON.parse(userStr);
}

function saveLoggedUser(user) {
    localStorage.setItem("user", JSON.stringify(user));
    localStorage.setItem("isLoggedIn", "true");
}

function clearLoggedUser() {
    localStorage.removeItem("user");
    localStorage.removeItem("isLoggedIn");
}

function isLoggedIn() {
    return getLoggedUser() != null;
}

function isAdmin() {
    let user = getLoggedUser();
    return user != null && user.role == "Admin";
}

// Sends the visitor to Login.html when the page requires a user
function requireLogin() {
    if (!isLoggedIn()) {
        window.location.href = "Login.html";
        return false;
    }

    return true;
}

// Admin pages use this one
function requireAdmin() {
    if (!requireLogin()) {
        return false;
    }

    if (!isAdmin()) {
        $("body").html("<div class='page'><h2>Admin permission is required</h2>" +
            "<a href='index.html'>Back to home page</a></div>");
        return false;
    }

    return true;
}

function logoutFunc() {
    ajaxCall("POST", usersApi + "/Logout", null, logoutSuccess, logoutSuccess);
}

function logoutSuccess() {
    clearLoggedUser();
    window.location.href = "Login.html";
}


function buildMenu(activePage) {
    let user = getLoggedUser();

    let pages = [
        ["index.html", "Countries", "countries", false],
        ["MyLists.html", "My Lists", "lists", true],
        ["Shares.html", "Shares", "shares", false],
        ["Quiz.html", "Quiz", "quiz", true],
        ["Map.html", "Map", "map", false],
        ["Advanced.html", "Advanced Features", "advanced", false],
        ["AIAdvisor.html", "AI Advisor", "ai", true]
    ];

    let str = "<div class='top-row'><div class='menu-links'>";

    for (let i = 0; i < pages.length; i++) {
        let needsLogin = pages[i][3];

        if (needsLogin && !isLoggedIn()) {
            continue;
        }

        let activeClass = (pages[i][2] == activePage) ? "active" : "";

        str += `<a class="${activeClass}" href="${pages[i][0]}">${pages[i][1]}</a>`;
    }

    if (isAdmin()) {
        let activeClass = (activePage == "admin") ? "active" : "";
        str += `<a class="${activeClass}" href="Admin.html">Admin</a>`;
    }

    str += "</div><div class='menu-user'>";

    if (user == null) {
        str += "<span>Hello guest</span>";
        str += "<input type='button' value='Login' onclick=\"window.location.href='Login.html'\" />";
    }
    else {
        str += "<span>Hello " + user.fullName + "</span>";
        str += "<input type='button' value='My Profile' onclick=\"window.location.href='Profile.html'\" />";
        str += "<input type='button' value='LogOut' onclick='logoutFunc()' />";
    }

    str += "</div></div>";

    $("#menuPH").html(str);
}

// ---------------------------------------------------------
// Small helpers
// ---------------------------------------------------------

// Numbers with thousands separators
function formatNumber(num) {
    if (num == null) {
        return "0";
    }

    return Number(num).toLocaleString();
}

// Protects against HTML injected by other users (shares, names)
function safeText(text) {
    if (text == null) {
        return "";
    }

    return $("<div>").text(text).html();
}

// Reads a value from the address bar, e.g. country.html?id=3
function getQueryParam(name) {
    let params = new URLSearchParams(window.location.search);
    return params.get(name);
}

// The standard error callback used by every page
function error(err) {
    console.log(err);

    if (err != null && err.responseText) {
        swal("Error", err.responseText, "error");
    }
    else {
        swal("Error", "The server could not be reached", "error");
    }
}
