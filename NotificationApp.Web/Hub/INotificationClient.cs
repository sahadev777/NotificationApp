using NotificationApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotificationApp.Web.Hub
{
    public interface INotificationClient
    {
        /// <summary>
        /// This event occurs when new message is posted to the
        /// NofiticationController.
        /// </summary>
        Task AddNewnotification(bool isAdded);

        /// <summary>
        /// This event occurs when new Acknowledgement sent by employees with IP address, Geolocation((longitude/latitude) and  Time stamp to Admin'
        /// NofiticationController.
        /// </summary>
 
        Task Acknowledgement(bool isACK);
    }
}
