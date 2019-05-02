using System;
using System.Collections.Generic;
using System.Text;

namespace NotificationApp.DataAccess.Options
{
    public class DataProviderOptions
    {
        public static DataProviderOptions Instance;
        public string ConnectionString { get; set; }
    }
}
