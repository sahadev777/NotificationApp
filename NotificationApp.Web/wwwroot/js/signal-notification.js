"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/hubs/notification").build();

//document.getElementById("submit-btn").disabled = true;


connection.start().then(function () {
    //document.getElementById("submit-btn").disabled = false;
    console.info("Connected");
}).catch(function (err) {
    return console.error(err.toString());
    });

connection.on("AddNewnotification", function (isAdded) {
    if (isAdded) {
        reloadEmpNotificationDetails();
    }
});

connection.on("Acknowledgement", function (isACK) {
    if (isACK) {
        reloadAdminNotificationDetails();
    }
});