using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for LinqQuerys
/// </summary>
public class LinqQuerys
{
    public LinqQuerys()
    {
        //
        // TODO: Add constructor logic here
        //

    }


    public static List<CropProducers> GetCropProducers()
    {
        using (var dbContext = new NWDataModel())
        {



            var result = from cpf in dbContext.CropProducerFilters
                         join c in dbContext.Crops on cpf.CropUID equals c.UID
                         join p in dbContext.Producers on cpf.ProducerUID equals p.UID
                         select new CropProducers
                         {
                             UID = cpf.UID,
                             ProducerUID = cpf.ProducerUID,
                             CropUID = cpf.CropUID,
                             Crop = c.Description,
                             Producer = p.Description
                         };

            return result.ToList();

        }
    }


    public class ProducerNameID_UID{
        public Guid UID { get; set; }
        public string  NameID { get; set; }
    }


    public static List<ProducerNameID_UID> GetProducerNameID_UID_List()
    {
        using (var dbContext = new NWDataModel())
        {
            var result = from producer in dbContext.Producers
                         orderby producer.Description + " - " + producer.Id.ToString() descending

                         select new ProducerNameID_UID
                         {
                             UID = producer.UID,
                             NameID = producer.Description + " - " + producer.Id.ToString()
                         };
            
            return result.ToList();
        }


    }

}