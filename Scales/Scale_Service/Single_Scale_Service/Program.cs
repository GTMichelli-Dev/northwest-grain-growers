using System;
using System.ServiceProcess;
using System.Threading;

namespace Single_Scale_Service
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9B3-68fd-A8CF-72F04E6BDE9c}");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                    new Single_Scale_Service()
                };
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                // Notify the user that another instance is already running.
                Console.WriteLine("Another instance of the service is already running.");
                // Since this is a service, this message will only be visible when running the executable manually.
                // If running as a service, the SCM will not show this message.
            }
        }
    }
}
