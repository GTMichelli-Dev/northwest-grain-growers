using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class VarietyList
{
    public Guid UID;
    public float Percent;
    public VarietyList(Guid NewUID, float NewPercent)
    {
        UID = NewUID;
        Percent = NewPercent;
    }


}

public class RoundPercent
{


    public List<VarietyList> Values { get; private set; }
    public RoundPercent(List<VarietyList> NumbersToRound)
    {
        Values = NumbersToRound;

    }


    public List<VarietyList> RoundedValues
    {
        get
        {
            List<VarietyList> RValues = new List<VarietyList>();
            if (CanRoundValues)
            {


                float value = 0;
                foreach (var num in Values)
                {
                    value += (int)num.Percent;
                }
                float remainder = 100 - value;
                float divisor = remainder / Values.Count;


                foreach (var variety in Values)
                {
                    RValues.Add(new VarietyList(variety.UID, (float)variety.Percent + (float)divisor));
                }

            }
            else
            {
                foreach (var num in Values)
                {
                    RValues.Add(new VarietyList(num.UID, (float)num.Percent));
                }

            }
            return RValues;
        }
    }




    public bool CanRoundValues
    {
        get
        {
            float value = 0;
            foreach (var num in Values)
            {
                value += (int)num.Percent;
            }

            int ivalue = (int)value;
            if (ivalue < 100 && ivalue >= 99)
            {
                return true;
            }
            else
            {
                return false;
            }

        }






    }



}
