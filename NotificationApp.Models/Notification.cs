using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationApp.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public int StaffId { get; set; }
        public int AdminStaffId { get; set; }
        public string Message { get; set; }
        public DateTime SentDate { get; set; }

        public DateTime? AcknowledgementDate { get; set; }
    }
    public class NotificationDetails : Notification
    {
        public string IP { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
    public class NotificationAdmin : NotificationDetails
    {
        public string EmployeeName { get; set; }
    }

    public class NotificationEmployee : Notification
    {
        public string AdminName { get; set; }
    }
}
