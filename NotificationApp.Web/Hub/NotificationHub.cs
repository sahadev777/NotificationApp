using Microsoft.AspNetCore.SignalR;
using NotificationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationApp.Web.Hub
{

    /// <summary>
    /// Used for providing realtime updates for the NotificationController.
    /// </summary>
    public class NotificationHub : Hub<INotificationClient>
    {

        /// <summary>
        /// Notify all off-premise employee that a notification message has been added.
        /// </summary>
        public async Task AddNewnotification(bool isAdded) => await Clients.All.AddNewnotification(isAdded);

        public async Task Acknowledgement(bool isACK) => await Clients.All.Acknowledgement(isACK);
    }
}
