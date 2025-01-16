using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWGrain
{


    class Options
    {

        public class WorkStation_Options
        {
           


            private static OptionsDataset.WorkStation_OptionsDataTable workStation_OptionsDataTable;

            public static OptionsDataset.WorkStation_OptionsDataTable GetWorkStationOptions()
            {

                if (workStation_OptionsDataTable == null)
                {
                    workStation_OptionsDataTable = new NWGrain.OptionsDataset.WorkStation_OptionsDataTable();
                    using (OptionsDatasetTableAdapters.WorkStation_OptionsTableAdapter workStation_OptionsTableAdapter = new OptionsDatasetTableAdapters.WorkStation_OptionsTableAdapter())
                    {
                        workStation_OptionsTableAdapter.FillByServer(workStation_OptionsDataTable, Environment.MachineName);
                    }
                }
                return workStation_OptionsDataTable;
            }

            public static OptionsDataset.WorkStation_OptionsRow GetWorkStationOption(string Description)
            {
               
                    workStation_OptionsDataTable = new NWGrain.OptionsDataset.WorkStation_OptionsDataTable();
                    using (OptionsDatasetTableAdapters.WorkStation_OptionsTableAdapter workStation_OptionsTableAdapter = new OptionsDatasetTableAdapters.WorkStation_OptionsTableAdapter())
                    {
                        workStation_OptionsTableAdapter.FillByServerDescription(workStation_OptionsDataTable, Description, Environment.MachineName);
                    }
               
                if (workStation_OptionsDataTable.Count > 0)
                {
                    return workStation_OptionsDataTable[0];
                }
                else
                {
                    return null;
                }
            }

            public static void SaveOption(string Description, string Value)
            {
                try
                {
                    using (OptionsDatasetTableAdapters.WorkStation_OptionsTableAdapter workStation_OptionsTableAdapter = new OptionsDatasetTableAdapters.WorkStation_OptionsTableAdapter())
                    {
                        workStation_OptionsTableAdapter.FillByServerDescription(workStation_OptionsDataTable, Description,Environment.MachineName);


                        if (workStation_OptionsDataTable.Count > 0)
                        {
                            workStation_OptionsDataTable[0].Value = Value;

                        }
                        else
                        {
                            OptionsDataset.WorkStation_OptionsRow row = workStation_OptionsDataTable.NewWorkStation_OptionsRow();
                            row.WorkStation_Name = Environment.MachineName;
                            row.UID = Guid.NewGuid();
                            row.Server_Name = Settings.ServerName;
                            row.WorkStation_Name = System.Environment.MachineName;
                            row.Description = Description;
                            row.Value = Value;
                            workStation_OptionsDataTable.AddWorkStation_OptionsRow(row);
                        }

                        workStation_OptionsTableAdapter.Update(workStation_OptionsDataTable);
                    }

                }
                catch (Exception ex)
                {
                    Logging.Add_System_Log("Save WorkStation Option", ex.Message);
                }
            }


            public static void DeleteOption(string Description)
            {
                try
                {
                    using (OptionsDatasetTableAdapters.QueriesTableAdapter Q = new OptionsDatasetTableAdapters.QueriesTableAdapter())
                    {
                        Q.DeleteWorkStationOption(Description, Environment.MachineName);
                    }


                }
                catch (Exception ex)
                {
                    Logging.Add_System_Log("Delete WorkStation Option", ex.Message);
                }
            }

        }





        public class Location_Options
        {
            private static OptionsDataset.Location_OptionsDataTable Location_OptionsDataTable;

            public static OptionsDataset.Location_OptionsDataTable GetLocationOptions()
            {

                if (Location_OptionsDataTable == null)
                {
                    Location_OptionsDataTable = new NWGrain.OptionsDataset.Location_OptionsDataTable();
                    using (OptionsDatasetTableAdapters.Location_OptionsTableAdapter Location_OptionsTableAdapter = new OptionsDatasetTableAdapters.Location_OptionsTableAdapter())
                    {
                        Location_OptionsTableAdapter.FillByServer(Location_OptionsDataTable);
                    }
                }
                return Location_OptionsDataTable;
            }

            public static OptionsDataset.Location_OptionsDataTable GetLocationOptions(int Location_Id)
            {

                if (Location_OptionsDataTable == null)
                {
                    Location_OptionsDataTable = new NWGrain.OptionsDataset.Location_OptionsDataTable();
                    using (OptionsDatasetTableAdapters.Location_OptionsTableAdapter Location_OptionsTableAdapter = new OptionsDatasetTableAdapters.Location_OptionsTableAdapter())
                    {
                        Location_OptionsTableAdapter.FillByLocation_Id(Location_OptionsDataTable, Location_Id);
                    }
                }
                return Location_OptionsDataTable;
            }

            public static OptionsDataset.Location_OptionsRow GetLocationOption(int Location_Id, string Description)
            {
                if (Location_OptionsDataTable == null)
                {
                    Location_OptionsDataTable = new NWGrain.OptionsDataset.Location_OptionsDataTable();
                    using (OptionsDatasetTableAdapters.Location_OptionsTableAdapter Location_OptionsTableAdapter = new OptionsDatasetTableAdapters.Location_OptionsTableAdapter())
                    {
                        Location_OptionsTableAdapter.FillByLocationDescription(Location_OptionsDataTable, Location_Id, Description);
                    }
                }
                if (Location_OptionsDataTable.Count > 0)
                {
                    return Location_OptionsDataTable[0];
                }
                else
                {
                    return null;
                }
            }

            public static void SaveOption(int Location_Id, string Description, string Value)
            {
                try
                {
                    Location_OptionsDataTable = GetLocationOptions();
                    bool found = false;
                    foreach (OptionsDataset.Location_OptionsRow row in Location_OptionsDataTable)
                    {
                        if (row.Description.ToUpper() == Description.ToUpper())
                        {
                            found = true;
                            row.Value = Value;
                        }

                    }
                    if (!found)
                    {
                        OptionsDataset.Location_OptionsRow row = Location_OptionsDataTable.NewLocation_OptionsRow();

                        row.UID = Guid.NewGuid();
                        row.Location_ID = Location_Id;
                        row.Description = Description;
                        row.Value = Value;
                        Location_OptionsDataTable.AddLocation_OptionsRow(row);
                    }
                    using (OptionsDatasetTableAdapters.Location_OptionsTableAdapter Location_OptionsTableAdapter = new OptionsDatasetTableAdapters.Location_OptionsTableAdapter())
                    {
                        Location_OptionsTableAdapter.Update(Location_OptionsDataTable);
                    }

                }
                catch (Exception ex)
                {
                    Logging.Add_System_Log("Save Location Option", ex.Message);
                }
            }


            public static void DeleteOption(int Location_Id, string Description)
            {
                try
                {
                    using (OptionsDatasetTableAdapters.QueriesTableAdapter Q = new OptionsDatasetTableAdapters.QueriesTableAdapter())
                    {
                        Q.DeleteLocationOption(Location_Id, Description);
                    }


                }
                catch (Exception ex)
                {
                    Logging.Add_System_Log("Delete Location Option", ex.Message);
                }
            }

        }


        public class Site_Options
        {
            private static OptionsDataset.Site_OptionsDataTable Site_OptionsDataTable;

            public static OptionsDataset.Site_OptionsDataTable GetSiteOptions()
            {

                if (Site_OptionsDataTable == null)
                {
                    Site_OptionsDataTable = new NWGrain.OptionsDataset.Site_OptionsDataTable();
                    using (OptionsDatasetTableAdapters.Site_OptionsTableAdapter Site_OptionsTableAdapter = new OptionsDatasetTableAdapters.Site_OptionsTableAdapter())
                    {
                        Site_OptionsTableAdapter.FillByServer(Site_OptionsDataTable);
                    }
                }
                return Site_OptionsDataTable;
            }



            public static OptionsDataset.Site_OptionsRow GetSiteOption(string Description)
            {
                if (Site_OptionsDataTable == null)
                {
                    Site_OptionsDataTable = new NWGrain.OptionsDataset.Site_OptionsDataTable();
                    using (OptionsDatasetTableAdapters.Site_OptionsTableAdapter Site_OptionsTableAdapter = new OptionsDatasetTableAdapters.Site_OptionsTableAdapter())
                    {
                        Site_OptionsTableAdapter.FillByDescription(Site_OptionsDataTable, Description);
                    }
                }
                if (Site_OptionsDataTable.Count > 0)
                {
                    return Site_OptionsDataTable[0];
                }
                else
                {
                    return null;
                }
            }

            public static void SaveOption(int Site_Id, string Description, string Value)
            {
                try
                {
                    Site_OptionsDataTable = GetSiteOptions();
                    bool found = false;
                    foreach (OptionsDataset.Site_OptionsRow row in Site_OptionsDataTable)
                    {
                        if (row.Description.ToUpper() == Description.ToUpper())
                        {
                            found = true;
                            row.Value = Value;
                        }

                    }
                    if (!found)
                    {
                        OptionsDataset.Site_OptionsRow row = Site_OptionsDataTable.NewSite_OptionsRow();

                        row.UID = Guid.NewGuid();
                        row.Server_Name = Settings.ServerName;
                        row.Description = Description;
                        row.Value = Value;
                        Site_OptionsDataTable.AddSite_OptionsRow(row);
                    }
                    using (OptionsDatasetTableAdapters.Site_OptionsTableAdapter Site_OptionsTableAdapter = new OptionsDatasetTableAdapters.Site_OptionsTableAdapter())
                    {
                        Site_OptionsTableAdapter.Update(Site_OptionsDataTable);
                    }

                }
                catch (Exception ex)
                {
                    Logging.Add_System_Log("Save Site Option", ex.Message);
                }
            }


            public static void DeleteOption(int Site_Id, string Description)
            {
                try
                {
                    using (OptionsDatasetTableAdapters.QueriesTableAdapter Q = new OptionsDatasetTableAdapters.QueriesTableAdapter())
                    {
                        Q.DeleteSiteOption(Description);
                    }


                }
                catch (Exception ex)
                {
                    Logging.Add_System_Log("Delete Site Option", ex.Message);
                }
            }

        }

    }
}
