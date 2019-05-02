$(function () {
    $("#submit-btn").click(function () {
        var formAction = $('#form-postnotification').attr("action");
        var msg = $('#txtMessage').val();
        if (msg) {
            $.ajax({
                url: formAction,
                dataType: 'json',
                type: 'post',
                contentType: 'application/json',
                data: JSON.stringify(msg),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    reloadAdminNotificationDetails();
                    $('#txtMessage').val('');
                },
                error: function (jqXhr, textStatus, errorThrown) {

                }
            });
        }
        else {
            alert("Please Insert value for Notification Message.")
        }
        
    });

    $(document).ready(function () {
        loadAdminNotiData();
        loadEmployeeNotiData();
    });

    function loadAdminNotiData() {
        $('#table-admin-noti-details').DataTable({
            "ajax": {
                "url": '/Notification/GetAdminNotifications',
                "dataSrc": ""
            },
            "columns": [
                { "data": "employeeName" },
                { "data": "message" },
                {
                    "data": "sentDate",
                    "render": function (data) {
                        var date = new Date(data);
                        return moment(date).format('MM-DD-YYYY hh:mm:ss A');
                    }
                },
                {
                    "data": "acknowledgementDate",
                    "render": function (data) {
                        if (data) {
                            var date = new Date(data);
                            return moment(date).format('MM-DD-YYYY hh:mm:ss A');
                        }
                        else {
                            return '';
                        }

                    }
                },
                { "data": "ip" },
                { "data": "latitude" },
                { "data": "longitude" }
            ],
            "order": [[2, "desc"]]
        });
    }



    function loadEmployeeNotiData() {
        $('#table-emp-noti-details').DataTable({
            "ajax": {
                "url": '/Notification/GetEmployeeNotifications',
                "dataSrc": ""
            },
            "columns": [
                { "data": "adminName" },
                { "data": "message" },
                {
                    "data": "sentDate",
                    "render": function (data) {
                        var date = new Date(data);
                        return moment(date).format('MM-DD-YYYY hh:mm:ss A');
                    }
                },
                {
                    "data": "acknowledgementDate",
                    "render": function (data) {
                        if (data) {
                            var date = new Date(data);
                            return moment(date).format('MM-DD-YYYY hh:mm:ss A');
                        }
                        else {
                            return '';
                        }

                    }
                },
                {
                    "render": function (data, type, row) {
                        if (!row.acknowledgementDate) {
                            return '<a href="#" onclick="ackNotification(' + row.notificationId + ')">Acknowledge</a>';
                        }
                        else {
                            return '';
                        }
                    }
                }
               
            ],
            "order": [[2, "desc"]]
        });
    }

});

function reloadEmpNotificationDetails() {
    $('#table-emp-noti-details').DataTable().ajax.reload();
}


function reloadAdminNotificationDetails() {
    $('#table-admin-noti-details').DataTable().ajax.reload();
}
function ackNotification(notificationId) {
    debugger;
    if ("geolocation" in navigator) { //check geolocation available 
        //try to get user current location using getCurrentPosition() method
        navigator.geolocation.getCurrentPosition(function (position) {
            debugger;
            latitude = position.coords.latitude;
            longitude = position.coords.longitude;

            var obj = {
                "NotificationId": notificationId,
                "Latitude": latitude,
                "Longitude": longitude,
            }

            $.ajax({
                url: '/Notification/PutNotification',
                dataType: 'json',
                type: 'put',
                contentType: 'application/json',
                data: JSON.stringify(obj),
                processData: false,
                success: function (data, textStatus, jQxhr) {
                    reloadEmpNotificationDetails();

                },
                error: function (jqXhr, textStatus, errorThrown) {
                    console.error(errorThrown);
                }
            });
        }, showError);
    } else {
        alert("Browser doesn't support geolocation! please allow location permission");
    }
}




function showError(error) {
    var errorMsg;
    switch (error.code) {
        case error.PERMISSION_DENIED:
            errorMsg = "User denied the request for Geolocation."
            break;
        case error.POSITION_UNAVAILABLE:
            errorMsg = "Location information is unavailable."
            break;
        case error.TIMEOUT:
            errorMsg = "The request to get user location timed out."
            break;
        case error.UNKNOWN_ERROR:
            errorMsg = "An unknown error occurred."
            break;
    }
    alert(errorMsg);
}