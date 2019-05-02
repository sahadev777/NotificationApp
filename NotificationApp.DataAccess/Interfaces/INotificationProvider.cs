using NotificationApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NotificationApp.DataAccess.Interfaces
{
    public interface INotificationProvider
    {
        Task<IEnumerable<NotificationAdmin>> GetNotificationsByAdminIdAsync(int adminId);

        Task<IEnumerable<NotificationEmployee>> GetNotificationsByEmployeeIdAsync(int employeeId);

        Task<int> CreateNotificationAsync(Notification notification);

        Task UpdateNotificationACKDetailsAsync(NotificationDetails notification);
    }
}
