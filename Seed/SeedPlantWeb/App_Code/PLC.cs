using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for PLCs
/// </summary>
public class PLC
{

    public enum enumBatchType {NotSet, Clean,Bulk}

    public static PLCDataSet plcDataset = new PLCDataSet();

    public static PCResponse pcResponse = new PCResponse();


  

    private static  readonly object PLCLock = new object();

    public static bool PLCConnected
    {
        get
        {
            try
            {
                return (plcDataset.Treatments.Count > 0 || plcDataset.Bins.Count > 0);
            }
            catch
            {
                return false;
            }
        }
    }


    public PLC()
    {
     
    }

   


    public class PCResponse
    {
        public PCResponse()
        {
            UID = Guid.NewGuid();
            BinStatus = new List<ItemStatus>();
            TreatmentStatus = new List<ItemStatus>();
            ColorStatus = new List<ItemStatus>();
            Reset();
        }

        public void Reset()
        {
   
            BinStatus.Clear();
            TreatmentStatus.Clear();
            ColorStatus.Clear();
            BatchType = enumBatchType.NotSet; 

        }
        public class ItemStatus
        {
            public int ItemId { get; set; }
            public bool Active { get; set; }
        }


   

        public Guid UID { get; set; }
        public enumBatchType BatchType { get; set; }
        public List<ItemStatus> BinStatus { get; set; }
        public List<ItemStatus> TreatmentStatus { get; set; }
        public List<ItemStatus> ColorStatus { get; set; }
    }
}