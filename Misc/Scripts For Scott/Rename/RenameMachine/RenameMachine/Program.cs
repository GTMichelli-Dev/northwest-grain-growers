using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RenameMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Machine Name");
            var Name = Console.ReadLine();
            Console.WriteLine("New Machine Name is " + Name + "Type Y to Confirm");
            var Response = Console.ReadLine();
            if (Response.ToUpper() == "Y")
            {
                SetMachineName(Name);
            }
        }


        public static bool SetMachineName(string newName)
        {
            RegistryKey key = Registry.LocalMachine;

            string activeComputerName = "SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ActiveComputerName";
            RegistryKey activeCmpName = key.CreateSubKey(activeComputerName);
            activeCmpName.SetValue("ComputerName", newName);
            activeCmpName.Close();
            string computerName = "SYSTEM\\CurrentControlSet\\Control\\ComputerName\\ComputerName";
            RegistryKey cmpName = key.CreateSubKey(computerName);
            cmpName.SetValue("ComputerName", newName);
            cmpName.Close();
            string _hostName = "SYSTEM\\CurrentControlSet\\services\\Tcpip\\Parameters\\";
            RegistryKey hostName = key.CreateSubKey(_hostName);
            hostName.SetValue("Hostname", newName);
            hostName.SetValue("NV Hostname", newName);
            hostName.Close();
            return true;
        }
    }
}
