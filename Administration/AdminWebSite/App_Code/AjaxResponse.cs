using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for AjaxResponse
/// </summary>
public class AjaxResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string Data { get; set; }
    

    public AjaxResponse()
    {
        Success = false;
        Message = "";
        Data = "";
    }
}