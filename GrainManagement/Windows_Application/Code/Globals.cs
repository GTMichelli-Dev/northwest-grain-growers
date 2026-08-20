using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWGrain
{
    class Globals
    {
      //  public static enum enumTicketType { Inbound_Load, Outbound_Load, Transaction_Load }
        static DateTime LastUpdate;
        static string CurrentWeighmaster;
        public static  System.Drawing.Point MainPoint;
        public static string Weighmaster
        {
            get
            {
                if (LastUpdate.ToShortDateString() != DateTime.Now.ToShortDateString())
                {
                    CurrentWeighmaster = "";
                }
                return CurrentWeighmaster;
            }
            set 
            {
                CurrentWeighmaster = value;
                LastUpdate = DateTime.Now;
            }
        }
    }
}
