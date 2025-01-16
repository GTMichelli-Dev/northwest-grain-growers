using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Audit
/// </summary>
public class Audit
{
    public Audit()
    {
        
    }

    public static void AddAuditTrail(string Type_Of_Audit,int Location_Id, string Record_Id, string Description, string OldValue, string NewValue)
    {
        try
        {
            string ServerName = "";
            using (AuditDataSetTableAdapters.QueriesTableAdapter Q = new AuditDataSetTableAdapters.QueriesTableAdapter())
            {
                ServerName = Q.GetServerName();
            }
            using (AuditDataSetTableAdapters.Audit_TrailTableAdapter audit_TrailTableAdapter = new AuditDataSetTableAdapters.Audit_TrailTableAdapter())
            {
                using (AuditDataSet.Audit_TrailDataTable audit_TrailDataTable = new AuditDataSet.Audit_TrailDataTable())
                {
                    AuditDataSet.Audit_TrailRow row = audit_TrailDataTable.NewAudit_TrailRow();
                    row.UID = Guid.NewGuid();
                    row.Type_Of_Audit = Type_Of_Audit;
                    row.Audit_Date = DateTime.Now;
                    row.Server_Name = ServerName;
                    row.Description = Description;
                    row.Record_Id = Record_Id;
                    row.Operator = "Administrator";
                    row.Location_Id = Location_Id;
                    row.Old_Value = OldValue;
                    row.New_Value = NewValue;
                    row.Reason = "";
                    audit_TrailDataTable.AddAudit_TrailRow(row);
                    audit_TrailTableAdapter.Update(audit_TrailDataTable);

                }
            }
        }
        catch (Exception ex)
        {


        }
    }


   
}