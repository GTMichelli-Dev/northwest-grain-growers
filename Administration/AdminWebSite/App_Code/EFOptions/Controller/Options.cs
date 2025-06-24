
using DocumentFormat.OpenXml.Drawing.Diagrams;
using EFOptions.Dto;
using EFOptions.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
public class Options
{
    public List<TransferFilterDto> GetTransferFilters() {
        using ( var context = new NW_DataContext())
        {
            var TransferFilterOption = context.LocationOptions
             .Where(opt => opt.Description == "TransferFilter" && opt.LocationId==0)
             .Select(opt => new
             {
                 Uid = opt.Uid,
                 Description = opt.Description,
                 Value = opt.Value
             })
             .FirstOrDefault();

            if (TransferFilterOption == null)
            {
                return new List<TransferFilterDto>();
            }
           
            
            List<TransferFilterDto> transferFilters;
            try
            {
                transferFilters = JsonConvert.DeserializeObject<List<TransferFilterDto>>(TransferFilterOption.Value);
                return transferFilters ?? new List<TransferFilterDto>();
            }
            catch
            {
                // If deserialization fails, return a list with a new TransferFilterDto
                return new List<TransferFilterDto> { new TransferFilterDto() };
            }

        }

    }

    public void SaveTransferFilters(List<TransferFilterDto> transferFilters)
    {
        using (var context = new NW_DataContext())
        {
            var transferfilterOption = context.LocationOptions
                .Where(opt => opt.Description == "TransferFilter" && opt.LocationId == 0)
                .FirstOrDefault();

            var values =  JsonConvert.SerializeObject(transferFilters);
            if (transferfilterOption== null)
            {
                transferfilterOption = new LocationOption
                {
                    Description = "TransferFilter",
                    LocationId = 0,
                    Value = values,
                };
                context.LocationOptions.Add(transferfilterOption);
                
            }
            else
            {
                transferfilterOption.Value = values;
            }
            context.SaveChanges();
        }
    }

    public List<TransferFilterDto> GetLocationFilters()
    {
        using (var context = new NW_DataContext())
        {
            // Get all locations
            //var locations = context.Locations
            //    .Select(loc => new { loc.Id, loc.Description })
            //    .ToList();

            // Get transfer filters
            var transferFilters = GetTransferFilters();

            // Build the result list
            //var result = locations.Select(loc =>
            //{
            //    bool isFiltered = transferFilters.Any(tf => tf.SourceLocationId == loc.Id);
            //    return new LocationFilter
            //    {
            //        Id = loc.Id,
            //        Description = loc.Description,
            //        FilterDescription = isFiltered ? "Filtered Sites" : "No Filtering"
            //    };
            //}).ToList();

            return transferFilters;
        }

    }


    //public  List<SourceLocationFilters> GetFiltersForLocation(int Id){
    //    using (var context = new NW_DataContext())
    //    {
    //        // Get all locations
    //        var locations = context.Locations
    //            .Select(loc => new { loc.Id, loc.Description })
    //            .ToList();

    //        // Get all transfer filters where SourceLocationId == Id
    //        var transferFilters = GetTransferFilters()
    //            .Where(tf => tf.SourceLocationId == Id)
    //            .ToList();

    //        // Build the result list
    //        var result = locations.Select(loc =>
    //        {
    //            bool isFiltered = transferFilters.Any(tf => tf.DestinationId == loc.Id);
    //            return new SourceLocationFilters
    //            {
    //                DestinationId = loc.Id,
    //                DestinationDescription = loc.Description,
    //                Filtered = isFiltered
    //            };
    //        }).ToList();

    //        return result;
    //    }
    //}

    //public void EnsureAllServersInServerLocationAsync()
    //{
      
    //        using (var context = new NW_DataContext())
    //        {
    //            var allServerUids = context.Servers.Select(s => s.Uid).ToList();
    //            var existingServerUids = context.ServerLocations.Select(sl => sl.ServerUid).ToList();

    //            var missingServerUids = allServerUids.Except(existingServerUids).ToList();

    //            foreach (var uid in missingServerUids)
    //            {
    //                var serverLocation = new ServerLocation
    //                {
    //                    Uid = Guid.NewGuid(),
    //                    ServerUid = uid,
    //                    Description = string.Empty,
    //                    RemotePrint = false,
    //                };

    //                context.ServerLocations.Add(serverLocation);
    //            }

    //            context.SaveChanges();
    //        }
    
    //}

    //public void EnsureAllLocationsHaveLicensedOption()
    //{
     
    //        using (var context = new NW_DataContext())
    //        {
    //            // Use int Id (not Uid) for matching
    //            var allLocationIds = context.Locations.Select(loc => loc.Id).ToList();

    //            var existingLicensedIds = context.LocationOptions
    //                .Where(opt => opt.Description == "Licensed")
    //                .Select(opt => opt.LocationId)
    //                .ToList();

    //            var missingLocationIds = allLocationIds.Except(existingLicensedIds).ToList();

    //            foreach (var id in missingLocationIds)
    //            {
    //                var newOption = new LocationOption
    //                {
    //                    Uid = Guid.NewGuid(),
    //                    LocationId = id,
    //                    Description = "Licensed",
    //                    Value = "True"
    //                };

    //                context.LocationOptions.Add(newOption);
    //            }

    //            context.SaveChanges();
    //        }
        
    //}




    //public List<LocationLicenseDto> LoadLocationLicenseDtos()
    //{
    //    using (var context = new NW_DataContext())
    //    {
    //        var query = from sl in context.Locations
    //                    join s in context.LocationOptions on sl.Id equals s.LocationId
    //                    where s.Description == "Licensed" && sl.Id>0

    //                    orderby sl.Description
    //                    select new
    //                    {
    //                        s.Uid,
    //                        s.Value,
    //                        sl.Id,
    //                        sl.Description
    //                    };

    //        var result = query.ToList() // Execute the query and load data into memory
    //            .Select(x => new LocationLicenseDto
    //            {
    //                Uid = x.Uid,
    //                Licensed = bool.Parse(x.Value), // Perform parsing in memory
    //                Id = x.Id,
    //                Description = x.Description
    //            }).OrderBy(x=> x.Description ).ToList();

    //        return result;
    //    }
    //}




    public   List<ServerLocationDto> LoadServerLocationDtos()
    {
      
            using (var context = new NW_DataContext())
            {
                var query = from sl in context.ServerLocations
                            join s in context.Servers on sl.ServerUid equals s.Uid
                            where s.ServerName != "ARCHIVE"
                               && s.ServerName != "Leo"
                               && s.ServerName != "MISSION-SRV"
                               && s.ServerName != "Endicott-NUC"
                               && s.ServerName != "SHEF-SRV"
                               && s.ServerName != "ENDPC002"
                               && s.ServerName != "LYNDB001"
                               && s.ServerName != "PRES-SRV"
                               && s.ServerName != "SPOF-SRV"
                               && s.ServerName != "DAYTON-SRV"
                               && s.ServerName != "WALDB001"
                               && s.ServerName != "PK-SRV"
                               && s.ServerName != "LSPPC003"
                               && s.ServerName != "DSPDB001"
                               && s.ServerName != "DELDB001"
                               && s.ServerName != ""


                            orderby s.ServerName
                            select new ServerLocationDto
                            {
                                Uid = sl.Uid,
                                ServerUid = s.Uid,
                                ServerName = s.ServerName,
                                Description = sl.Description
                            };

                return query.ToList();

            }
      
    }



}

