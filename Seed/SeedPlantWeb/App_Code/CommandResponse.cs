using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CommandResponse
/// </summary>
public class CommandResponse
{
    public CommandResponse()
    {
        Message = "";
        Result = enumResult.Unknown; 
    }

    public enum enumResult
    {
        Unknown,
        Success,
        Failure
    }




    public string  Message { get; set; }

    public enumResult Result { get; set; }

}