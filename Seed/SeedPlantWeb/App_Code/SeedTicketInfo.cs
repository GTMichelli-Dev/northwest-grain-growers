using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for SeedTicket
/// </summary>
public class SeedTicketInfo
{
    public SeedTicketInfo()
    {
        //
        // TODO: Add constructor logic here
        //
    }



    #region ReportDataset

    public static  ReportDataSet GetAllTicketInfo(SeedTicketDataSet seedTicketDataset, Guid UID)
    {
        ReportDataSet reportDataSet = new global::ReportDataSet();
        SeedTicketDataSet.Seed_TicketsRow seedRow = seedTicketDataset.Seed_Tickets.FirstOrDefault(x => x.UID == UID);
        if (seedRow == null)
        {
            return null;
        }
        else
        {
            decimal TotalNet = GetVarietyTotalWeights(seedTicketDataset);
            ListDataSet.ProducersListRow pRow = null;
            if (!seedRow.IsGrower_IDNull())
            {
                using (ListDataSetTableAdapters.ProducersListTableAdapter producersListTableAdapter = new ListDataSetTableAdapters.ProducersListTableAdapter())
                {
                    using (ListDataSet.ProducersListDataTable producersListDataTable = new ListDataSet.ProducersListDataTable())
                    {
                        if (producersListTableAdapter.FillByID(producersListDataTable, seedRow.Grower_ID) > 0)
                        {
                            pRow = producersListDataTable[0];
                        }
                    }
                }
            }

            ReportDataSet.TicketsRow ticketRow = reportDataSet.Tickets.NewTicketsRow();
            ticketRow.UID = seedRow.UID;
            ticketRow.ReadOnly = seedRow.ReadOnly;
            if(!seedRow.IsTicketNull()) ticketRow.Ticket = seedRow.Ticket;

            ticketRow.Location_ID = seedRow.Location_ID;
            ticketRow.Completed = !seedRow.IsTicketNull();
            ticketRow.Ticket_Date = seedRow.Ticket_Date;
            ticketRow.Ticket_Type = seedRow.Ticket_Type;
            ticketRow.Manual = seedRow.Manual;
            if (!seedRow.IsTruck_IDNull()) ticketRow.Truck_ID = seedRow.Truck_ID;
            if (!seedRow.IsGrower_IDNull()) ticketRow.Grower_ID = seedRow.Grower_ID;
            if (!seedRow.IsBOLNull()) ticketRow.BOL = seedRow.BOL;
            if (!seedRow.IsPONull()) ticketRow.PO = seedRow.PO;
            if (!seedRow.IsBag_CntNull()) ticketRow.Bag_Cnt = seedRow.Bag_Cnt;
            if (!seedRow.IsBag_SizeNull()) ticketRow.Bag_Size = seedRow.Bag_Size;
            if (!seedRow.IsWeighmasterNull()) ticketRow.Weighmaster = seedRow.Weighmaster;
            if (!seedRow.IsCommentsNull()) ticketRow.Comments = seedRow.Comments;
            if (!seedRow.IsGrower_IDNull()) ticketRow.Description = (pRow == null) ? seedRow.Grower_Name  : pRow.Description;
            reportDataSet.Tickets.AddTicketsRow(ticketRow);





            //Ticket Varieties
            using (ListDataSet.SeedVarietiesDataTable seedVarietiesDataTable = new ListDataSet.SeedVarietiesDataTable())
            {
                using (ListDataSetTableAdapters.SeedVarietiesTableAdapter seedVarietiesTableAdapter = new ListDataSetTableAdapters.SeedVarietiesTableAdapter())
                {
                    seedVarietiesTableAdapter.FillByAllForLocation(seedVarietiesDataTable, GlobalVars.Location);
                    using (SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter seed_TicketsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter())
                    {
                        foreach (SeedTicketDataSet.Seed_Ticket_VarietiesRow vRow in seedTicketDataSet.Seed_Ticket_Varieties)
                        {
                            if (vRow.RowState != System.Data.DataRowState.Deleted)
                            {
                                ReportDataSet.Ticket_VarietiesRow ticketVarietyRow = reportDataSet.Ticket_Varieties.NewTicket_VarietiesRow();
                                ticketVarietyRow.UID = vRow.UID;
                                ticketVarietyRow.Seed_Ticket_UID = vRow.Seed_Ticket_UID;

                                if (!vRow.IsVariety_IDNull()) ticketVarietyRow.Variety_ID = vRow.Variety_ID;
                                if (!vRow.IsPercent_Of_LoadNull())
                                {
                                    decimal Percent = 0;
                                    if (!vRow.IsPercent_Of_LoadNull())
                                    {
                                        Percent = Convert.ToDecimal(vRow.Percent_Of_Load * 0.01);
                                        ticketVarietyRow.Percent_Of_Load = vRow.Percent_Of_Load;
                                        ticketVarietyRow.Total = TotalNet * Percent;
                                    }
                                }
                                if (!vRow.IsCommentNull()) ticketVarietyRow.Comment = vRow.Comment;
                                if (!vRow.IsVariety_IDNull() && vRow.Variety_ID > 0)
                                {
                                    var VarietyRow = seedVarietiesDataTable.FirstOrDefault(x => x.ID == vRow.Variety_ID);
                                    {
                                        if (VarietyRow != null)
                                        {
                                            ticketVarietyRow.Description = VarietyRow.Description;
                                        }

                                    }
                                }
                                else
                                {
                                    if (!vRow.IsCustom_NameNull()) ticketVarietyRow.Description = vRow.Custom_Name;
                                }
                                if (!vRow.IsBinNull()) ticketVarietyRow.Bin = vRow.Bin;
                                if (!vRow.IsBin_NameNull()) ticketVarietyRow.Bin_Name = vRow.Bin_Name;
                                reportDataSet.Ticket_Varieties.AddTicket_VarietiesRow(ticketVarietyRow);
                            }
                        }
                    }
                }
            }



            //Ticket Treatments
            using (ListDataSet.SeedChemicalsDataTable seedChemicalsDataTable = new ListDataSet.SeedChemicalsDataTable())
            {
                using (ListDataSetTableAdapters.SeedChemicalsTableAdapter seedChemicalsTableAdapter = new ListDataSetTableAdapters.SeedChemicalsTableAdapter())

                {
                    seedChemicalsTableAdapter.FillByAllForLocation(seedChemicalsDataTable, GlobalVars.Location);
                    foreach (SeedTicketDataSet.Seed_Ticket_TreatmentsRow Trow in seedTicketDataset.Seed_Ticket_Treatments)
                    {
                        if (Trow.RowState != System.Data.DataRowState.Deleted)
                        {
                            ReportDataSet.Ticket_TreatmentsRow TreatRow = reportDataSet.Ticket_Treatments.NewTicket_TreatmentsRow();
                            TreatRow.UID = Trow.UID;
                            TreatRow.Seed_Ticket_UID = Trow.UID;
                            if (!Trow.IsTreatment_IDNull())
                            {
                                TreatRow.ID = Trow.Treatment_ID;
                                var treatmentRow = seedChemicalsDataTable.FirstOrDefault(x => x.ID == TreatRow.ID);
                                if (treatmentRow != null)
                                {
                                    TreatRow.Description = treatmentRow.Description;
                                }
                                else
                                {
                                    if (!Trow.IsCustom_NameNull()) TreatRow.Description = Trow.Custom_Name;
                                }

                            }

                            if (!Trow.IsCommentNull()) TreatRow.Comment = Trow.Comment;

                            if (!Trow.IsRateNull())
                            {
                                TreatRow.Rate = Trow.Rate;
                                TreatRow.Total = ((decimal)TotalNet / 100) * ((decimal)TreatRow.Rate / 128);
                            }

                            reportDataSet.Ticket_Treatments.AddTicket_TreatmentsRow(TreatRow);
                        }
                    }

                }
            }


        




                //ticketWeights
                foreach (var SeedWeightRow in seedTicketDataSet.Seed_Ticket_Weights)
                {
                    var TicketWeightRow = reportDataSet.Ticket_Weights.NewTicket_WeightsRow();
                    TicketWeightRow.UID = SeedWeightRow.UID;
                    TicketWeightRow.Seed_Ticket_UID = SeedWeightRow.Seed_Ticket_UID;
                    if (!SeedWeightRow.IsStarting_WeightNull()) TicketWeightRow.Starting_Weight = SeedWeightRow.Starting_Weight;
                    if (!SeedWeightRow.IsStarting_TimeNull()) TicketWeightRow.Starting_Time = SeedWeightRow.Starting_Time;
                    if (!SeedWeightRow.IsManual_InNull()) TicketWeightRow.Manual_In = SeedWeightRow.Manual_In;
                    if (!SeedWeightRow.IsEnding_TimeNull()) TicketWeightRow.Ending_Time = SeedWeightRow.Ending_Time;
                    if (!SeedWeightRow.IsEnding_WeightNull()) TicketWeightRow.Ending_Weight = SeedWeightRow.Ending_Weight;
                    if (!SeedWeightRow.IsManual_OutNull()) TicketWeightRow.Manual_Out = SeedWeightRow.Manual_Out;
                    reportDataSet.Ticket_Weights.AddTicket_WeightsRow(TicketWeightRow);
                }


            return reportDataSet;
        }
        
    }



    public static decimal GetVarietyTotalWeights(SeedTicketDataSet seedTicketDataset)
    {
        float Total = 0;
        foreach (var row in seedTicketDataset.Seed_Ticket_Weights)
        {
            if (!row.IsEnding_WeightNull())
            {
                Total += Math.Abs(row.Ending_Weight - row.Starting_Weight);
            }
        }
        return Convert.ToDecimal(Total);
    }

    #endregion





    public static CommandResponse  Ticketcomplete()
    {

        CommandResponse cr = new CommandResponse();
        cr.Result = CommandResponse.enumResult.Failure;
        try
        {


            bool Complete = false;
            {
                {

                    Complete = (seedTicketDataSet.Seed_Ticket_Weights.Count > 0);
                    if (!Complete)
                    {
                        cr.Message = "No Weight(s) For Ticket";
                    }
                    else
                    {
                        foreach (var item in seedTicketDataSet.Seed_Ticket_Weights)
                        {
                            if (item.IsEnding_TimeNull())
                            {
                                cr.Message = "Weight Is Not Complete";
                                Complete = false;
                                break;
                            }
                        }
                        if (Complete)
                        {
                            Complete = seedTicketDataSet.Seed_Ticket_Varieties.Count > 0;
                            if (!Complete)
                            {
                                cr.Message = "No Varieties Selected";
                            }
                            else
                            {

                                if (CurrentSeedTicketRow.Ticket_Type == "Bag")
                                {
                                    Complete = ((!CurrentSeedTicketRow.IsBag_CntNull() && CurrentSeedTicketRow.Bag_Cnt > 0) || (!CurrentSeedTicketRow.IsBag_SizeNull() && CurrentSeedTicketRow.Bag_Size > 0));
                                    if (!Complete)
                                    {
                                        cr.Message = "Bag Count Not Correct";

                                    }

                                }

                                if (Complete)
                                {
                                    Complete = (!CurrentSeedTicketRow.IsGrower_IDNull());
                                    if (!Complete)
                                    {
                                        cr.Message = "Grower Not Set";
                                    }
                                    else
                                    {
                                        Complete = (!CurrentSeedTicketRow.IsWeighmasterNull());
                                        if (!Complete)
                                        {
                                            cr.Message = "Weighmaster Not Set";
                                        }
                                        else
                                        {
                                            cr.Result = CommandResponse.enumResult.Success;
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
            }
        }
        catch
        {

        }
        return cr;
    }


    public static SeedTicketDataSet.Seed_TicketsRow CreateSeedTicket()
    {

        seedTicketDataSet = new SeedTicketDataSet();
        SeedTicketDataSet.Seed_TicketsRow row = seedTicketDataSet.Seed_Tickets.NewSeed_TicketsRow();
        row.UID = Guid.NewGuid();
        row.ReadOnly = false;
        row.Location_ID = GlobalVars.Location;
        row.Manual = false;
        row.Ticket_Type = "Truck";
        row.Ticket_Date = DateTime.Now;
        row.Pending = false;
        row.Void = false;
        row.Returned = false;
       
        AddToAudit(enumAuditType.Create, row.UID, "Ticket Created");


        seedTicketDataSet.Seed_Tickets.AddSeed_TicketsRow(row);
        HttpContext.Current.Session["CurrentSeedTicketWeightRow"] = null;
        HttpContext.Current.Session["CurrentSeedTicketVarietyRow"] = null;
        HttpContext.Current.Session["CurrentSeedTicketTreatmentRow"] = null;

        return seedTicketDataSet.Seed_Tickets[0];
    }


    public static void CompleteTicket()
    {
        SeedTicketInfo.CurrentSeedTicketRow.Ticket_Date = DateTime.Now;
        SeedTicketInfo.CurrentSeedTicketRow.Pending = false;
        SeedTicketInfo.CurrentSeedTicketRow.Weighmaster = HttpContext.Current.User.Identity.Name;
        SaveTicket();
        using (SeedTicketDataSetTableAdapters.QueriesTableAdapter Q = new SeedTicketDataSetTableAdapters.QueriesTableAdapter())
        {
            Q.CompleteTicket(CurrentSeedTicketRow.UID);
        }
        if (TicketWeighType== GlobalVars.enumSeedTicketWeighType.Truck || TicketWeighType == GlobalVars.enumSeedTicketWeighType.ReturnBulk ) Camera.TakePicture(CurrentSeedTicketRow.UID);


    }
    
    

    public static CommandResponse  SaveTicket()
    {


        CommandResponse tc = Ticketcomplete();

        

        CommandResponse Response = new global::CommandResponse();

        if (seedTicketDataSet != null)
        {
            try
            {
                try
                {
                    CleanUpCurrentPercents();
                }
                catch { }
                using (SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter seed_TicketsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter())
                {
                    
                    seed_TicketsTableAdapter.Update(seedTicketDataSet);
                }
                using (SeedTicketDataSetTableAdapters.Seed_Ticket_TreatmentsTableAdapter seed_Ticket_TreatmentsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_TreatmentsTableAdapter())
                {
                    seed_Ticket_TreatmentsTableAdapter.Update(seedTicketDataSet);
                }
                using (SeedTicketDataSetTableAdapters.Seed_Ticket_VarietiesTableAdapter seed_Ticket_VarietiesTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_VarietiesTableAdapter())
                {
                    seed_Ticket_VarietiesTableAdapter.Update(seedTicketDataSet);
                }
                using (SeedTicketDataSetTableAdapters.Seed_Ticket_WeightsTableAdapter seed_Ticket_WeightsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_WeightsTableAdapter())
                {
                    seed_Ticket_WeightsTableAdapter.Update(seedTicketDataSet);
                }
                using (SeedTicketDataSetTableAdapters.Seed_Ticket_MiscTableAdapter seed_Ticket_MiscTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_MiscTableAdapter())
                {
                    seed_Ticket_MiscTableAdapter.Update(seedTicketDataSet);
                }
                    Response.Result = CommandResponse.enumResult.Success;
                seedTicketDataSet.AcceptChanges();
            }
            catch(Exception ex)
            {
                Auditing.LogMessage("SeedTicketInfo.Saveticket()", ex.ToString());
                Response.Message = ex.Message;
                Response.Result = CommandResponse.enumResult.Failure;
            }

        }
        else
        {
            Auditing.LogMessage("SeedTicketInfo.Saveticket()","SeedTicketDataSet==null");
            Response.Message = "SeedTicketDataSet==null";
            Response.Result = CommandResponse.enumResult.Failure;

        }
        return Response;
    }

    public static SeedTicketDataSet GetSeedTicketDataset(Guid UID)
    {
        using (SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter seed_TicketsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_TicketsTableAdapter())
        {
            using (SeedTicketDataSetTableAdapters.Seed_Ticket_TreatmentsTableAdapter seed_Ticket_TreatmentsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_TreatmentsTableAdapter())
            {
                using (SeedTicketDataSetTableAdapters.Seed_Ticket_VarietiesTableAdapter seed_Ticket_VarietiesTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_VarietiesTableAdapter())
                {
                    using (SeedTicketDataSetTableAdapters.Seed_Ticket_MiscTableAdapter seed_Ticket_MiscTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_MiscTableAdapter())
                    {
                        using (SeedTicketDataSetTableAdapters.Seed_Ticket_WeightsTableAdapter seed_Ticket_WeightsTableAdapter = new SeedTicketDataSetTableAdapters.Seed_Ticket_WeightsTableAdapter())
                        {
                            seedTicketDataSet = new SeedTicketDataSet();
                            seed_TicketsTableAdapter.FillByUID(seedTicketDataSet.Seed_Tickets, UID);
                            if (seedTicketDataSet.Seed_Tickets.Count > 0) CurrentSeedTicketRow = seedTicketDataSet.Seed_Tickets[0];
                            seed_Ticket_TreatmentsTableAdapter.FillBySeed_Ticket_UID(seedTicketDataSet.Seed_Ticket_Treatments, UID);
                            seed_Ticket_VarietiesTableAdapter.FillBySeed_Ticket_UID(seedTicketDataSet.Seed_Ticket_Varieties, UID);
                            seed_Ticket_WeightsTableAdapter.FillBySeed_Ticket_UID(seedTicketDataSet.Seed_Ticket_Weights, UID);
                            seed_Ticket_MiscTableAdapter.FillBySeedTicketUID(seedTicketDataSet.Seed_Ticket_Misc, UID);
                            if (seedTicketDataSet.Seed_Ticket_Weights.Count > 0) CurrentSeedTicketWeightRow = seedTicketDataSet.Seed_Ticket_Weights[0];
                        }
                    }
                }
            }









        }
        return seedTicketDataSet;
    }


    public static SeedTicketDataSet.Seed_Ticket_VarietiesRow GetSeedTicketVarietyRow(Guid UID)
    {
        CurrentSeedTicketVarietyRow = seedTicketDataSet.Seed_Ticket_Varieties.FindByUID(UID);
        return CurrentSeedTicketVarietyRow;

    }


    public static SeedTicketDataSet.Seed_Ticket_TreatmentsRow  GetSeedTicketTreatmentRow(Guid UID)
    {
        CurrentSeedTicketTreatmentRow = seedTicketDataSet.Seed_Ticket_Treatments.FindByUID(UID);
        return CurrentSeedTicketTreatmentRow;

    }


    public static SeedTicketDataSet.Seed_Ticket_VarietiesRow CreateSeedTicketVarietyRow()
    {
        CurrentSeedTicketVarietyRow = seedTicketDataSet.Seed_Ticket_Varieties.NewSeed_Ticket_VarietiesRow();
        CurrentSeedTicketVarietyRow.UID = Guid.NewGuid();
        CurrentSeedTicketVarietyRow.Seed_Ticket_UID = CurrentSeedTicketRow.UID;
        CurrentSeedTicketVarietyRow.PC_Address = HttpContext.Current.Request.UserHostAddress;
        return CurrentSeedTicketVarietyRow;

    }


    public static SeedTicketDataSet.Seed_Ticket_TreatmentsRow CreateSeedTicketTreatmentRow()
    {
        CurrentSeedTicketTreatmentRow = seedTicketDataSet.Seed_Ticket_Treatments.NewSeed_Ticket_TreatmentsRow();
        CurrentSeedTicketTreatmentRow.UID = Guid.NewGuid();
        CurrentSeedTicketTreatmentRow.Seed_Ticket_UID = CurrentSeedTicketRow.UID;
        CurrentSeedTicketTreatmentRow.PC_Address = HttpContext.Current.Request.UserHostAddress;
        return CurrentSeedTicketTreatmentRow;

    }


    public static SeedTicketDataSet seedTicketDataSet
    {
        get
        {
            if (HttpContext.Current.Session["seedTicketDataSet"] == null)
            {
                return null;
            }
            else
            {
                try
                {
                    return (SeedTicketDataSet)HttpContext.Current.Session["seedTicketDataSet"];
                }
                catch
                {
                    return null;
                }
            }
        }
        set
        {
            HttpContext.Current.Session["seedTicketDataSet"] = value;
        }
    }





    public static SeedTicketDataSet.Seed_TicketsRow CurrentSeedTicketRow
    {
        get
        {
            if (seedTicketDataSet == null)
            {
                return null;
            }
            else
            {
                if (seedTicketDataSet.Seed_Tickets.Count > 0)
                {
                    return seedTicketDataSet.Seed_Tickets[0];
                }
                else
                {
                    return CreateSeedTicket();
                }
            }
        }
        set
        {
            SeedTicketDataSet.Seed_TicketsRow row =(value!= null)? seedTicketDataSet.Seed_Tickets.FindByUID(value.UID): null;
            if (row == null)
            {
                seedTicketDataSet.Seed_Tickets.Clear();
               // seedTicketDataSet.Seed_Tickets.AddSeed_TicketsRow(row);
            }
            else
            {
                seedTicketDataSet.Seed_Tickets[0].ItemArray = value.ItemArray;
            }
        }
    }



    public static SeedTicketDataSet.Seed_Ticket_TreatmentsRow CurrentSeedTicketTreatmentRow
    {
        get
        {

            try
            {
                return (SeedTicketDataSet.Seed_Ticket_TreatmentsRow)HttpContext.Current.Session["CurrentSeedTicketTreatmentRow"];
            }
            catch
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentSeedTicketTreatmentRow"] = value;
        }
    }



    public static SeedTicketDataSet.Seed_Ticket_VarietiesRow CurrentSeedTicketVarietyRow
    {
        get
        {

            try
            {
                return (SeedTicketDataSet.Seed_Ticket_VarietiesRow)HttpContext.Current.Session["CurrentSeedTicketVarietyRow"];
            }
            catch
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentSeedTicketVarietyRow"] = value;
        }
    }



    public static SeedTicketDataSet.Seed_Ticket_MiscRow  CurrentSeedTicketMiscRow
    {
        get
        {

            try
            {
                return (SeedTicketDataSet.Seed_Ticket_MiscRow)HttpContext.Current.Session["CurrentSeedTicketMiscRow"];
            }
            catch
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentSeedTicketMiscRow"] = value;
        }
    }


    public static SeedTicketDataSet.Seed_Ticket_WeightsRow CurrentSeedTicketWeightRow
    {
        get
        {

            try
            {
                return (SeedTicketDataSet.Seed_Ticket_WeightsRow)HttpContext.Current.Session["CurrentSeedTicketWeightRow"];
            }
            catch
            {
                return null;
            }

        }
        set
        {
            HttpContext.Current.Session["CurrentSeedTicketWeightRow"] = value;
        }
    }


    public static SeedTicketDataSet.Seed_Ticket_WeightsRow CreateSeedTicketWeightsRow()
    {
        if (seedTicketDataSet == null) CreateSeedTicket();
        CurrentSeedTicketWeightRow = seedTicketDataSet.Seed_Ticket_Weights.NewSeed_Ticket_WeightsRow();
        CurrentSeedTicketWeightRow.UID = Guid.NewGuid();
        CurrentSeedTicketWeightRow.Seed_Ticket_UID = CurrentSeedTicketRow.UID;
        CurrentSeedTicketWeightRow.Creation_Date = DateTime.Now;
        CurrentSeedTicketWeightRow.PC_Weight_In_Address = HttpContext.Current.Request.UserHostAddress;
        seedTicketDataSet.Seed_Ticket_Weights.AddSeed_Ticket_WeightsRow(CurrentSeedTicketWeightRow);
        return CurrentSeedTicketWeightRow;

    }

    public static GlobalVars.enumSeedTicketWeighType TicketWeighType
    {
        get
        {

            return (GlobalVars.enumSeedTicketWeighType)Enum.Parse(typeof(GlobalVars.enumSeedTicketWeighType), SeedTicketInfo.CurrentSeedTicketRow.Ticket_Type);
        }
    }


    public static bool TicketComplete
    {
        get
        {
            if (CurrentSeedTicketRow == null)
            {
                return false;

            }
            else
            {
                if (CurrentSeedTicketRow.IsTicketNull())
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }


    public enum enumAuditType
    {
        Create,
        Delete,
        Update
       



    }

    public static void AddToAudit(enumAuditType AuditType,Guid RecordUID,string Description,string NewValue ="",string OldValue="" ,string Reason= "" )
    {
        try
        {
            var row = seedTicketDataSet.Audit_Trail.NewAudit_TrailRow();
            row.UID = Guid.NewGuid();
            row.Type_Of_Audit = AuditType.ToString();
            row.Record_UID = RecordUID; 
            row.Operator = System.Web.HttpContext.Current.User.Identity.Name ;
            row.Location_Id = GlobalVars.Location;
            row.PC_Address = HttpContext.Current.Request.UserHostAddress;
            row.Audit_Date = DateTime.Now;
            row.Description = Description;
            row.Old_Value = OldValue;
            row.New_Value = NewValue;
            row.Reason = Reason;
            seedTicketDataSet.Audit_Trail.AddAudit_TrailRow(row);
        }
        catch (Exception ex)
        {
            Auditing.LogMessage("SeedTicketInfo.AddToAudit", ex.Message);

        }
    }

    public static void ResetWeights()
    {
        
        SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Weights.Clear();
        SeedTicketInfo.CurrentSeedTicketWeightRow = null;
        SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.Clear();
        SeedTicketInfo.CurrentSeedTicketRow.SetBag_CntNull();
        SeedTicketInfo.CurrentSeedTicketRow.SetBag_SizeNull();
       
        SeedTicketInfo.CurrentSeedTicketRow.Ticket_Type = GlobalVars.SeedTicketWeighType.ToString();
    }



    public static void AddBag(int Quantity)
    {
        using (ListDataSetTableAdapters.Misc_ItemsTableAdapter misc_ItemsTableAdapter = new ListDataSetTableAdapters.Misc_ItemsTableAdapter())
        {
            using (ListDataSet.Misc_ItemsDataTable misc_ItemsDataTable = new ListDataSet.Misc_ItemsDataTable())
            {
                misc_ItemsTableAdapter.Fill(misc_ItemsDataTable, GlobalVars.Location);
                ListDataSet.Misc_ItemsRow Lrow = misc_ItemsDataTable.FirstOrDefault(x => x.Description.ToUpper() == "BAGS");
                if (Lrow != null)
                {
                    if (SeedTicketInfo.CurrentSeedTicketRow != null)
                    {
                        bool NotFound;
                        int ID = Lrow.ID;

                        SeedTicketDataSet.Seed_Ticket_MiscRow row = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.FirstOrDefault(x => x.Item_Id == ID);
                        NotFound = (row == null);
                        if (NotFound)
                        {
                            row = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.NewSeed_Ticket_MiscRow();
                            row.UID = Guid.NewGuid();
                            row.Seed_Ticket_UID = SeedTicketInfo.CurrentSeedTicketRow.UID;
                            row.Item_Id = ID;
                            row.Description = Lrow.Description;
                            row.Quantity = 0;
                        }
                        row.Quantity = Quantity;
                        row.Comment = "";
                        row.Hidden = false;
                        if (NotFound) SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Misc.AddSeed_Ticket_MiscRow(row);

                    }
                }
            }
        }
       
    }




    public static void CleanUpCurrentPercents()
    {
        List<VarietyList> OriginalValues = new List<VarietyList>();
        //var Retval = new global::AddEditVariety.VarietyTotals() { Total = 0, Count = 0 };
        try
        {



            foreach (var Item in SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties)
            {
                OriginalValues.Add(new VarietyList(Item.UID, (int)Item.Percent_Of_Load));

            }
            RoundPercent rp = new RoundPercent(OriginalValues);
            if (rp.CanRoundValues)
            {
                List<VarietyList> NewValues = rp.RoundedValues;
                foreach (var item in NewValues)
                {
                    var row = SeedTicketInfo.seedTicketDataSet.Seed_Ticket_Varieties.FirstOrDefault(x => x.UID == item.UID);
                    if (row != null) row.Percent_Of_Load = item.Percent;

                }


            }


        }
        catch
        {

        }

    }
}