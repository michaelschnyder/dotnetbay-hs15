using System;
using System.Data.Entity.SqlServer;
using System.Net.Http;

using DotNetBay.WebApi.Controller;

using Microsoft.Owin.Hosting;

namespace DotNetBay.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var typesLoaded = new[] { typeof(StatusController), typeof(SqlProviderServices) };

            var host = "http://localhost:9001/";

            using (var app = WebApp.Start<Startup>(url: host))
            {
                // SelfCheck
                var client = new HttpClient();
                client.BaseAddress = new Uri(host);

                var response = client.GetAsync("/api/status").Result;

                Console.WriteLine(response);

                Console.Write("Press enter to quit.");
                Console.ReadLine();
            }
        }
    }
}
