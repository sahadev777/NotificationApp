using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NotificationApp.DataAccess.Helpers;
using NotificationApp.DataAccess.Interfaces;
using NotificationApp.DataAccess.Options;
using NotificationApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationApp.DataAccess.Providers
{
    public class NotificationProvider : INotificationProvider
    {
        #region Global Declaration
        private readonly DataProviderOptions _options;
        private readonly ILogger<NotificationProvider> _logger;
        private readonly DataProviderHelper _dataProviderHelper = new DataProviderHelper();
        #endregion

        #region Constant

        private const string InsertNotificationProc = "usp_Insert_Notification";
        private const string UpdateNotificationProc = "usp_Update_Notification_Acknowledgement";
        private const string GetEmployeeNotificationsInfoByStaffIdProc = "usp_GetEmployeeNotificationsInfoByStaffId";
        private const string usp_GetAdminNotificationInfoByAdminStaffIdProc = "usp_GetAdminNotificationInfoByAdminStaffId";

        private const string NOTIFICATIONID = "NotificationId";
        private const string STAFFID = "StaffId";
        private const string EMPLOYEENAME = "EmployeeName";
        private const string ADMINNAME = "AdminName";
        private const string ADMINSTAFFID = "AdminStaffId";
        private const string MESSAGE = "Message";
        private const string SENTDATE = "SentDate";
        private const string ACKKNOWLEDGMENTDATE = "AcknowledgementDate";
        private const string IP = "IP";
        private const string LATITUDE = "Latitude";
        private const string LONGITUDE = "Longitude";

        #endregion

        #region constuctor
        public NotificationProvider(IOptions<DataProviderOptions> dataProviderOptions, ILogger<NotificationProvider> logger)
        {
            _options = dataProviderOptions.Value;
            _logger = logger;
        }

        #endregion

        #region Methods
        public async Task<int> CreateNotificationAsync(Notification notification)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@StaffId", notification.StaffId),
                    new SqlParameter("@AdminStaffId", notification.AdminStaffId),
                    new SqlParameter("@Message", notification.Message),
                };
                return Convert.ToInt32(await _dataProviderHelper.StoredProcScalerAsync(_options.ConnectionString, InsertNotificationProc, parameters.ToArray()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }    
        }

        public async Task<IEnumerable<NotificationAdmin>> GetNotificationsByAdminIdAsync(int adminId)
        {
            var notifications = new List<NotificationAdmin>();
            try
            {
                var sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@AdminStaffId", adminId)
                };
                notifications.AddRange(await _dataProviderHelper.StoredProcAsync(_options.ConnectionString, usp_GetAdminNotificationInfoByAdminStaffIdProc, x => GetAdminNotificationFromReaderAsync(x), sqlParams));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
            return notifications.Any() ? notifications : Enumerable.Empty<NotificationAdmin>();
        }

        private async Task<IEnumerable<NotificationAdmin>> GetAdminNotificationFromReaderAsync(DbDataReader reader)
        {
            var retValue = new List<NotificationAdmin>();
            while (await reader.ReadAsync())
            {
                var notification = new NotificationAdmin
                {
                    NotificationId = (int)reader[NOTIFICATIONID],
                    AdminStaffId = (int)reader[ADMINSTAFFID],
                    Message = reader[MESSAGE] as string,
                    SentDate = (DateTime)reader[SENTDATE],
                    AcknowledgementDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal(ACKKNOWLEDGMENTDATE)))) ? null : (DateTime?)reader.GetValue(reader.GetOrdinal(ACKKNOWLEDGMENTDATE)),
                    EmployeeName = reader[EMPLOYEENAME] as string,
                    IP = reader[IP] as string,
                    Latitude = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal(LATITUDE)))) ? null : (decimal?)reader.GetValue(reader.GetOrdinal(LATITUDE)),
                    Longitude = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal(LONGITUDE)))) ? null : (decimal?)reader.GetValue(reader.GetOrdinal(LONGITUDE)),
                };
                retValue.Add(notification);
            }
            return retValue;
        }
        public async Task<IEnumerable<NotificationEmployee>> GetNotificationsByEmployeeIdAsync(int employeeId)
        {
            var notifications = new List<NotificationEmployee>();
            try
            {
                var sqlParams = new SqlParameter[]
                {
                    new SqlParameter("@StaffId", employeeId)
                };
                notifications.AddRange(await _dataProviderHelper.StoredProcAsync(_options.ConnectionString, GetEmployeeNotificationsInfoByStaffIdProc, x => GetEmployeeNotificationFromReaderAsync(x), sqlParams));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw;
            }
            return notifications.Any() ? notifications : Enumerable.Empty<NotificationEmployee>();

        }
        private async Task<IEnumerable<NotificationEmployee>> GetEmployeeNotificationFromReaderAsync(DbDataReader reader)
        {
            var retValue = new List<NotificationEmployee>();
            while (await reader.ReadAsync())
            {
                var notification = new NotificationEmployee
                {
                    NotificationId = (int)reader[NOTIFICATIONID],
                    AdminStaffId = (int)reader[ADMINSTAFFID],
                    AdminName = reader[ADMINNAME] as string,
                    Message = reader[MESSAGE] as string,
                    SentDate = (DateTime)reader[SENTDATE],
                    StaffId = (int) reader[STAFFID],
                    AcknowledgementDate = string.IsNullOrEmpty(Convert.ToString(reader.GetValue(reader.GetOrdinal(ACKKNOWLEDGMENTDATE)))) ? null : (DateTime?)reader.GetValue(reader.GetOrdinal(ACKKNOWLEDGMENTDATE)),
                };
                retValue.Add(notification);
            }
            return retValue;
        }

        public async Task UpdateNotificationACKDetailsAsync(NotificationDetails notification)
        {
            try
            {
                var parameters = new List<SqlParameter>
                {
                    new SqlParameter("@NotificationId", notification.NotificationId),
                    new SqlParameter("@IP", notification.IP),
                    new SqlParameter("@Latitude", notification.Latitude),
                    new SqlParameter("@Longitude", notification.Longitude),
                };
                await _dataProviderHelper.StoredProcNonQueryAsync(_options.ConnectionString, UpdateNotificationProc, parameters.ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                throw ex;
            }
        }
        #endregion

    }
}
