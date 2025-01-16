using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using UpdateZipCodes.Models;

class Program
{
    static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // Example of how to use the service
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<NW_DataContext>();

            // Read and deserialize the JSON file
            var jsonFilePath = "USCities.json";
            var jsonString = File.ReadAllText(jsonFilePath);
            var zipCodes = JsonSerializer.Deserialize<List<DownloadedZipCodes>>(jsonString);
            if (zipCodes == null)
            {
                Console.WriteLine("Error deserializing JSON file");
                return;
            }
            // Example: Print the zip codes

            context.ZipCodes.RemoveRange(context.ZipCodes);
            context.SaveChanges();
            foreach (var zipCode in zipCodes)
            {
                context.Add(new ZipCode
                {
                    State = zipCode.state,
                    County = zipCode.county,
                    Zip = zipCode.zip_code.ToString()
                });
               // Console.WriteLine($"Zip Code: {zipCode.zip_code}, City: {zipCode.city}, State: {zipCode.state}");
            }
            context.SaveChanges();



        }


    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            })
            .ConfigureServices((context, services) =>
            {
                services.AddDbContext<NW_DataContext>(options =>
                    options.UseSqlServer(context.Configuration.GetConnectionString("DefaultConnection")));

                // Add other services here
            });
}
