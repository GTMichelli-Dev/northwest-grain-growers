using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWGrain
{
    class Misc
    {



        public static class SplitWeight
        {

            //    static string Header(bool Inbound)
            //    {
            //        if (Inbound)
            //        {
            //            return "<ISplit>";
            //        }
            //        else
            //        {
            //            return "<OSplit>";
            //        }
            //    }


            //    static string Footer(bool Inbound)
            //    {
            //        if (Inbound)
            //        {
            //            return "</ISplit>";
            //        }
            //        else
            //        {
            //            return "</OSplit>";
            //        }
            //    }




            //    public static string Add_Split_Weights(string CurrentComment, string SplitWeightComment, bool Inbound)
            //    {
            //        CurrentComment = Remove_Split_Weights(CurrentComment, Inbound);
            //        string NewSplitWeighComment = Header(Inbound) + SplitWeightComment + Footer(Inbound);
            //        return CurrentComment += NewSplitWeighComment;

            //    }



            //    public static string Remove_Split_Weights(string CurrentComment, bool Inbound)
            //    {
            //        int Start = CurrentComment.IndexOf(Header(Inbound));
            //        int End = CurrentComment.IndexOf(Footer(Inbound));
            //        if (Start > -1 && End > Start)
            //        {
            //            Start += Header(Inbound).Length;
            //            End += Footer(Inbound).Length;
            //            CurrentComment = CurrentComment.Remove(Start- Header(Inbound).Length , (End - (Start-Header(Inbound).Length)));
            //        }

            //        return CurrentComment;

            //    }



            public static void SaveSplitWeights(VirtualDataset.SplitWeightDataTable splitWeights, Guid LoadUID, bool inbound)
            {
                using (VirtualDatasetTableAdapters.Load_SplitsTableAdapter load_SplitsTableAdapter = new VirtualDatasetTableAdapters.Load_SplitsTableAdapter())
                {
                    using (VirtualDataset.Load_SplitsDataTable load_Splits = new VirtualDataset.Load_SplitsDataTable())
                    {
                        foreach (var items in splitWeights)
                        {
                            var row = load_Splits.NewLoad_SplitsRow();
                            row.UID = Guid.NewGuid();
                            row.TimeWeighed = items.TimeWeighed;
                            row.Load_UID = LoadUID;
                            row.Inbound = inbound;
                            row.Weight = items.Weight;
                            load_Splits.AddLoad_SplitsRow(row);
                        }
                        load_SplitsTableAdapter.Update(load_Splits);
                    }
                }

            }

            public static int Weighments(Guid Load_UID, bool Inbound)
            {
                int WeightCount = 0;
                try
                {
                    using (VirtualDatasetTableAdapters.QueriesTableAdapter Q = new VirtualDatasetTableAdapters.QueriesTableAdapter())
                    {
                        int? Count = (int?)Q.CountOfSplits(Inbound, Load_UID);
                        if (Count != null)
                        {
                            WeightCount = (int)Count;
                        }
                    }
                }
                catch
                {

                }


                return (WeightCount < 1) ? 1 : WeightCount;


                //    }



            }
        }
    }
}
