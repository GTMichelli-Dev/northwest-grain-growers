using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

public class Cookies
{
    public Cookies()
    {
        //
        // TODO: Add constructor logic here
        //
    }



 

    /// <summary>
    /// Stores a value in a user Cookie, creating it if it doesn't exists yet.
    /// </summary>
    public static void StoreInCookie(
        string cookieName,
        string cookieDomain,
        string keyName,
        string value,
        DateTime? expirationDate,
        bool httpOnly = false)
    {
        // NOTE: we have to look first in the response, and then in the request.
        // This is required when we update multiple keys inside the cookie.
        HttpCookie cookie = HttpContext.Current.Response.Cookies[cookieName]
            ?? HttpContext.Current.Request.Cookies[cookieName];
        if (cookie == null) cookie = new HttpCookie(cookieName);
        if (!String.IsNullOrEmpty(keyName)) cookie.Values.Set(keyName, value);
        else cookie.Value = value;
        if (expirationDate.HasValue) cookie.Expires = expirationDate.Value;
        if (!String.IsNullOrEmpty(cookieDomain)) cookie.Domain = cookieDomain;
        if (httpOnly) cookie.HttpOnly = true;
        HttpContext.Current.Response.Cookies.Set(cookie);
    }

    /// <summary>
    /// Stores multiple values in a Cookie using a key-value dictionary, 
    ///  creating the cookie (and/or the key) if it doesn't exists yet.
    /// </summary>
    public static void StoreInCookie(
        string cookieName,
        string cookieDomain,
        Dictionary<string, string> keyValueDictionary,
        DateTime? expirationDate,
        bool httpOnly = false)
    {
        // NOTE: we have to look first in the response, and then in the request.
        // This is required when we update multiple keys inside the cookie.
        HttpCookie cookie = HttpContext.Current.Response.Cookies[cookieName]
            ?? HttpContext.Current.Request.Cookies[cookieName];
        if (cookie == null) cookie = new HttpCookie(cookieName);
        if (keyValueDictionary == null || keyValueDictionary.Count == 0)
            cookie.Value = null;
        else
            foreach (var kvp in keyValueDictionary)
                cookie.Values.Set(kvp.Key, kvp.Value);
        if (expirationDate.HasValue) cookie.Expires = expirationDate.Value;
        if (!String.IsNullOrEmpty(cookieDomain)) cookie.Domain = cookieDomain;
        if (httpOnly) cookie.HttpOnly = true;
        HttpContext.Current.Response.Cookies.Set(cookie);
    }

    /// <summary>
    /// Retrieves a single value from Request.Cookies
    /// </summary>
    public static string GetFromCookie(string cookieName, string keyName)
    {
        HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
        if (cookie != null)
        {
            string val = (!String.IsNullOrEmpty(keyName)) ? cookie[keyName] : cookie.Value;
            if (!String.IsNullOrEmpty(val)) return Uri.UnescapeDataString(val);
        }
        return null;
    }


    /// <summary>
    /// Removes a single value from a cookie or the whole cookie (if keyName is null)
    /// </summary>
    public static void RemoveCookie(string cookieName, string keyName, string domain = null)
    {
        if (String.IsNullOrEmpty(keyName))
        {
            if (HttpContext.Current.Request.Cookies[cookieName] != null)
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
                cookie.Expires = DateTime.UtcNow.AddYears(-1);
                if (!String.IsNullOrEmpty(domain)) cookie.Domain = domain;
                HttpContext.Current.Response.Cookies.Add(cookie);
                HttpContext.Current.Request.Cookies.Remove(cookieName);
            }
        }
        else
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[cookieName];
            cookie.Values.Remove(keyName);
            if (!String.IsNullOrEmpty(domain)) cookie.Domain = domain;
            HttpContext.Current.Response.Cookies.Add(cookie);
        }
    }

    /// <summary>
    /// Checks if a cookie / key exists in the current HttpContext.
    /// </summary>
    public static bool CookieExist(string cookieName, string keyName)
    {
        HttpCookieCollection cookies = HttpContext.Current.Request.Cookies;
        return (String.IsNullOrEmpty(keyName))
            ? cookies[cookieName] != null
            : cookies[cookieName] != null && cookies[cookieName][keyName] != null;
    }
}