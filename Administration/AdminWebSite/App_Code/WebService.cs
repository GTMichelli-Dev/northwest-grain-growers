using BinData;
using DocumentFormat.OpenXml.Drawing.ChartDrawing;
using EFOptions.Dto;
using EFOptions.Models;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;

/// <summary>
/// Summary description for WebService
/// </summary>
[WebService(Namespace = "http://walws001/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class WebService : System.Web.Services.WebService
{

    public WebService()
    {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }


    [WebMethod]
    public void UpdateLicensedStatus(Guid uid, bool licensed)
    {
        using (var context = new EFOptions.Models.NW_DataContext())
        {
            var option = context.LocationOptions
                .FirstOrDefault(lo => lo.Uid == uid && lo.Description == "Licensed");

            if (option != null)
            {
                option.Value = licensed ? "True" : "False";
                context.SaveChanges();
            }
        }
    }   

    //[WebMethod]
    //public string GetLocationLicenseType()
    //{
    //    Options options = new Options();

    //    options.EnsureAllLocationsHaveLicensedOption();  // sync version
    //    var list = options.LoadLocationLicenseDtos();    // sync version

    //    var json = JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings
    //    {
    //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    //    });

    //    return json;
    //}


    [WebMethod]
    public string GetLocationFilters()
    {
        Options options = new Options();

        
        var list = options.GetLocationFilters();    // sync version

        var json = JsonConvert.SerializeObject(list, Formatting.Indented, new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        });

        return json;
    }



    [WebMethod]
    public string GetLocations()
    {
        
        using (var context = new NW_DataContext())
        {
            var locations = context.Locations.Select(x => new LocationDTO
            {
                Uid = x.Uid,
                Id=x.Id,
                Description = x.Description,
                Active = x.Active,
                District = x.District,
              

            }).OrderBy(x => x.Description).ToList();
            var json = JsonConvert.SerializeObject(locations, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
        }

       

      
    }


    [WebMethod]
    public AjaxResponse AddNewLocationFilter(int FromLocationId,string FromDescription ,int ToLocationId ,string ToDescription)
    {
        var ar = new AjaxResponse();
        try
        {

            var options = new Options();    
            var locationFilters = options.GetLocationFilters();
            if (locationFilters.Any(x => x.SourceLocationId == FromLocationId && x.DestinationLocationId == ToLocationId))
            {
                ar.Message = $"Filter already From {FromDescription} To {ToDescription} exists.";
                ar.Success = true;
                return ar;
            }
            locationFilters.Add(new TransferFilterDto
            {
                Uid = Guid.NewGuid(),
                SourceLocationId = FromLocationId,
                DestinationLocationId = ToLocationId,
                SourceDescription = FromDescription,
                DestinationDescription = ToDescription
            }); 
            options.SaveTransferFilters(locationFilters);
            return new AjaxResponse
            {
                Success = true,
                Message = $"Filter added From {FromDescription} To {ToDescription}."
            };
        }
        catch (DbEntityValidationException e)
        {
            ar.Message = ValidationErrors.getValidationErrors(e);
        }
        catch (Exception ex)
        {
            ar.Message = ex.Message + "<br/>" + ex.GetBaseException().Message;
        }
        return ar;
    }


    [WebMethod]
    public AjaxResponse DeleteLocationFilter(Guid UID)
    {
        var ar = new AjaxResponse();
        try
        {

            var options = new Options();
            var locationFilters = options.GetLocationFilters();
            locationFilters.RemoveAll(x => x.Uid == UID);
            

            options.SaveTransferFilters(locationFilters);
            return new AjaxResponse
            {
                Success = true,
                Message = $"Filter removed."
            };
        }
        catch (DbEntityValidationException e)
        {
            ar.Message = ValidationErrors.getValidationErrors(e);
        }
        catch (Exception ex)
        {
            ar.Message = ex.Message + "<br/>" + ex.GetBaseException().Message;
        }
        return ar;
    }


    [WebMethod]
    public AjaxResponse GetCropProducers()
    {
        var ar = new AjaxResponse();
        try
        {
            var cropProducers = LinqQuerys.GetCropProducers();
            ar.Success = true;
            ar.Data = JsonConvert.SerializeObject(cropProducers);

        }
        catch (DbEntityValidationException e)
        {
            ar.Message = ValidationErrors.getValidationErrors(e);
        }
        catch (Exception ex)
        {
            ar.Message = ex.Message + "<br/>" + ex.GetBaseException().Message;

        }
        return ar;
    }



    [WebMethod]
    public AjaxResponse AddCropProducer(Guid CropUID,string NameID)
    {
        var ar = new AjaxResponse();
        try
        {
            using (var db = new NWDataModel())
            {
                var GrowerRow = LinqQuerys.GetProducerNameID_UID_List().FirstOrDefault(x => x.NameID.ToLower() == NameID.ToLower());
               
                var exists = LinqQuerys.GetCropProducers().FirstOrDefault(x => x.CropUID == CropUID && x.ProducerUID == GrowerRow.UID);
                if (exists != null)
                {
                    ar.Message = $"Crop {exists.Crop} and {exists.Producer} Filter Already Exists";
                }
                else
                {
                    db.CropProducerFilters.Add(new CropProducerFilter { CropUID = CropUID, ProducerUID = GrowerRow.UID, UID = Guid.NewGuid() });
                    db.SaveChanges();
                    ar.Success = true;




                }
            }

        }
        catch (DbEntityValidationException e)
        {
            ar.Message = ValidationErrors.getValidationErrors(e);
        }
        catch (Exception ex)
        {
            ar.Message = ex.Message + "<br/>" + ex.GetBaseException().Message;

        }
        return ar;
    }


    [WebMethod]
    public AjaxResponse DeleteCropProducer(Guid UID)
    {
        var ar = new AjaxResponse();
        try
        {
            using (var db = new NWDataModel())
            {
                db.CropProducerFilters.Remove(db.CropProducerFilters.FirstOrDefault(x => x.UID == UID));
                db.SaveChanges();
                ar.Success = true;

            }

        }
        catch (DbEntityValidationException e)
        {
            ar.Message = ValidationErrors.getValidationErrors(e);
        }
        catch (Exception ex)
        {
            ar.Message = ex.Message + "<br/>" + ex.GetBaseException().Message;

        }
        return ar;
    }

    [WebMethod]
    public AjaxResponse GetProducers(string filter)
    {
        var ar = new AjaxResponse();
        try
        {
            ar.Success = true;
            var lst = LinqQuerys.GetProducerNameID_UID_List().Where(x=>x.NameID.ToLower().Contains(filter.ToLower())).Select(x => x.NameID);
            ar.Data = JsonConvert.SerializeObject(lst);
        }
        catch (DbEntityValidationException e)
        {
            ar.Message = ValidationErrors.getValidationErrors(e);
        }
        catch (Exception ex)
        {
            ar.Message = ex.Message + "<br/>" + ex.GetBaseException().Message;

        }
        return ar;
    }




    [WebMethod]
    public AjaxResponse CheckProducer(string Filter)
    {
        var ar = new AjaxResponse();
        try
        {
            var found = LinqQuerys.GetProducerNameID_UID_List().FirstOrDefault(x => x.NameID.ToLower() == Filter.ToLower());
            ar.Success = (found != null);
            if (found != null)ar.Data = found.NameID;
            
        }
        catch (DbEntityValidationException e)
        {
            ar.Message = ValidationErrors.getValidationErrors(e);
        }
        catch (Exception ex)
        {
            ar.Message = ex.Message + "<br/>" + ex.GetBaseException().Message;

        }
        return ar;
    }





    [WebMethod]
    public AjaxResponse GetCrops()
    {
        var ar = new AjaxResponse();
        try
        {
            using (var db = new NWDataModel())
            {
                var list = db.Crops.Where(x => x.Id != 1).OrderBy(x => x.Description).ToList();

                ar = GetCropProducers();
                ar.Success = true;
                ar.Data = JsonConvert.SerializeObject(list);
            }



        }
        catch (DbEntityValidationException e)
        {
            ar.Message = ValidationErrors.getValidationErrors(e);
        }
        catch (Exception ex)
        {
            ar.Message = ex.Message + "<br/>" + ex.GetBaseException().Message;

        }
        return ar;
    }


    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public string GetBins()
    {
        using (var context = new BinDBContext())
        {
            var query = from location in context.Locations
                        join bin in context.Bins on location.Id equals bin.LocationId
                        join adjustmentGroup in
                            (from ba in context.BinAdjustments
                             group ba by ba.BinUid into g
                             select new
                             {
                                 UID = g.Key,
                                 AdjustedDate = g.Max(ba => ba.AdjustedDate)
                             })
                        on bin.Uid equals adjustmentGroup.UID into adjustmentGroups
                        from adjGroup in adjustmentGroups.DefaultIfEmpty()
                        join adjustment in context.BinAdjustments
                        on new { BinUid = bin.Uid, AdjustedDate = adjGroup.AdjustedDate } equals new { adjustment.BinUid, adjustment.AdjustedDate } into adjustments
                        from adj in adjustments.DefaultIfEmpty()
                        orderby location.Description, bin.Bin1
                        select new BinDTO
                        {
                            AdjustedDate = adjGroup.AdjustedDate,
                            LocationId = bin.LocationId,
                            Bin = bin.Bin1,
                            Location = location.Description,
                            BinUID = bin.Uid,
                            District = location.District,
                            Bushels = adj != null ? adj.Bushels : 0,
                            Protein = adj != null ? (float)adj.Protein : 0,
                            Comment = adj != null ? adj.Comment : string.Empty,
                            AdjustmentUID = adj != null ? adj.Uid : Guid.Empty,
                        };

            var bins = query.ToList();
            JavaScriptSerializer js = new JavaScriptSerializer();
            return JsonConvert.SerializeObject(bins);
        }
    }

    [WebMethod]
    public byte[] DownloadTicket(Guid UID)
    {
        MemoryStream stream = SeedPlantPrinting.Send_TicketToBrowser(UID);
        return stream.ToArray();
    }

}
