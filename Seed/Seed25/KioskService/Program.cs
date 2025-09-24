using System;
using System.Device.Gpio;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace GpioInputApp
{
    class Program
    {

        private static DateTime _lastEventTime = DateTime.MinValue;
        private static  TimeSpan _debounceTime = TimeSpan.FromMilliseconds(500);
        private static int _gpioPin = 5;
        private static string _printer = "NOT SET";
        private static string _locationId = "1";
        private static string _websiteUrl = "http://10.102.224.63/api/Kiosk";

        private static async Task Main(string[] args)
        {
            var appName = Assembly.GetExecutingAssembly().GetName().Name;
            var appVersion = Assembly.GetExecutingAssembly().GetName().Version;
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("*******************************************");
            Console.WriteLine("");
            Console.WriteLine($"{appName} - Version {appVersion} © NWGG");
            Console.WriteLine("");
            Console.WriteLine("********************************************");
            // Check if appsettings.json exists, if not create it with default values
            var configFilePath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
            if (!File.Exists(configFilePath))
            {
                var defaultConfig = @"
                {
                  ""LocationId"": ""1"",
                  ""PortName"": ""/dev/ttyUSB0"",
                  ""Printer"": ""KioskPrinter2"",
                  ""GPIOPin"": 4,
                  
                  ""WebsiteUrl"": ""http://10.102.224.63/api/Kiosk""
                }";
                File.WriteAllText(configFilePath, defaultConfig);
                Console.WriteLine("Created default appsettings.json file.");
            }

            // Build configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Read configuration values
            _locationId = configuration["LocationId"] ?? "1";
            _printer = configuration["Printer"] ?? "NOT SET";
            _gpioPin = int.Parse(configuration["GPIOPin"] ?? "5");
            _debounceTime = TimeSpan.FromMilliseconds(int.Parse(configuration["DebounceTime"] ?? "500"));

            _websiteUrl = configuration["WebsiteUrl"] ?? "http://10.102.224.63/api/Kiosk";
            Console.WriteLine();

            Console.WriteLine(">>>>>>>>>>>>>>>>>>>> CONFIGURATION <<<<<<<<<<<<<<<<<<<<");
            Console.WriteLine($"GPIO Pin: {_gpioPin}");
            Console.WriteLine($"Printer: {_printer}");
            Console.WriteLine($"DebounceTime: {_debounceTime}");

            Console.WriteLine($"Website URL: {_websiteUrl}");
            Console.WriteLine(">>>>>>>>>>>>>>>>>>END  CONFIGURATION <<<<<<<<<<<<<<<<<<");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("___________________________________________");
            Console.WriteLine("Press Ctrl+C to exit.");
            using HttpClient client = new HttpClient();


            var checkButtonTask = CheckButtonPush(client);
            await Task.WhenAll(checkButtonTask);
        }

        private async static Task CheckButtonPush(HttpClient client)
        {
            try
            {

                using var controller = new GpioController();
                controller.OpenPin(_gpioPin, PinMode.InputPullUp);
                var lastPinValue = controller.Read(_gpioPin);
                Console.WriteLine($"Monitoring GPIO Pin:{_gpioPin}  Initial status ({DateTime.Now}): {(lastPinValue == PinValue.Low ? "ALERT 🚨" : "READY ✅")}");
                while (true)
                {
                    try
                    {

                        var pinValue = controller.Read(_gpioPin);
                        var now = DateTime.Now;
                        if (pinValue != lastPinValue)
                        {
                            Console.WriteLine($"({DateTime.Now}) {(pinValue == PinValue.Low ? "ALERT 🚨" : "READY ✅")}");
                            lastPinValue = pinValue;
                            if (now - _lastEventTime > _debounceTime)
                                {
                                _lastEventTime = now;
                                Console.WriteLine("Button Pushed");
                                await NotifyButtonPushed(client);
                            }
                            else { 
                                Console.WriteLine("Ignored due to debounce \u00AC");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error reading GPIO pin: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening serial port: {ex.Message}");
            }
        }


        private static async Task NotifyButtonPushed(HttpClient client)
        {
            //var request = new HttpRequestMessage(HttpMethod.Post, $"{_websiteUrl}/buttonPushed?Printer={_printer}");
            //request.Content = new StringContent("{\"message\":\"Button pushed\"}", Encoding.UTF8, "application/json");

            //HttpResponseMessage response = await client.SendAsync(request);
            //if (!response.IsSuccessStatusCode)
            //{
            //    Console.WriteLine($"Error sending notification: {response.StatusCode} ({response.ReasonPhrase})");
            //}
            //else
            //{
            //    Console.WriteLine("Notification sent successfully!");
            //}
            await Task.Delay(100); // Simulate some async work
        }
      
      
    }
}
