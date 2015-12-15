using System.Configuration;
using System.Reflection;

using Microsoft.Azure;
using System;
using System.Linq;

using Microsoft.SqlServer.Server;

namespace DotNetBay.Common
{
    public class DotNetBayAppSettings
    {
        public string DatabaseConnection
        {
            get
            {
                return this.GetDbConnection("DatabaseConnection");
            }
        }

        private string GetDbConnection(string name)
        {
            if (IsAzureEnvironment())
            {
                return CloudConfigurationManager.GetSetting(name);
            }
            else
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
        }

        private static bool IsAzureEnvironment()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            return loadedAssemblies.Any(a 
                => a.FullName.StartsWith("Microsoft.WindowsAzure.ServiceRuntime", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}
