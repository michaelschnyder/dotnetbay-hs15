using System.Configuration;

using Microsoft.Azure;
using System;
using System.Linq;

namespace DotNetBay.Common
{
    public class DotNetBayAppSettings
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is fine.")]
        public string DatabaseConnection
        {
            get
            {
                return GetDbConnection("DatabaseConnection");
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "This is fine.")]
        public bool HasWorker
        {
            get
            {
                return IsAzureEnvironment();
            }
        }

        private static string GetDbConnection(string name)
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
                => a.FullName.StartsWith("Microsoft.WindowsAzure.ServiceRuntime", StringComparison.OrdinalIgnoreCase));
        }
    }
}
