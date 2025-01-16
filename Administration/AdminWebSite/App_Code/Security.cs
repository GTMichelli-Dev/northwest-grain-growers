using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Security
/// </summary>
public class Security
{

    const string SuperPass = "S";// "$up3r123";
    const string AdminPass = "harvest2020";//"@dm1n$";

    public Security()
    {

    }

    


    public static bool ChangeUserSecurityLevel(System.Web.SessionState.HttpSessionState Session ,enumSecurityLevel NewLevel, string Password)
    {
        bool Retval = false;
        enumSecurityLevel UserSecurityLevel = GetUsersSecurityLevel(Session);
        if (NewLevel!=UserSecurityLevel )
        {
            if (NewLevel== enumSecurityLevel.Administrator )
            {
                if (Password==AdminPass )
                {
                    SetUserSecurityLevel(Session, NewLevel);
                    Retval = true;
                    
                }

            }
            else if (NewLevel== enumSecurityLevel.Supervisor )
            {
                if(Password== SuperPass  )
                {
                    SetUserSecurityLevel(Session, NewLevel);
                    Retval = true;
                    
                }
            }
            else
            {
                SetUserSecurityLevel(Session, enumSecurityLevel.View);
                Retval = true;
            }

        }
        else
        {
            Retval = true;
        }
        return Retval;
    }




    public static  enumSecurityLevel  GetUsersSecurityLevel(System.Web.SessionState.HttpSessionState Session )
    {
        if (Session["SecurityLevel"] == null) Session["SecurityLevel"] = enumSecurityLevel.View;
        return (enumSecurityLevel)Session["SecurityLevel"];
    }

    private static  void SetUserSecurityLevel(System.Web.SessionState.HttpSessionState Session ,enumSecurityLevel NewLevel)
    {
        Session["SecurityLevel"] = NewLevel;
    }


    public enum enumSecurityLevel
    {
        View,
        Supervisor,
        Administrator
    }





    
}