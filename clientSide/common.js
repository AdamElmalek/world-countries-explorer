// common.js
// Shared settings and helpers for every page.

// CHANGE THIS to your own address on the Ruppin server
const SERVER = "https://proj.ruppin.ac.il/cgroup137/test2/tar1";

const usersApi = SERVER + "/api/Users";
const countriesApi = SERVER + "/api/Countries";
const listsApi = SERVER + "/api/Lists";
const sharesApi = SERVER + "/api/Shares";
const quizApi = SERVER + "/api/Quiz";
const adminApi = SERVER + "/api/Admin";


// Logged user (kept in localStorage, like in HW4)
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