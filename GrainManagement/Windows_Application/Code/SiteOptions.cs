using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWGrain
{
    public static class SiteOptions
    {
        const string RemotePrint = "RemotePrintOriginal";
        const string PromptForTruckType = "PromptForTruckTypeOriginal"; 

        public static string GetServerName()
        {
            using (OptionsDatasetTableAdapters.QueriesTableAdapter Q = new OptionsDatasetTableAdapters.QueriesTableAdapter())
            {
              
                return Q.GetServerName();
            }
        }

        public static bool GetRemotePrintOriginal()
        {
            using (OptionsDatasetTableAdapters.Site_OptionsTableAdapter optionsTableAdapter = new OptionsDatasetTableAdapters.Site_OptionsTableAdapter())
            {
                using (OptionsDataset.Site_OptionsDataTable optionsTable = new OptionsDataset.Site_OptionsDataTable())
                {
                    optionsTableAdapter.FillByDescription(optionsTable, RemotePrint);
                    if (optionsTable.Count == 0)
                    {
                        
                        var newRow = optionsTable.NewSite_OptionsRow();
                        newRow.UID = Guid.NewGuid(); 
                        newRow.Description = RemotePrint;
                        newRow.Value = false.ToString();
                        newRow.Server_Name = GetServerName(); // Set the server name for the new row
                        optionsTable.AddSite_OptionsRow(newRow);
                        optionsTableAdapter.Update(optionsTable);
                    }
                    else if (optionsTable.Count > 1)
                    {
                        // Keep the first row, delete the rest
                        for (int i = 1; i < optionsTable.Count; i++)
                        {
                            optionsTableAdapter.Delete(optionsTable[i].UID);
                            optionsTableAdapter.Update(optionsTable);
                        }
                    }
                    return bool.Parse(optionsTable[0].Value);

                }
            }
        }

        public static void SetRemotePrintOriginal(bool value)
        {
            using (OptionsDatasetTableAdapters.Site_OptionsTableAdapter optionsTableAdapter = new OptionsDatasetTableAdapters.Site_OptionsTableAdapter())
            {
                using (OptionsDataset.Site_OptionsDataTable optionsTable = new OptionsDataset.Site_OptionsDataTable())
                {
                    optionsTableAdapter.FillByDescription(optionsTable, RemotePrint);
                    if (optionsTable.Count == 0)
                    {
                        var newRow = optionsTable.NewSite_OptionsRow();
                        newRow.UID = Guid.NewGuid();
                        newRow.Description = RemotePrint;
                        newRow.Value = value.ToString();
                        newRow.Server_Name = GetServerName();

                        optionsTable.AddSite_OptionsRow(newRow);
                        
                    }
                    else if (optionsTable.Count > 1)
                    {
                        // Keep the first row, delete the rest
                        for (int i = 1; i < optionsTable.Count; i++)
                        {
                            optionsTableAdapter.Delete(optionsTable[i].UID);
                            
                        }
                    }
                    optionsTable[0].Value=value.ToString();
                    optionsTableAdapter.Update(optionsTable);

                }
            }
        }



        public static bool GetPromptForTruckType()
        {
            using (OptionsDatasetTableAdapters.Site_OptionsTableAdapter optionsTableAdapter = new OptionsDatasetTableAdapters.Site_OptionsTableAdapter())
            {
                using (OptionsDataset.Site_OptionsDataTable optionsTable = new OptionsDataset.Site_OptionsDataTable())
                {
                    optionsTableAdapter.FillByDescription(optionsTable, PromptForTruckType);
                    if (optionsTable.Count == 0)
                    {
                        var newRow = optionsTable.NewSite_OptionsRow();
                        newRow.UID = Guid.NewGuid();
                        newRow.Description = PromptForTruckType;
                        newRow.Value = false.ToString();
                        newRow.Server_Name = GetServerName();

                        optionsTable.AddSite_OptionsRow(newRow);
                        optionsTableAdapter.Update(optionsTable);
                    }
                    else if (optionsTable.Count > 1)
                    {
                        // Keep the first row, delete the rest
                        for (int i = 1; i < optionsTable.Count; i++)
                        {
                            optionsTableAdapter.Delete(optionsTable[i].UID);
                            optionsTableAdapter.Update(optionsTable);
                        }
                    }
                    return bool.Parse(optionsTable[0].Value);

                }
            }
        }

        public static void SetPromptForTruckType(bool value)
        {
            using (OptionsDatasetTableAdapters.Site_OptionsTableAdapter optionsTableAdapter = new OptionsDatasetTableAdapters.Site_OptionsTableAdapter())
            {
                using (OptionsDataset.Site_OptionsDataTable optionsTable = new OptionsDataset.Site_OptionsDataTable())
                {
                    optionsTableAdapter.FillByDescription(optionsTable, PromptForTruckType);
                    if (optionsTable.Count == 0)
                    {
                        var newRow = optionsTable.NewSite_OptionsRow();
                        newRow.UID = Guid.NewGuid();
                        newRow.Description = PromptForTruckType;
                        newRow.Value = value.ToString();
                        newRow.Server_Name = GetServerName();

                        optionsTable.AddSite_OptionsRow(newRow);

                    }
                    else if (optionsTable.Count > 1)
                    {
                        // Keep the first row, delete the rest
                        for (int i = 1; i < optionsTable.Count; i++)
                        {
                            optionsTableAdapter.Delete(optionsTable[i].UID);

                        }
                    }
                    optionsTable[0].Value = value.ToString();
                    optionsTableAdapter.Update(optionsTable);

                }
            }
        }
    }



}
