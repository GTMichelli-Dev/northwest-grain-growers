using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Text;

/// <summary>
/// Summary description for ValidationErrors
/// </summary>
public class ValidationErrors
{
    public  ValidationErrors()
    {
        //
        // TODO: Add constructor logic here
        //
        
    }

    public static string getValidationErrors(DbEntityValidationException e)
    {
        StringBuilder sb = new StringBuilder();
            foreach (var eve in e.EntityValidationErrors)
            {
                sb.AppendLine(string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State));
                foreach (var ve in eve.ValidationErrors)
                {
                    sb.AppendLine(string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage));
                }
            }
            return sb.ToString();
        }
    
}