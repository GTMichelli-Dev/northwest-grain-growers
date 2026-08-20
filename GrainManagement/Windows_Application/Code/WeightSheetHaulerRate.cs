using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWGrain
{
    class WeightSheetHaulerRate
    {
        public enum enumRateType
        {
            NotSet,
            Field,
            FarmStorage,
            Custom
        }




        public static string GetBolType(enumRateType rateType)
        {
            if (rateType == enumRateType.NotSet)
            {
                return null;
            }
            else if (rateType==enumRateType.Field)
            {
                return "A";
            }
            else if (rateType == enumRateType.FarmStorage)
            {
                return "F";
            }
            else if (rateType == enumRateType.Custom)
            {
                return "C";
            }
            else
            {
                return null;
            }

        }


        public static enumRateType GetRateType(string bolType)
        {
            if (string.IsNullOrEmpty(bolType))
            {
                return enumRateType.NotSet;
            }
            else if (bolType.ToUpper().Trim()== "A"){
                return enumRateType.Field; 
            }
            else if (bolType.ToUpper().Trim() == "F")
            {
                return enumRateType.FarmStorage;
            }
            else if (bolType.ToUpper().Trim() == "C")
            {
                return enumRateType.Custom;
            }
            else
            {
                return enumRateType.NotSet;
            }

        }

        public class Rate
        {
            public Guid UID { get; set; }
            public decimal RateAmount { get; set; }
            public  enumRateType RateType { get; set; }

           

        }



        public static void UpdateWeightSheetRate(Rate newRate)
        {
            string BolType = GetBolType(newRate.RateType);
            using (HaulerRatesDataSet haulerRates = new HaulerRatesDataSet())
            {
                using (HaulerRatesDataSetTableAdapters.HaulerRateTableAdapter haulerRateTableAdapter = new HaulerRatesDataSetTableAdapters.HaulerRateTableAdapter())
                {
                    if (haulerRateTableAdapter.Fill(haulerRates.HaulerRate, newRate.UID) > 0)
                    {
                        HaulerRatesDataSet.HaulerRateRow row = haulerRates.HaulerRate[0];
                        
                        if (newRate.RateType == enumRateType.NotSet ) {
                            row.SetBOL_TypeNull();
                            row.SetRateNull();
                        }
                        else {
                            row.BOL_Type = BolType;
                            row.Rate = newRate.RateAmount;
                        }
                        haulerRateTableAdapter.Update(haulerRates);

                    }
                        
                }
            }
        }






        public static Rate GetWeightSheetRate(Guid UID)
        {
            using (HaulerRatesDataSet haulerRates = new HaulerRatesDataSet())
            {
                using (HaulerRatesDataSetTableAdapters.HaulerRateTableAdapter haulerRateTableAdapter = new HaulerRatesDataSetTableAdapters.HaulerRateTableAdapter())
                {
                    if (haulerRateTableAdapter.Fill(haulerRates.HaulerRate, UID) > 0)
                    {
                        var row = haulerRates.HaulerRate[0];
                        Rate rate = new Rate() { UID = UID };
                        rate.RateAmount = (row.IsRateNull()) ? 0 : row.Rate;
                        rate.RateType = (row.IsBOL_TypeNull()) ? enumRateType.NotSet : GetRateType(row.BOL_Type);
                        return rate;
                    }
                    else
                    {
                        return null;
                    }

                }
            }
        }
    }
}
