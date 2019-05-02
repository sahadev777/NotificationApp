using System;

namespace NotificationApp.Models
{
    public class Staff
    {
        public int StaffId { get; set; }
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string TimeZone { get; set; }
        public char? Gender { get; set; }
        public DateTime? Birthdate { get; set; }
        public DateTime CreateDate { get; set; }
        public string IP { get; set; }
    }
}
