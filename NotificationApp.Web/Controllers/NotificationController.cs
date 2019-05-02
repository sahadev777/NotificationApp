using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using NotificationApp.DataAccess.Interfaces;
using NotificationApp.Models;
using NotificationApp.Web.Hub;
using NotificationApp.Web.Models;

namespace NotificationApp.Web.Controllers
{
    public class NotificationController : Controller
    {

        #region Global declaration
        private readonly INotificationProvider _notificationProvider;
        private IHubContext<NotificationHub> _hub;
        private IHttpContextAccessor _accessor;
        #endregion

        #region constructor
        public NotificationController(INotificationProvider notificationProvider, IHubContext<NotificationHub> hub, IHttpContextAccessor accessor)
        {
            _notificationProvider = notificationProvider;
            _hub = hub;
            _accessor = accessor;
        }
        #endregion

        #region Views related actions
        /// <summary>
        ///This action used for Redirect to Admin view
        /// </summary>
        /// <returns></returns>
        public IActionResult AdminView()
        {
            return View();
        }

        /// <summary>
        /// This action used for Redirect to Employee view
        /// </summary>
        /// <returns></returns>
        public IActionResult EmployeeView()
        {
            return View();
        }
        #endregion

        #region API methods
        /// <summary>
        /// This method used for create new notification message
        /// </summary>
        /// <param name="message">new message</param>
        /// <returns>new generated notification id</returns>
        [HttpPost]
        public async Task<IActionResult> PostNotification([FromBody] string message)
        {
            try
            {
                Notification notification = new Notification();
                notification.AdminStaffId = Constants.ADMINID;
                notification.StaffId = Constants.EMPID;
                notification.Message = message;
                var notificationId = await _notificationProvider.CreateNotificationAsync(notification);
                await _hub.Clients.All.SendAsync("AddNewnotification", true);
                return new OkObjectResult(notificationId);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        /// <summary>
        /// This method is used for save acknowledgement(IP , Latitute and Longitute) information
        /// </summary>
        [HttpPut]
        public async Task<IActionResult> PutNotification([FromBody] NotificationAckViewModel notificationAckViewModel)
        {
            try
            {
                NotificationDetails notificationDetails = new NotificationDetails();
                notificationDetails.NotificationId = notificationAckViewModel.NotificationId;
                string remoteIP = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                if (!string.IsNullOrEmpty(remoteIP))
                notificationDetails.IP = remoteIP;
                notificationDetails.Latitude = notificationAckViewModel.Latitude;
                notificationDetails.Longitude = notificationAckViewModel.Longitude;
                await _notificationProvider.UpdateNotificationACKDetailsAsync(notificationDetails);
                await _hub.Clients.All.SendAsync("Acknowledgement", true);
                return new OkObjectResult(true);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        
        /// <summary>
        /// This method use for get all admin notification related information
        /// </summary>
        /// <returns>Admin related notification information</returns>
        [HttpGet]
        public async Task<IActionResult> GetAdminNotifications()
        {
            try
            {
                return new OkObjectResult(await _notificationProvider.GetNotificationsByAdminIdAsync(Constants.ADMINID));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// This method will return employee related notification infornation
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetEmployeeNotifications()
        {
            try
            {
                return new OkObjectResult(await _notificationProvider.GetNotificationsByEmployeeIdAsync(Constants.EMPID));
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
        #endregion
    }
}