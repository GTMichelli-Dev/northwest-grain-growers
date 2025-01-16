using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


/// <summary>
/// Summary description for Auditing
/// </summary>
public class Auditing
{
    public Auditing()
    {
        //
        // TODO: Add constructor logic here
        //
    }


    public static void LogMessage(string Code_Source, string Message)
    {
        try
        {
            using (AuditDataSetTableAdapters.QueriesTableAdapter Q = new AuditDataSetTableAdapters.QueriesTableAdapter())
            {
                Q.SaveLog(Code_Source, Message, GlobalVars.Location, HttpContext.Current.Request.UserHostAddress, HttpContext.Current.User.Identity.Name);
            }
        }
        catch
        {

        }
    }

    public static void ReportTicketCreated( SeedTicketDataSet.Seed_TicketsRow seedTicketRow)
    {
        using (AuditDataSet auditDataSet = new global::AuditDataSet())
        {
            using (AuditDataSetTableAdapters.Audit_TrailTableAdapter audit_TrailTableAdapter = new AuditDataSetTableAdapters.Audit_TrailTableAdapter())
            {
                AuditDataSet.Audit_TrailRow row = auditDataSet.Audit_Trail.NewAudit_TrailRow();
                row.UID = Guid.NewGuid();
                row.Operator = HttpContext.Current.User.Identity.Name;
                row.Location_Id = GlobalVars.Location;
                row.PC_Address = HttpContext.Current.Request.UserHostAddress;
                row.Type_Of_Audit = "New Ticket";
                row.Record_UID = seedTicketRow.UID;
                row.Audit_Date = DateTime.Now;
                row.Description = "New Ticket Created";
                row.New_Value = seedTicketRow.Ticket.ToString();
                auditDataSet.Audit_Trail.AddAudit_TrailRow(row);
                audit_TrailTableAdapter.Update(auditDataSet);
            }
        }
    }
}