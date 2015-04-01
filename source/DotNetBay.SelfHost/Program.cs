using System;

using DotNetBay.WebApi.Controller;

using Microsoft.Owin.Hosting;

namespace DotNetBay.SelfHost
{
    class Program
    {
        static void Main(string[] args)
        {
            var typesLoaded = typeof(StatusController);

            WebApp.Start<Startup>(url: "http://localhost:9001/");

            Console.ReadLine();
        }
    }
}
