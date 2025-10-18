using Seed25.DTO;
using static PrintApiController;


    public class SampleData
    {


        public static InvoiceDTO GetSampleData(InvoiceRequest pr)
        {
            pr.Type = pr.Type.ToUpper();
            if (pr.Type == "STRING") pr.Type = "TRUCK";
            var dto = new InvoiceDTO
            {
                UID = Guid.NewGuid(),
                Ticket = 123456,
                InvoiceDate = DateTime.Now,
                CustomerName = "3040870 - Valley Agrinomics ",
                PO = "ABC-78910",
                BOL = "ZXQ-345-54321",
                TruckId = "TRK-001",
                Weighmaster = "Mike Johnson",
                Comments = "This is a comment Line ",
                Location = "Walla Walla Seed Plant",
                Address1 = "456 Seed St",
                Address2 = "Walla Walla, WA 99362",
                Phone = "509-555-1234",
                Type = pr.Type,
                RequestedAmount = "Requested 1500 bu.",
                TreatedSeed = pr.TreatedSeed,
                Clearfield = pr.Clearfield,
                Coaxium = pr.Coaxium,
                InvoiceVarietyDTOs = new List<InvoiceVarietyDTO>
            {
                new InvoiceVarietyDTO{ Description="SHINE CERTIFIED SWW", LotNumber="001-654", ItemId=987656, Percent= 0.01M ,Weight=1200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW", LotNumber="002-34321", ItemId=123456, Percent= 0.223M, Weight=11200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", LotNumber="003-45654", ItemId=765432, Percent= 0.2534M, Weight=11200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", LotNumber="004-5432", ItemId=765434, Percent= 0.0254M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", ItemId=765423, Percent= 0.4M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="Shine Certified", ItemId=987656, Percent= 0.02M, Weight=1200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW", ItemId=162345, Percent= 0.33M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", ItemId=763454, Percent= 0.03M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", ItemId=764354, Percent= 0.20M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS AST", ItemId=732654, Percent= 0.100111M, Weight=1 },
            },

                invoiceTreatmentDTOs = new List<InvoiceTreatmentDTO>
            {
                new InvoiceTreatmentDTO{ Description="Apron XL", ItemId =111222, Rate=1.23M, Gallons=12.10M },
                new InvoiceTreatmentDTO{ Description="Helix", ItemId=333444, Rate=0.23M, Gallons=2.10M },
                new InvoiceTreatmentDTO{ Description="Inspire", ItemId=555666, Rate=4.23M, Gallons=882.10M },
                new InvoiceTreatmentDTO{ Description="Seed Start", ItemId =111222, Rate=1.23M, Gallons=12.10M },
                new InvoiceTreatmentDTO{ Description="Albaugh", ItemId=333444, Rate=0.23M, Gallons=2.10M },
                new InvoiceTreatmentDTO{ Description="Legend", ItemId=555666, Rate=4.23M, Gallons=882.10M },
            },
                invoiceMiscDTOs = new List<InvoiceMiscDTO>
            {
                new InvoiceMiscDTO{ ItemID=543212, Description="Loading Fee", Unit="each", Amount= 25.00M },
                new InvoiceMiscDTO{ ItemID=543212, Description="Bags",Unit="each", Amount= 15.00M },
                new InvoiceMiscDTO{ ItemID=543212, Description="Palletts",Unit="each", Amount= 1M },
            },
                invoiceWeightDTOs = new List<InvoiceWeightDTO>
            {
                new InvoiceWeightDTO{  ID=12345678, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345679, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345680, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345681, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345682, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345683, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345684, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345678, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=9, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=3, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=4, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=5, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=6, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=5, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=6, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=7, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=8, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=9, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=3, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=4, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=9, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=3, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=4, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
            },
                invoiceBagsDTOs = new List<InvoiceBagDTO>
            {
                new InvoiceBagDTO{ ID=1, Weight= 50M, Quantity= 100M, Unit="lbs" },
                new InvoiceBagDTO{ ID=2, Weight= 25M, Quantity= 35M, Unit="lbs" },
                new InvoiceBagDTO{ ID=3, Weight= 15.25M, Quantity= 1M, Unit="lbs" },
            },

                invoiceLotDTOs = new List<InvoiceLotDTO>
            {
                new InvoiceLotDTO{ Lot="ABCD-12345", ItemId= 123456,  ItemDescription= "SHINE CERTIFIED SWW" },
                new InvoiceLotDTO{ Lot="AODFD-23456", ItemId= 234567, ItemDescription= "LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW" },
                new InvoiceLotDTO{ Lot="VFG-34567", ItemId= 345678, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },
                new InvoiceLotDTO{ Lot="WSUD-45678", ItemId= 456789, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },
                new InvoiceLotDTO{ Lot="IODdd-56789", ItemId= 567890, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },

            },



            };

            dto.invoiceAnalysisDTOs = dto.InvoiceVarietyDTOs.Select((v, i) => new InvoiceAnalysisDTO
            {
                DateTested = DateTime.Now.AddDays(-2 - i * 3),
                PureSeed = .95m - i * 0.1m,
                Germination = 1,
                OtherCrop = 0.5m + i * 0.1m,
                WeedSeed = 0.2m + i * 0.05m,
                InertMatter = .13m + i * 0.2m,
                ItemDescription = v.Description,
                ItemId = v.ItemId
            }).ToList();


            if (pr.Type == "TRUCK")
            {

                dto.invoiceWeightDTOs = dto.invoiceWeightDTOs.Take(1).ToList();
                dto.invoiceWeightDTOs[0].TruckId = dto.TruckId;
            }
            return dto;
        }



        public static InvoiceDTO GetSampleData(long Ticket)
        {
            var pr = new InvoiceRequest { Type = "Truck", TreatedSeed = true, Clearfield = true, Coaxium = false };
            pr.Type = pr.Type.ToUpper();
            if (pr.Type == "STRING") pr.Type = "TRUCK";
            var dto = new InvoiceDTO
            {
                UID = Guid.NewGuid(),
                Ticket = Ticket,
                InvoiceDate = DateTime.Now,
                CustomerName = "3040870 - Valley Agrinomics ",
                PO = "ABC-78910",
                BOL = "ZXQ-345-54321",
                TruckId = "TRK-001",
                Weighmaster = "Mike Johnson",
                Comments = "This is a comment Line ",
                Location = "Walla Walla Seed Plant",
                Address1 = "456 Seed St",
                Address2 = "Walla Walla, WA 99362",
                Phone = "509-555-1234",
                Type = pr.Type,
                RequestedAmount = "",
                TreatedSeed = pr.TreatedSeed,
                Clearfield = pr.Clearfield,
                Coaxium = pr.Coaxium,
                InvoiceVarietyDTOs = new List<InvoiceVarietyDTO>
            {
                new InvoiceVarietyDTO{ Description="SHINE CERTIFIED SWW", LotNumber="001-654", ItemId=987656, Percent= 0.01M ,Weight=1200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW", LotNumber="002-34321", ItemId=123456, Percent= 0.223M, Weight=11200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", LotNumber="003-45654", ItemId=765432, Percent= 0.2534M, Weight=11200 },
                new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", LotNumber="004-5432", ItemId=765434, Percent= 0.0254M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", ItemId=765423, Percent= 0.4M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="Shine Certified", ItemId=987656, Percent= 0.02M, Weight=1200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW", ItemId=162345, Percent= 0.33M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", ItemId=763454, Percent= 0.03M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS HYDRAX -777 COAXIMUM REGISTERED SWW", ItemId=764354, Percent= 0.20M, Weight=11200 },
                //new InvoiceVarietyDTO{ Description="LCS AST", ItemId=732654, Percent= 0.100111M, Weight=1 },
            },

                invoiceTreatmentDTOs = new List<InvoiceTreatmentDTO>
            {
                new InvoiceTreatmentDTO{ Description="Apron XL", ItemId =111222, Rate=1.23M, Gallons=12.10M },
                new InvoiceTreatmentDTO{ Description="Helix", ItemId=333444, Rate=0.23M, Gallons=2.10M },
                new InvoiceTreatmentDTO{ Description="Inspire", ItemId=555666, Rate=4.23M, Gallons=882.10M },
                new InvoiceTreatmentDTO{ Description="Seed Start", ItemId =111222, Rate=1.23M, Gallons=12.10M },
                new InvoiceTreatmentDTO{ Description="Albaugh", ItemId=333444, Rate=0.23M, Gallons=2.10M },
                new InvoiceTreatmentDTO{ Description="Legend", ItemId=555666, Rate=4.23M, Gallons=882.10M },
            },
                invoiceMiscDTOs = new List<InvoiceMiscDTO>
            {
                new InvoiceMiscDTO{ ItemID=543212, Description="Loading Fee", Unit="each", Amount= 25.00M },
                new InvoiceMiscDTO{ ItemID=543212, Description="Bags",Unit="each", Amount= 15.00M },
                new InvoiceMiscDTO{ ItemID=543212, Description="Palletts",Unit="each", Amount= 1M },
            },
                invoiceWeightDTOs = new List<InvoiceWeightDTO>
            {
                new InvoiceWeightDTO{  ID=12345678, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345679, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345680, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345681, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345682, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345683, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345684, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=12345678, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=9, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=3, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=4, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=5, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=6, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=5, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=6, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=7, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=8, Gross= 28000M, Tare= 15000M, Net= 13000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=9, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=3, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=4, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=9, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                 new InvoiceWeightDTO{ ID=3, Gross= 32000M, Tare= 18000M, Net= 14000M, Unit="lbs" },
                new InvoiceWeightDTO{ ID=4, Gross= 23000M, Tare= 12000M, Net= 11000M, Unit="lbs" },
            },
                invoiceBagsDTOs = new List<InvoiceBagDTO>
            {
                new InvoiceBagDTO{ ID=1, Weight= 50M, Quantity= 100M, Unit="lbs" },
                new InvoiceBagDTO{ ID=2, Weight= 25M, Quantity= 35M, Unit="lbs" },
                new InvoiceBagDTO{ ID=3, Weight= 15.25M, Quantity= 1M, Unit="lbs" },
            },

                invoiceLotDTOs = new List<InvoiceLotDTO>
            {
                new InvoiceLotDTO{ Lot="ABCD-12345", ItemId= 123456,  ItemDescription= "SHINE CERTIFIED SWW" },
                new InvoiceLotDTO{ Lot="AODFD-23456", ItemId= 234567, ItemDescription= "LCS HYDRAX -1555 COAXIMUM CERTIFIED SWW" },
                new InvoiceLotDTO{ Lot="VFG-34567", ItemId= 345678, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },
                new InvoiceLotDTO{ Lot="WSUD-45678", ItemId= 456789, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },
                new InvoiceLotDTO{ Lot="IODdd-56789", ItemId= 567890, ItemDescription= "LCS HYDRAX -777 COAXIMUM REGISTERED SWW" },

            },



            };

            dto.invoiceAnalysisDTOs = dto.InvoiceVarietyDTOs.Select((v, i) => new InvoiceAnalysisDTO
            {
                DateTested = DateTime.Now.AddDays(-2 - i * 3),
                PureSeed = .95m - i * 0.1m,
                Germination = 1,
                OtherCrop = 0.5m + i * 0.1m,
                WeedSeed = 0.2m + i * 0.05m,
                InertMatter = .13m + i * 0.2m,
                ItemDescription = v.Description,
                ItemId = v.ItemId
            }).ToList();


            if (pr.Type == "TRUCK")
            {

                dto.invoiceWeightDTOs = dto.invoiceWeightDTOs.Take(1).ToList();
                dto.invoiceWeightDTOs[0].TruckId = dto.TruckId;
            }
            return dto;
        }
    }
