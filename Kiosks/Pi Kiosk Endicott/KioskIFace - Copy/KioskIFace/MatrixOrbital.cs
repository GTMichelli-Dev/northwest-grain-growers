using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for MatrixOrbital
/// </summary>
public class MatrixOrbital
{
	public MatrixOrbital()
	{
		
	}

        public static byte[] clear_screen = { 254, 88 };
        public static byte[] CurrentWeight = System.Text.Encoding.ASCII.GetBytes("Current Weight (LBS)");
        public static byte[] Lbs = System.Text.Encoding.ASCII.GetBytes("Lbs.");

        public static string Clear_screen()
        {
            int[] values = { 254, 88 };
            char[] chars = values.Select(x => (char)x).ToArray();
            string str = new string(chars);
            //Console.WriteLine(str); // "8120005828"

            //char c = Convert.ToChar(254);
            //char d = Convert.ToChar(88);
            //string Test = c.ToString() + d.ToString();
            return str ;
        }


        public static byte[] ResetScreen(int Weight, string ScaleStatus,string Line2,string Line3)
        {
            List<byte> DataList = new List<byte>();
            DataList.AddRange(clear_screen);
            DataList.AddRange(new byte[] { 254, 45, 1, 0, 0, 240, 15, 1, 1, 2, 0, 0, 1 });
            DataList.AddRange(new byte[] {254, 46, 1});
            DataList.AddRange(CurrentWeight);
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 45, 2, 0, 15, 230, 65, 1, 1, 10, 0, 0, 1 });
            DataList.AddRange(new byte[] {254, 46, 2});
            DataList.AddRange(WeightString(Weight));
            DataList.Add(0);
            DataList.AddRange(new byte[] {254, 45, 4, 0, 70, 240, 88, 1, 1, 4, 0, 0, 1});
            DataList.AddRange(new byte[] {254, 46, 4});
            DataList.AddRange(StringToByte(ScaleStatus));
            DataList.Add(0);
            DataList.AddRange(new byte[] {254, 45, 5, 0, 88, 240, 107, 1, 1, 4, 0, 0, 1});
            DataList.AddRange(new byte[] {254, 46, 5});
            DataList.AddRange(StringToByte(Line2));
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 45, 6, 0, 108, 240, 128, 1, 1, 4, 0, 0, 1 });
            DataList.AddRange(new byte[] { 254, 46, 6 });
            DataList.AddRange(StringToByte(Line3));
            DataList.Add(0);
            return DataList.ToArray();
        }



    public static byte[] UpdateWeight(int Weight)
    {
        List<byte> DataList = new List<byte>();
        DataList.Add(0);
        DataList.Add(0);
        DataList.AddRange(new byte[] { 254, 45, 2, 0, 15, 230, 65, 1, 1, 10, 0, 0, 1 });
        DataList.AddRange(new byte[] { 254, 46, 2 });
        DataList.AddRange(WeightString(Weight));
        DataList.Add(0);
        DataList.Add(0);
        return DataList.ToArray();
    }

    /// <summary>
    /// Restarts the screen Like Turns it off then back on
    /// </summary>
    /// <returns></returns>
    public static byte[] RestartScreen()
    {
        List<byte> DataList = new List<byte>();
        DataList.Add(0);
        DataList.Add(0);
        DataList.AddRange(new byte[] { 254, 253, 77, 79, 117});
        DataList.Add(0);
        DataList.Add(0);
        return DataList.ToArray();
    }




    public static byte[] SetWeight(int Weight)
        {
            List<byte> DataList = new List<byte>();
            DataList.Add(0);
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 2 });
            DataList.AddRange(WeightString(Weight));
            DataList.Add(0);
            DataList.Add(0);
            return DataList.ToArray();
        }


        public static byte[] SetLine1(string Message)
        {
            List<byte> DataList = new List<byte>();
            DataList.Add(0);
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 4 });
            DataList.AddRange(StringToByte(Message));
            DataList.Add(0);
            DataList.Add(0);
            return DataList.ToArray();
        }



        public static byte[] SetLine2(string Message)
        {
            List<byte> DataList = new List<byte>();
            DataList.Add(0);
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 5 });
            DataList.AddRange(StringToByte(Message));
            DataList.Add(0);
            DataList.Add(0);
            return DataList.ToArray();
        }

        public static byte[] SetLine3(string Message)
        {
            List<byte> DataList = new List<byte>();
            DataList.Add(0);
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 6 });
            DataList.AddRange(StringToByte(Message));
            DataList.Add(0);
            DataList.Add(0);
            return DataList.ToArray();
        }


        public static byte[] SetWeightStatus(int Weight, string Status)
        {
            List<byte> DataList = new List<byte>();
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 2 });
            DataList.AddRange(WeightString(Weight));
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 4 });
            DataList.AddRange(StringToByte(Status));
            DataList.Add(0);
            return DataList.ToArray();
        }




        public static byte[] SetAll(int Weight,string Line1,String Line2,String Line3)
        {
            List<byte> DataList = new List<byte>();
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 2 });
            DataList.AddRange(WeightString(Weight));
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 4 });
            DataList.AddRange(StringToByte(Line1));
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 5 });
            DataList.AddRange(StringToByte(Line2));
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 6 });
            DataList.AddRange(StringToByte(Line3));
            DataList.Add(0);
            return DataList.ToArray();
        }

        public static byte[] SetLine2_3(String Line2, String Line3)
        {
            List<byte> DataList = new List<byte>();
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 5 });
            DataList.AddRange(StringToByte(Line2));
            DataList.Add(0);
            DataList.AddRange(new byte[] { 254, 46, 6 });
            DataList.AddRange(StringToByte(Line3));
            DataList.Add(0);
            return DataList.ToArray();
        }







        public static byte[] StringToByte(string Message)
        {
            return System.Text.Encoding.ASCII.GetBytes(Message);
        }

        public static byte[] WeightString(int Weight)
        {

           return System.Text.Encoding.ASCII.GetBytes(String.Format("{0:N0}",Weight));
        }
}