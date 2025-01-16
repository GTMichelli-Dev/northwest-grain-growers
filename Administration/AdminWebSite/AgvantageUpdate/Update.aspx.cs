using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;


public partial class Update : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Security.GetUsersSecurityLevel(Session) == Security.enumSecurityLevel.View) Response.Redirect("~/Default.aspx");
        if (!this.IsPostBack)
        {
            if (File.Exists(@"c:\Agvantage\Results_Crops.xml"))
            {
                File.Delete(@"c:\Agvantage\Results_Crops.xml");
            }
            if (File.Exists(@"c:\Agvantage\Results_Producers.xml"))
            {
                File.Delete(@"c:\Agvantage\Results_Producers.xml");
            }
            if (File.Exists(@"c:\Agvantage\Results_Carriers.xml"))
            {
                File.Delete(@"c:\Agvantage\Results_Carriers.xml");
            }


            this.hfStartingTime.Value = DateTime.Now.ToString();

            Timer2.Enabled = true;



            String command = @"c:\Agvantage\Transfer.bat";


            ProcessStartInfo ProcessInfo;

            Process process = new Process();
            ProcessInfo = new ProcessStartInfo("cmd.exe", "/c " + command);

            process = Process.Start(ProcessInfo);
            //process.WaitForExit();

        }
    }



    protected void Timer2_Tick(object sender, EventArgs e)
    {
        var CropsDone = (File.Exists(@"c:\Agvantage\Results_Crops.xml")) ? true : false;
        var ProducersDone = (File.Exists(@"c:\Agvantage\Results_Producers.xml")) ? true : false;
        var CarriersDone = (File.Exists(@"c:\Agvantage\Results_Carriers.xml")) ? true : false;

            var WhatsDownloading = "Downloading Crops<br/>";


            string WhatsDone = "";
            if (CropsDone)
            {

                WhatsDownloading = "Downloading Producers <br/>";
                WhatsDone = "Downloaded Crops <br/>";
            }
            if (ProducersDone)
            {
                WhatsDownloading = "Downloading Carriers<br/>";
                WhatsDone += "Downloaded Producers <br/>";
            }

            if (CarriersDone)
            {
                Wait.Visible = false;
                Response.Redirect("~/AgvantageUpdate/Results?home=1");
            }

            //var What = "<br/>AllDone:" + AllDone.Value + "  CropsDone:" + CropsDone.ToString() + "  ProducersDone:" + ProducersDone.ToString() + " CarriersDone:" + CarriersDone.ToString();
            var StartDate = Convert.ToDateTime(hfStartingTime.Value);
            TimeSpan ts = DateTime.Now - StartDate;


            this.lblPrompt.Text = WhatsDone + WhatsDownloading + "<br/><br/><hr/>Total Elapsed Time:" + ts.ToString(@"hh\:mm\:ss");

       
    }


}
