using System.Configuration;
using System.Reflection;

using Microsoft.Azure;
using System;
using System.Linq;

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
            if (this.IsAzureEnvironment())
            {
                return CloudConfigurationManager.GetSetting(name);
            }
            else
            {
                return ConfigurationManager.ConnectionStrings[name].ConnectionString;
            }
        }

        private bool IsAzureEnvironment()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();

            return loadedAssemblies.Any(a 
                => a.FullName.StartsWith("Microsoft.WindowsAzure.ServiceRuntime"));
        }
    }
}
