using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

/// <summary>
/// Override the method TrySetMobileMasterPage by returning false while it tries to find the site.mobile.master page in the solution
/// </summary>
public class SiteMobileMasterFriendlyUrlResolver : Microsoft.AspNet.FriendlyUrls.Resolvers.WebFormsFriendlyUrlResolver
{
    protected override bool TrySetMobileMasterPage(HttpContextBase httpContext, Page page, string mobileSuffix)
    {
        return false;
        //return base.TrySetMobileMasterPage(httpContext, page, mobileSuffix);
    }
}