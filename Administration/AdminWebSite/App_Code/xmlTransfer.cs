using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
public class xmlTransfer
{
    public class TransferResult
    {
        public TransferResult()
        {
            ErrorMessages = new List<string>();
            TransferTime = DateTime.Now;
            RecordsTransfered = 0;
        }
        public DateTime TransferTime { get; set; }
        public int RecordsTransfered { get; set; }
        public List<string> ErrorMessages { get; set; }
    }


    public static TransferResult GetLastTransfer(string fileName)
    {
        var tr = new TransferResult();
        try
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            tr.TransferTime = Convert.ToDateTime(doc.SelectSingleNode("/body/TransferDate").InnerText);
            tr.RecordsTransfered = Convert.ToInt16(doc.SelectSingleNode("/body/RecordsTransfered").InnerText);
            XmlNode ErrorNode = doc.SelectSingleNode("/body/Errors");

            foreach (XmlNode row in ErrorNode.ChildNodes)
            {
                tr.ErrorMessages.Add(row.InnerText);
            }
           
        }
        catch
        {
            tr.ErrorMessages.Add("No Results From Transfer -- Transfer Failed");
        }
        return tr;
    }








}



