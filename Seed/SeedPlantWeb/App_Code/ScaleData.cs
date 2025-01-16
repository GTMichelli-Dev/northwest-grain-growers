using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for ScaleData
/// </summary>
public class ScaleData
{
    public ScaleData()
    {
        
    }

    public static DateTime  LastUpdated { get; set; }

    private static ScaleDataSet.ScalesDataTable weighScales;

    public class CurrentScale
    {
        public string Description { get; set; }
        public int Weight { get; set; }
        public bool Motion { get; set; }
        public bool Valid { get; set; }
        public string  Status { get; set; }


    }

    public static CurrentScale GetCurrentScale(int ScaleId)
    {
        
         ScaleDataSet.ScalesRow srow= WeighScales.FirstOrDefault(x=> x.Index == ScaleId );
         if (srow != null)
        {
            CurrentScale cScale = new CurrentScale();
            cScale.Description = srow.Description;
            cScale.Motion = srow.Motion;
            cScale.Valid = srow.Valid;
            cScale.Weight = srow.Weight;
            cScale.Status = srow.Status;
            return cScale;
        }
         else
        {
            return null;
        }
    }



    public static ScaleDataSet.ScalesDataTable WeighScales
    {

        get
        {
            if (weighScales== null )
            {
                LastUpdated = DateTime.Now.AddDays(-1);
                weighScales = new ScaleDataSet.ScalesDataTable();
                ScaleDataSet.ScalesRow newScale = weighScales.NewScalesRow();

                newScale.Description = "Manual";
                newScale.Weight = 0;
                newScale.Motion = false;
                newScale.ReadOnly = false;
                newScale.Valid = true; 
                newScale.Status = "";
                newScale.Index = 1;
                weighScales.AddScalesRow(newScale);

            }
            try
            {
                if ((DateTime.Now - LastUpdated).TotalSeconds > 1)
                {
                    using (ScaleWebService.LocalWebService proxy = new ScaleWebService.LocalWebService())
                    {
                        ScaleWebService.LocalDataSet.Weigh_ScalesDataTable currentScales;
                        currentScales = proxy.GetScales();
                        LastUpdated = DateTime.Now;
                        foreach (ScaleWebService.LocalDataSet.Weigh_ScalesRow  scale in currentScales )
                        {
                            Random tf = new Random();
                            tf.Next(0, 1);

                            scale.OK= (tf.Next(0, 4)<3);
                            scale.Motion = (tf.Next(0, 4) < 3);

                            string Status = "OK";

                            if (!scale.OK )
                            {
                                Status = "Fail";// scale.Status; 
                            }
                            else if (scale.Motion )
                            {
                                Status = "Motion";

                            }

                            var existingScale = weighScales.FirstOrDefault(x => x.Description == scale.Description);
                            if (existingScale==null)
                            {
                                existingScale = weighScales.NewScalesRow();

                                existingScale.Description = scale.Description;
                                existingScale.Weight = scale.Weight ;
                                existingScale.Motion = scale.Motion ;
                                existingScale.ReadOnly = true ;
                                existingScale.Status = Status;
                                existingScale.Index = weighScales.Count + 1;
                                weighScales.AddScalesRow(existingScale);
                            }
                            else
                            {
                                Random r = new Random();
                                existingScale.Description = scale.Description;
                                existingScale.Weight = r.Next(0, 120000);// scale.Weight;
                                existingScale.Motion = scale.Motion;
                                existingScale.Valid = scale.OK;
                                existingScale.ReadOnly = true;
                                existingScale.Status = Status;
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return weighScales;
        }
        set
        {
              
        }
    }


}