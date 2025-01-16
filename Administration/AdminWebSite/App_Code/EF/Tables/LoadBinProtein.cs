using System;
using System.Collections.Generic;


public partial class LoadBinProtein
{
    public Guid Uid { get; set; }

    public long LoadId { get; set; }

    public DateTime? TimeOut { get; set; }

    public string Bin { get; set; }

    public float Protein { get; set; }

    public int? Net { get; set; }

    public int LocationId { get; set; }

    public string Crop { get; set; }

    public string Location { get; set; }

    public int? ProteinWeight { get; set; }

    public bool? ProteinValid { get; set; }
}