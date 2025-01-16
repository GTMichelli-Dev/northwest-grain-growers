using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;

namespace Scale_Service
{
    static class Program
    {
        static Mutex mutex = new Mutex(true, "{8F6F0AC4-B9B3-45fd-A8CF-72F04E6BDE9c}");
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
                new Scale_Service()
                };
                ServiceBase.Run(ServicesToRun);
            }

        }
    }
}
