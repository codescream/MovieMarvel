using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MovieMarvel.Models;
using MovieMarvel.Utitlity;

namespace MovieMarvel
{
    public class Program
    {
        public static Fetch Fetch = new Fetch();
        public static Control Control = new Control();
        public static Rental Rental = new Rental();
        // public static MovieContext MovieDB = new MovieContext();

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}