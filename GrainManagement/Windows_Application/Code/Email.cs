using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace NWGrain
{
    class Email
    {

        public static void SendWS(Guid UID)
        {
            try
            {
                Thread thread = new Thread(() => EmailWS(UID));
                thread.Start();
            }
            catch (Exception ex)
            {
                Logging.Add_System_Log("Email.SendWS", ex.Message);
            }
        }


        public static void EmailWeightSheets(WeightSheetDataSet.AllWeightSheetsDataTable allWeightSheetsDataTable)
        {
            foreach (WeightSheetDataSet.AllWeightSheetsRow row in allWeightSheetsDataTable)
            {

              
                Email.SendWS(row.Weight_Sheet_UID);
            }
        }


        //private MemoryStream PDFStream(Guid WSUID)
        //{
        //    MemoryStream stream;
        //    if (Transfer)
        //    {
        //        stream = Printing.PrintTransferWeightSheet(WSUID);
        //    }
        //    else
        //    {
        //        stream = Printing.PrintWeightSheet(WSUID);
        //    }


        //}



        static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        private static void EmailWS(Guid UID)
        {

            try
            {
                using (EmailDataSet.WeightSheetToEmailDataTable weightSheetToEmailDataTable = new NWGrain.EmailDataSet.WeightSheetToEmailDataTable())
                {

                    using (EmailDataSetTableAdapters.WeightSheetToEmailTableAdapter weightSheetToEmailTableAdapter = new EmailDataSetTableAdapters.WeightSheetToEmailTableAdapter())
                    {
                        if (weightSheetToEmailTableAdapter.Fill(weightSheetToEmailDataTable, UID) > 0)
                        {
                            EmailDataSet.WeightSheetToEmailRow row = weightSheetToEmailDataTable[0];
                            if (row.IsEmailedNull()) row.Emailed = false;
                            if (row.Email_WS && !row.Emailed)
                            {
                                if (!row.IsEmail_AddressNull() )
                                {
                                    string Message = string.Format("Weight Sheet:{0}", row.WS_Id) + System.Environment.NewLine;
                                    Message += string.Format("Location:{0}", row.Location) + System.Environment.NewLine;
                                    Message += string.Format("Producer:{0}", row.Producer) + System.Environment.NewLine;
                                    Message += string.Format("Lot:{0}", row.Lot_Number) + System.Environment.NewLine;
                                    Message += string.Format("Crop:{0}", row.Crop) + System.Environment.NewLine;

                                    MemoryStream stream = Printing.PrintWeightSheet(row.UID);
                                    string[] words = row.Email_Address.Replace(";"," ").Split(' ');// row.Email_Address.Split(' ');
                                    string Recipients = "";
                                    foreach (string email in words)
                                    {
                                        string emailAddress = email.Trim();
                                        emailAddress = emailAddress.Trim();
                                        if (IsValidEmail(emailAddress))
                                        {
                                            Recipients += emailAddress + ";";
                                        }
                                    }
                                    if (!string.IsNullOrWhiteSpace(Recipients)) {
                                        if (SendPDFMail(Recipients, stream, string.Format("{0}.pdf", row.WS_Id), string.Format("WeightSheet {0}", row.WS_Id), Message)){
                                            using (EmailDataSetTableAdapters.QueriesTableAdapter Q = new EmailDataSetTableAdapters.QueriesTableAdapter())
                                            {
                                                Q.SetWeightSheetAsEmailed(UID);
                                            }
                                            
                                        }
                                    }


                                }
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                Logging.Add_System_Log("Email.EmailWS", ex.Message);
            }
        }


        /// <summary>
        /// Sends an xlsx excell file to an email address
        /// </summary>
        /// <param name="Recipient">email Address</param>
        /// <param name="stream">memorystream of file</param>
        /// <param name="FileName">Name of file like something.xlsx</param>
        /// <param name="Subject">email Subject</param>
        /// <param name="Message">email Body</param>
        private static bool SendPDFMail(string Recipient, MemoryStream stream, string FileName, string Subject, string Message)
        {
            try
            {
                //Very important or file will be 0 bytes
                stream.Seek(0, SeekOrigin.Begin);

                System.Net.Mail.Attachment mailAttachment = new System.Net.Mail.Attachment(stream, new System.Net.Mime.ContentType("application/pdf"));
                mailAttachment.Name = FileName;

                List<System.Net.Mail.Attachment> Attchments = new List<System.Net.Mail.Attachment>();
                Attchments.Add(mailAttachment);
                SendMail(Recipient, Subject, Message, Attchments);
                return true;
            }
            catch (Exception ex)
            {
                Logging.Add_System_Log("Email.SendPDFMail", ex.Message);
                return false;
            }
        }


        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="Recipient">email Address</param>
        /// <param name="Subject">email subject</param>
        /// <param name="Message">email body</param>
        /// <param name="Attachments">list of attchments to send. Set to null if none</param>
        private static bool SendMail(string Recipient, string Subject, string Message, List<Attachment> Attachments)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "mail.smtp2go.com";
                smtpClient.Port = 25;
                smtpClient.EnableSsl = true;

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential("NWGG", "NWFyMHN3aW91b2Iw");
                MailMessage message = new MailMessage();
                message.From = new MailAddress("noreply@nwgrgr.com");


                foreach (var address in Recipient.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    message.To.Add(address);
                }

                message.Subject = Subject;
                message.Body = Message;
                if (Attachments != null && Attachments.Count > 0)
                {
                    foreach (Attachment attchment in Attachments)
                    {
                        message.Attachments.Add(attchment);
                    }
                }
                smtpClient.Timeout = 240000;
                smtpClient.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                try
                {
                    Logging.Add_System_Log("<Email.SendMail><Recipient>" + Recipient + "</Recipient>+<Subject>" + Subject + "</Subject></Email.SendMail>", ex.Message);
                }
                catch
                {
                    Logging.Add_System_Log("Email.SendMail", ex.Message);
                }
                return false;
            }

        }







    }
}
