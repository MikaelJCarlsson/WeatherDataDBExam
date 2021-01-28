using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Globalization;
using System.IO;
using WeatherDataExam.Model.Entities;

namespace WeatherDataExam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //ReadCSV(@"File\TemperaturData.csv");
            CreateHostBuilder(args).Build().Run();
        }
        private static void ReadCSV(string data)
        {
            string[] csvFile = File.ReadAllLines($"{data}");

            using (var context = new WeatherDataDBContext())
            {
                foreach (var value in csvFile)
                {
                    WeatherData wd = new WeatherData();
                    string[] tmpData = value.Split(",");

                    wd.Date = DateTime.Parse(tmpData[0]);
                    wd.Location = tmpData[1];
                    wd.Temperature = float.Parse(tmpData[2], CultureInfo.InvariantCulture);
                    wd.Humidity = float.Parse(tmpData[3], CultureInfo.InvariantCulture);

                    context.Add(wd);

                }
                context.SaveChanges();
            }

        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
