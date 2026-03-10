// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function checkLogInValidity(e, msgBox) {
    const reg = /^[a-zA-Z0-9#]+$/
    const msgName = document.getElementById(msgBox)

    if (!e.value.match(reg)) {
        msgName.style = "color:red; text-align:center"
        msgName.innerHTML = " User Name and Password  contains english letters or digits only"
    }
    else {
        msgName.style = "color:green; text-align:center"
        msgName.innerHTML = "ALL GOOD"
    }
}