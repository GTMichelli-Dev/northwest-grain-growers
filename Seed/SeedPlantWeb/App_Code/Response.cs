using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Response
/// </summary>
public class Response
{
    public Response()
    {
    }

    public enum enumResponse
    {
        Ok,
        Fail
    }

    public enumResponse  Results { get; set; }

    public string  Message { get; set; }

}