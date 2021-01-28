using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace WeatherDataExam.Model.Entities
{
    public class WeatherDataDBContext : DbContext
    {
        private string connectionString;
        public WeatherDataDBContext() : base()
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json", optional: false);

            var configuration = builder.Build();
            connectionString = configuration.GetConnectionString("sqlConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        public DbSet<WeatherData> WeatherData { get; set; }

    }
}

