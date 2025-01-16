using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CrystalDecisions;
using CrystalDecisions.CrystalReports;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;

// Check Crystal Decisions and manke sure all refrences are installed

namespace LotLabeler
{
    class Printing
    {

        public static bool PrintSampleLabel(NWDataset NWDataset, string Printer,int NumCopies)
        {

            //bool Check_Protien = Alert.Show("Check Protein On Sample", "Protein", true) == System.Windows.Forms.DialogResult.Yes;


            

            try
            {


                using (Reports.Sample_Label rptSample_Label = new Reports.Sample_Label())
                {


                    {


                        //see if the printer is valid 
                        bool ValidPrinter = false;

                        System.Drawing.Printing.PrinterSettings settings = new System.Drawing.Printing.PrinterSettings();
                        string DefaultPrinter = settings.PrinterName;

                        foreach (string InstalledPrinter in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
                        {
                            System.Diagnostics.Debug.Print(InstalledPrinter);
                            if (Printer.ToUpper() == InstalledPrinter.ToUpper())
                            {

                                ValidPrinter = true;
                                break;
                            }

                        }
                        if (!ValidPrinter) // if not valid use the system default printer
                        {
                            Printer = DefaultPrinter;
                        }
                        {


                            {

                                {
                                  //  using (PrintingTicket frm = new PrintingTicket(string.Format("Printing Sample Label For Lot {0}", NWDataset.SampleLabel[0].Lot_Number)))
                                    {
                                    //    frm.Show();
                                        System.Windows.Forms.Application.DoEvents();
                                        rptSample_Label.SetDataSource(NWDataset);


                                        // The New Way to Get the Prenter Name Set
                                        CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions printReportOptions = new CrystalDecisions.ReportAppServer.Controllers.PrintReportOptions();
                                        CrystalDecisions.ReportAppServer.Controllers.PrintOutputController printOutputController = new CrystalDecisions.ReportAppServer.Controllers.PrintOutputController();
                                        CrystalDecisions.ReportAppServer.ClientDoc.ISCDReportClientDocument rptClientDoc;
                                        rptClientDoc = rptSample_Label.ReportClientDocument;
                                        printReportOptions.PrinterName = Printer;
                                        printReportOptions.AddPrinterPageRange(1, NumCopies);
                                        rptClientDoc.PrintOutputController.PrintReport(printReportOptions);



                                        //string DirectoryPath = @"c:\ScaleTickets";
                                        //string Filename = DirectoryPath + @"\WS" + NWDataset.vwWeigh_Sheet[0].Location_Id.ToString() + "_" + NWDataset.vwWeigh_Sheet[0].WS_Id.ToString() + ".pdf";
                                        //if (!System.IO.Directory.Exists(DirectoryPath)) { System.IO.Directory.CreateDirectory(DirectoryPath); }
                                        //if (!System.IO.File.Exists(Filename)) { System.IO.File.Delete(Filename); }
                                        //rptWeigh_Sheet.ExportToDisk(ExportFormatType.PortableDocFormat, Filename);

                                    }

                                }
                                System.Threading.Thread.Sleep(1000);
                            }
                        }
                    }

                }
                return true;
            }
            catch (Exception ocrap)
            {
                Alert.Show("Looks Like The Sample Label Failed To Print", ocrap.Message, false);

                return false;
            }

        }


    }



}
