using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for KioskResponse
/// </summary>
public class KioskResponse
{

    public bool Success { get; set; }
    public string  Message1 { get; set; }
    public string Message2 { get; set; }
    public string Message3 { get; set; }

    public KioskResponse()
    {
        Success = false;
        Message1 = "";
        Message2 = "";
        Message3 = "";
    }

    public void SetMessage(bool success, string message1 = "", string message2 = "", string message3="")
    {
        Success = success;
        Message1 = message1;
        Message2 = message2;
        Message3 = message3;
    }

}