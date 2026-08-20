using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace NWGrain
{
    class Settings

    {

        private static NWDatasetTableAdapters.WorkStation_SetupTableAdapter workStation_SetupTableAdapter = new NWDatasetTableAdapters.WorkStation_SetupTableAdapter();

        private static NWDataset.WorkStation_SetupDataTable workStation_SetupDataTable;

        private static NWDataset.Site_SetupDataTable site_SetupDataTable;

        private static NWDatasetTableAdapters.Site_SetupTableAdapter site_SetupTableAdapter = new NWDatasetTableAdapters.Site_SetupTableAdapter();

        private static SettingsDataSetTableAdapters.QueriesTableAdapter SettingQueries = new SettingsDataSetTableAdapters.QueriesTableAdapter();

        private static NWDataset.LocationsRow  WorkStation_LocationRow;

        public static bool OkToStart;

        private static string serverName = string.Empty;

        public static class Sequence
        {
            public static int InboundSequence
            {
                get
                {
                    return (int)SettingQueries.GetInboundSequence(Settings.Location_Id);
                }
            }


            public static int LotSequence
            {
                get
                {
                    return (int)SettingQueries.GetLotSequence(Settings.Location_Id);
                }
            }


            public static int OutboundSequence
            {
                get
                {
                    return (int)SettingQueries.GetOutboundSequence(Settings.Location_Id);
                }
            }


            public static int LoadSequence
            {
                get
                {
                    return (int)SettingQueries.GetLoadSequence(Settings.Location_Id);
                }
            }


            public static int SeedSequence
            {
                get
                {
                    return (int)SettingQueries.GetSeedSequence(Settings.Location_Id);
                }
            }

        }

        public static NWDataset.WorkStation_SetupRow workStation_SetupRow
        {
            get
            {
                if (workStation_SetupDataTable == null)
                {
                    workStation_SetupDataTable = new NWDataset.WorkStation_SetupDataTable();
                    if (workStation_SetupTableAdapter.Fill(workStation_SetupDataTable, Environment.MachineName) == 0)
                    {
                        AddWorkStation(0);
                    }
                }

                return workStation_SetupDataTable[0];
            }

        }

        public static void SaveWorkStationSettings()
        {
            try
            {

                workStation_SetupTableAdapter.Update(workStation_SetupDataTable);
            }
            catch (Exception ex)
            {
                Logging.Add_System_Log("NWDataset.WorkStation_SetupRow workStation_SetupRow SET", ex.Message);
                Alert.Show("Error Saving WorkStation Information", "Error Saving WorkStation");

            }
        }

        public static void ResetWorkStation()
        {
            if (workStation_SetupDataTable != null)
            {
                workStation_SetupDataTable.Dispose();
                workStation_SetupDataTable = null;
                WorkStation_LocationRow = null;
            }
        }

        public static void ResetSite()
        {
            if (site_SetupDataTable != null)
            {
                site_SetupDataTable.Dispose();
                site_SetupDataTable = null;
            }
        }



        public static void Check_SiteSetup()
        {
            NWDataset.Site_SetupRow SiteRow = SiteSetup; 
        }

        public static NWDataset.Site_SetupRow SiteSetup
        {

            get
            {
                if (site_SetupDataTable == null || site_SetupDataTable.Count==0)
                {
                    site_SetupDataTable = new NWDataset.Site_SetupDataTable();
                    if (site_SetupTableAdapter.Fill(site_SetupDataTable, Location_Id) == 0)
                    {
   
                        using (SettingsDataSetTableAdapters.vwSiteLocationsTableAdapter VwSiteLocationsTableAdapter = new SettingsDataSetTableAdapters.vwSiteLocationsTableAdapter())
                        {
                            using (SettingsDataSet.vwSiteLocationsDataTable VwSiteLocationsDataTable = new SettingsDataSet.vwSiteLocationsDataTable())
                            {
                                if (VwSiteLocationsTableAdapter.Fill(VwSiteLocationsDataTable)>0)
                                {
                                    using (frmSetup frm = new frmSetup())
                                    {
                                        if (frm.ShowDialog()!= System.Windows.Forms.DialogResult.OK )
                                        {
                                            System.Windows.Forms.MessageBox.Show("Location Not Configured for Site<" + Settings.ServerName + "> Location Must Be Set Up To Continue ");
                                            Environment.Exit(0);
                                            return null;
                                        }
                                    }
                                } 
                            }

                        }
                        if (site_SetupDataTable == null) site_SetupDataTable = new NWDataset.Site_SetupDataTable();

                            if (site_SetupTableAdapter.Fill(site_SetupDataTable, Location_Id) == 0)
                            {
                                System.Windows.Forms.MessageBox.Show("Site Not Configured for Server<" + Settings.ServerName + "> Site Must Be Set Up To Continue ");
                                Environment.Exit(0);
                                return null;
                            }
                            else
                            {
                                return site_SetupDataTable[0];
                            }
                    }
                    else
                    {
                        return site_SetupDataTable[0];
                    }
                }
                else
                {
                    return site_SetupDataTable[0];
                }

                
            }
        }

        public static string Viewtype
        {
            get
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                return config.AppSettings.Settings["ViewType"].Value;
            }
            set
            {
                Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                config.AppSettings.Settings["ViewType"].Value = value;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        public static string ServerName
        {
            get
            {
                if (string.IsNullOrEmpty(serverName))
                {
                    using (NWDatasetTableAdapters.QueriesTableAdapter Q = new NWDatasetTableAdapters.QueriesTableAdapter())
                    {
                        serverName = Q.GetServername();
                    }
                }

                return serverName;
            }
        }

        private static void GetWorkStation()
        {
            if (workStation_SetupDataTable == null)
            {
                using (workStation_SetupDataTable = new NWDataset.WorkStation_SetupDataTable())
                {
                    if (workStation_SetupTableAdapter.Fill(workStation_SetupDataTable, Environment.MachineName) == 0)
                    {
                        AddWorkStation(0);
                    }
                }
                

            }
        }

        public static int Location_Id
        {
            get
            {
                GetWorkStation();
               

                if (WorkStation_LocationRow == null)
                {
                    GetWorkStationLocationRow();
                }

                return workStation_SetupDataTable[0].Current_Location_Id;
            }

            set
            {
                if (workStation_SetupDataTable == null)
                {
                    AddWorkStation(value);
                }
                else
                {
                    if (workStation_SetupTableAdapter.Fill(workStation_SetupDataTable, Environment.MachineName) == 0)
                    {
                        AddWorkStation(value);

                    }
                    else
                    {
                        try
                        {
                            workStation_SetupDataTable[0].Current_Location_Id = value;
                            workStation_SetupTableAdapter.Update(workStation_SetupDataTable);

                        }
                        catch (Exception ex)
                        {
                            Logging.Add_System_Log(" public static int Location_Id Updating Location ID ", ex.Message);
                        }

                    }
                    
                    ResetSite();
                    ResetWorkStation();
                   

                }
            }
        }

        public static NWDataset.LocationsRow CurrentWorkStationLocationRow
        {
            get
            {
                if (WorkStation_LocationRow == null) GetWorkStationLocationRow();

                return WorkStation_LocationRow;
            }
        }

        private static void  GetWorkStationLocationRow()
        {
            using (NWDatasetTableAdapters.LocationsTableAdapter locationsTaleAdapter = new NWDatasetTableAdapters.LocationsTableAdapter())
            {
                using (NWDataset.LocationsDataTable locationsDataTable = new NWDataset.LocationsDataTable())
                {
                    GetWorkStation();
                    locationsTaleAdapter.FillByLocationId(locationsDataTable, workStation_SetupDataTable[0].Current_Location_Id);
                    WorkStation_LocationRow = locationsDataTable[0];
                }
            }
        }
        

        public static void AddWorkStation(int Location_Id)
        {

            if (workStation_SetupDataTable == null) workStation_SetupDataTable = new NWDataset.WorkStation_SetupDataTable();

            if (workStation_SetupTableAdapter.Fill(workStation_SetupDataTable, Environment.MachineName) == 0)
            {
                NWDataset.WorkStation_SetupRow row = workStation_SetupDataTable.NewWorkStation_SetupRow();
                try
                {
                    row.UID = Guid.NewGuid();
                    row.Allow_Multi_Locations = true;
                    row.WorkStation_Name = Environment.MachineName;
                    row.Current_Location_Id = Location_Id;
                    row.Server_Name = ServerName;
                    row.Report_Printer = "";
                    row.Grade_Printer = "";
                    row.Inbound_Printer = "";
                    row.Seed_Ticket_Printer = "";
                    row.Outbound_Printer = "";
                    row.Weigh_Scale = "";
                    row.Allow_Multi_Locations = true;
                    row.Print_Inbound_Ticket = true;
                    row.Print_Outbound_Ticket = true;
                    workStation_SetupDataTable.AddWorkStation_SetupRow(row);
                    workStation_SetupTableAdapter.Update(workStation_SetupDataTable);

                }
                catch (Exception ex)
                {
                    Logging.Add_System_Log("  AddWorkStation(int Location_Id) Adding Workstation ", ex.Message);
                }
            }

        }



        public static string  ManualWeightPrinter
        {
            get
            {
                try
                {

                    if (Options.WorkStation_Options.GetWorkStationOption("ManualWeightPrinter").Value == null)
                    {
                        return "";
                    }
                    else
                    {

                        string Printer = Options.WorkStation_Options.GetWorkStationOption("ManualWeightPrinter").Value;
                        return Printer;
                    }
                }
                catch
                {
                    return "";
                }
            }
            set
            {
                Options.WorkStation_Options.SaveOption("ManualWeightPrinter", value);
            }
        }


        public static bool CheckUpdateAtStartup
        {
            get
            {
                try
                {
                    
                    if (Options.WorkStation_Options.GetWorkStationOption("CheckUpdateAtStartup").Value == null)
                    {
                        return false;
                    }
                    else
                    {
                        bool CheckUpdateAtStartup = true;
                        bool.TryParse(Options.WorkStation_Options.GetWorkStationOption("CheckUpdateAtStartup").Value, out CheckUpdateAtStartup);
                        return CheckUpdateAtStartup;
                    }
                }
                catch
                {
                    return true; 
                }
            }
            set
            {
                Options.WorkStation_Options.SaveOption("CheckUpdateAtStartup", value.ToString());
            }
        }






        public static bool AllowTareLookup
        {
            get
            {
                try
                {

                    if (Options.WorkStation_Options.GetWorkStationOption("AllowTareLookup").Value == null)
                    {
                        return false;
                    }
                    else
                    {
                        bool allowTareLookup = false;
                        bool.TryParse(Options.WorkStation_Options.GetWorkStationOption("AllowTareLookup").Value, out allowTareLookup);
                        return allowTareLookup;
                    }
                }
                catch 
                {
                    return false;
                }
            }
            set
            {
                Options.WorkStation_Options.SaveOption("AllowTareLookup", value.ToString());
            }
        }




    }
}
