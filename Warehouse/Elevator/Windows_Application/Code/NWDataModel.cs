using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;


public class NWDataModel : DbContext
{
    public NWDataModel() : base("name=NWGrain.Properties.Settings.NW_DataConnectionString")
    {
    }

 
}