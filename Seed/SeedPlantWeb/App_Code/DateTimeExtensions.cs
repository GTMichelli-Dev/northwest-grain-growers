using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for DateTimeExtensions
/// </summary>
public static class DateTimeExtensions
{

    public static bool IsValidTimeFormat(this string input)
    {
        try
        {
            TimeSpan dummyOutput = DateTime.Parse(input).TimeOfDay;
            return true;

        }
        catch
        {
            return false;
        }
    }



    public static bool IsValidDateFormat(this string input)
    {
        DateTime  dummyOutput;
        return DateTime.TryParse(input, out dummyOutput);
    }

    public class dateTimeReturnValues
    {
        public dateTimeReturnValues()
        {
            isValid = false;
            timeDate = DateTime.Now.AddDays(-1000);
        }
        public DateTime timeDate;
        public bool isValid;
    }

    public static dateTimeReturnValues  ConvertToDateTime(this string dateInput ,string timeInput)
    {
        dateTimeReturnValues dtv = new global::DateTimeExtensions.dateTimeReturnValues();
        DateTime dateOutput= DateTime.Now.AddDays(2000);
        if (DateTime.TryParse(dateInput+" "+timeInput , out dateOutput))
        {
            dtv.isValid = true;
            dtv.timeDate = dateOutput; 
        }

        return dtv;
        

    }


    public static string getFormattedDate(DateTime  date)
    {
        var year = date.Year;
        var month = date.Month.ToString().PadLeft(2, '0');
        var day = date.Day.ToString().PadLeft(2, '0');
        return month + '/' + day + '/' + year;
    }
}