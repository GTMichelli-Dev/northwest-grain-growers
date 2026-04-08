using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicWeigh.Web.Data;
using BasicWeigh.Web.Models;
using BasicWeigh.Web.Services;

namespace BasicWeigh.Web.Controllers;

public class MasterDataController : Controller
{
    private readonly ScaleDbContext _db;
    private readonly AppSetupCache _setupCache;

    public MasterDataController(ScaleDbContext db, AppSetupCache setupCache)
    {
        _db = db;
        _setupCache = setupCache;
    }

    public IActionResult Index()
    {
        var setup = _setupCache.Get();
        ViewBag.KioskCount = setup.KioskCount;
        ViewBag.UseQuickBooks = setup.UseQuickBooks;
        return View();
    }

    // ---- Customers ----
    [HttpGet("api/masterdata/customers")]
    public IActionResult GetCustomers()
    {
        return Json(_db.Customers.OrderBy(c => c.CustomerName).ToList());
    }

    [HttpPost("api/masterdata/customers")]
    public IActionResult AddCustomer([FromBody] Customer customer)
    {
        _db.Customers.Add(customer);
        _db.SaveChanges();
        return Json(customer);
    }

    [HttpPut("api/masterdata/customers")]
    public IActionResult UpdateCustomer([FromBody] JsonElement body)
    {
        var id = body.GetProperty("id").GetInt32();
        var existing = _db.Customers.Find(id);
        if (existing == null) return NotFound();
        if (body.TryGetProperty("customerName", out var name)) existing.CustomerName = name.GetString()!;
        if (body.TryGetProperty("active", out var active))
        {
            existing.Active = active.GetBoolean();
            if (!existing.Active) existing.UseAtKiosk = false;
        }
        if (body.TryGetProperty("useAtKiosk", out var kiosk)) existing.UseAtKiosk = kiosk.GetBoolean();
        _db.SaveChanges();
        return Json(existing);
    }

    [HttpDelete("api/masterdata/customers/{id:int}")]
    public IActionResult DeleteCustomer(int id)
    {
        var entity = _db.Customers.Find(id);
        if (entity == null) return NotFound();
        _db.Customers.Remove(entity);
        _db.SaveChanges();
        return Json(new { success = true });
    }

    // ---- Customers not already carriers (for carrier name suggestions) ----
    [HttpGet("api/masterdata/customers-not-carriers")]
    public IActionResult GetCustomersNotCarriers()
    {
        var carrierNames = _db.Carriers.Select(c => c.CarrierName).ToHashSet();
        var customers = _db.Customers
            .Where(c => c.Active && !carrierNames.Contains(c.CustomerName))
            .OrderBy(c => c.CustomerName)
            .Select(c => c.CustomerName)
            .ToList();
        return Json(customers);
    }

    // ---- Carriers ----
    [HttpGet("api/masterdata/carriers")]
    public IActionResult GetCarriers()
    {
        return Json(_db.Carriers.OrderBy(c => c.CarrierName).ToList());
    }

    [HttpPost("api/masterdata/carriers")]
    public IActionResult AddCarrier([FromBody] Carrier carrier)
    {
        _db.Carriers.Add(carrier);
        _db.SaveChanges();
        return Json(carrier);
    }

    [HttpPut("api/masterdata/carriers")]
    public IActionResult UpdateCarrier([FromBody] JsonElement body)
    {
        var id = body.GetProperty("id").GetInt32();
        var existing = _db.Carriers.Find(id);
        if (existing == null) return NotFound();
        if (body.TryGetProperty("carrierName", out var name)) existing.CarrierName = name.GetString()!;
        if (body.TryGetProperty("active", out var active))
        {
            existing.Active = active.GetBoolean();
            if (!existing.Active) existing.UseAtKiosk = false;
        }
        if (body.TryGetProperty("useAtKiosk", out var kiosk)) existing.UseAtKiosk = kiosk.GetBoolean();
        _db.SaveChanges();
        return Json(existing);
    }

    [HttpDelete("api/masterdata/carriers/{id:int}")]
    public IActionResult DeleteCarrier(int id)
    {
        var entity = _db.Carriers.Find(id);
        if (entity == null) return NotFound();
        _db.Carriers.Remove(entity);
        _db.SaveChanges();
        return Json(new { success = true });
    }

    // ---- Locations ----
    [HttpGet("api/masterdata/locations")]
    public IActionResult GetLocations()
    {
        return Json(_db.Locations.OrderBy(l => l.LocationName).ToList());
    }

    [HttpPost("api/masterdata/locations")]
    public IActionResult AddLocation([FromBody] Location location)
    {
        _db.Locations.Add(location);
        _db.SaveChanges();
        return Json(location);
    }

    [HttpPut("api/masterdata/locations")]
    public IActionResult UpdateLocation([FromBody] JsonElement body)
    {
        var id = body.GetProperty("id").GetInt32();
        var existing = _db.Locations.Find(id);
        if (existing == null) return NotFound();
        if (body.TryGetProperty("locationName", out var name)) existing.LocationName = name.GetString()!;
        if (body.TryGetProperty("active", out var active))
        {
            existing.Active = active.GetBoolean();
            if (!existing.Active) existing.UseAtKiosk = false;
        }
        if (body.TryGetProperty("useAtKiosk", out var kiosk)) existing.UseAtKiosk = kiosk.GetBoolean();
        _db.SaveChanges();
        return Json(existing);
    }

    [HttpDelete("api/masterdata/locations/{id:int}")]
    public IActionResult DeleteLocation(int id)
    {
        var entity = _db.Locations.Find(id);
        if (entity == null) return NotFound();
        _db.Locations.Remove(entity);
        _db.SaveChanges();
        return Json(new { success = true });
    }

    // ---- Destinations ----
    [HttpGet("api/masterdata/destinations")]
    public IActionResult GetDestinations()
    {
        return Json(_db.Destinations.OrderBy(d => d.DestinationName).ToList());
    }

    [HttpPost("api/masterdata/destinations")]
    public IActionResult AddDestination([FromBody] Destination destination)
    {
        _db.Destinations.Add(destination);
        _db.SaveChanges();
        return Json(destination);
    }

    [HttpPut("api/masterdata/destinations")]
    public IActionResult UpdateDestination([FromBody] JsonElement body)
    {
        var id = body.GetProperty("id").GetInt32();
        var existing = _db.Destinations.Find(id);
        if (existing == null) return NotFound();
        if (body.TryGetProperty("destinationName", out var name)) existing.DestinationName = name.GetString()!;
        if (body.TryGetProperty("active", out var active))
        {
            existing.Active = active.GetBoolean();
            if (!existing.Active) existing.UseAtKiosk = false;
        }
        if (body.TryGetProperty("useAtKiosk", out var kiosk)) existing.UseAtKiosk = kiosk.GetBoolean();
        _db.SaveChanges();
        return Json(existing);
    }

    [HttpDelete("api/masterdata/destinations/{id:int}")]
    public IActionResult DeleteDestination(int id)
    {
        var entity = _db.Destinations.Find(id);
        if (entity == null) return NotFound();
        _db.Destinations.Remove(entity);
        _db.SaveChanges();
        return Json(new { success = true });
    }

    // ---- Trucks ----
    [HttpGet("api/masterdata/trucks")]
    public IActionResult GetTrucks()
    {
        return Json(_db.Trucks.OrderBy(t => t.CarrierName).ThenBy(t => t.TruckId).ToList());
    }

    [HttpGet("api/masterdata/trucks/{carrierName}")]
    public IActionResult GetTrucksByCarrier(string carrierName)
    {
        var trucks = _db.Trucks
            .Where(t => t.CarrierName == carrierName)
            .OrderBy(t => t.TruckId)
            .ToList();
        return Json(trucks);
    }

    [HttpPost("api/masterdata/trucks")]
    public IActionResult AddTruck([FromBody] Truck truck)
    {
        var existing = _db.Trucks.FirstOrDefault(t => t.TruckId == truck.TruckId && t.CarrierName == truck.CarrierName);
        if (existing != null)
            return Json(existing);

        // Auto-create carrier if it doesn't exist (customer used as carrier)
        if (!_db.Carriers.Any(c => c.CarrierName == truck.CarrierName))
        {
            _db.Carriers.Add(new Carrier { CarrierName = truck.CarrierName, Active = true });
        }

        _db.Trucks.Add(truck);
        _db.SaveChanges();
        return Json(truck);
    }

    [HttpPut("api/masterdata/trucks")]
    public IActionResult UpdateTruck([FromBody] JsonElement body)
    {
        var id = body.GetProperty("id").GetInt32();
        var existing = _db.Trucks.Find(id);
        if (existing == null) return NotFound();
        if (body.TryGetProperty("truckId", out var truckId)) existing.TruckId = truckId.GetString()!;
        if (body.TryGetProperty("description", out var desc)) existing.Description = desc.ValueKind == JsonValueKind.Null ? null : desc.GetString();
        if (body.TryGetProperty("notes", out var notes)) existing.Notes = notes.ValueKind == JsonValueKind.Null ? null : notes.GetString();
        if (body.TryGetProperty("useAtKiosk", out var kiosk)) existing.UseAtKiosk = kiosk.GetBoolean();
        _db.SaveChanges();
        return Json(existing);
    }

    [HttpDelete("api/masterdata/trucks/{id:int}")]
    public IActionResult DeleteTruck(int id)
    {
        var entity = _db.Trucks.Find(id);
        if (entity == null) return NotFound();
        _db.Trucks.Remove(entity);
        _db.SaveChanges();
        return Json(new { success = true });
    }

    // ---- Sync from QuickBooks ----

    public class SyncRequest
    {
        public List<string> Names { get; set; } = new();
    }

    [HttpPost("api/masterdata/sync/customers")]
    public IActionResult SyncCustomers([FromBody] SyncRequest request)
    {
        if (request.Names == null || request.Names.Count == 0)
            return BadRequest(new { error = "Names list is required." });

        var qbNames = new HashSet<string>(request.Names, StringComparer.OrdinalIgnoreCase);
        var existing = _db.Customers.ToList();
        var existingByName = existing.ToDictionary(c => c.CustomerName, c => c, StringComparer.OrdinalIgnoreCase);

        int added = 0, reactivated = 0, deactivated = 0, unchanged = 0;

        // Add or reactivate customers from QB
        foreach (var name in qbNames)
        {
            if (existingByName.TryGetValue(name, out var customer))
            {
                if (!customer.Active)
                {
                    customer.Active = true;
                    customer.UseAtKiosk = true;
                    reactivated++;
                }
                else
                {
                    unchanged++;
                }
            }
            else
            {
                _db.Customers.Add(new Customer
                {
                    CustomerName = name,
                    Active = true,
                    UseAtKiosk = true
                });
                added++;
            }
        }

        // Deactivate customers not in QB
        foreach (var customer in existing)
        {
            if (customer.Active && !qbNames.Contains(customer.CustomerName))
            {
                customer.Active = false;
                customer.UseAtKiosk = false;
                deactivated++;
            }
        }

        _db.SaveChanges();
        return Json(new { added, reactivated, deactivated, unchanged });
    }

    [HttpPost("api/masterdata/sync/commodities")]
    public IActionResult SyncCommodities([FromBody] SyncRequest request)
    {
        if (request.Names == null || request.Names.Count == 0)
            return BadRequest(new { error = "Names list is required." });

        var qbNames = new HashSet<string>(request.Names, StringComparer.OrdinalIgnoreCase);
        var existing = _db.Commodities.ToList();
        var existingByName = existing.ToDictionary(c => c.CommodityName, c => c, StringComparer.OrdinalIgnoreCase);

        int added = 0, reactivated = 0, deactivated = 0, unchanged = 0;

        // Add or reactivate commodities from QB
        foreach (var name in qbNames)
        {
            if (existingByName.TryGetValue(name, out var commodity))
            {
                if (!commodity.Active)
                {
                    commodity.Active = true;
                    commodity.UseAtKiosk = true;
                    reactivated++;
                }
                else
                {
                    unchanged++;
                }
            }
            else
            {
                _db.Commodities.Add(new Commodity
                {
                    CommodityName = name,
                    Active = true,
                    UseAtKiosk = true
                });
                added++;
            }
        }

        // Deactivate commodities not in QB
        foreach (var commodity in existing)
        {
            if (commodity.Active && !qbNames.Contains(commodity.CommodityName))
            {
                commodity.Active = false;
                commodity.UseAtKiosk = false;
                deactivated++;
            }
        }

        _db.SaveChanges();
        return Json(new { added, reactivated, deactivated, unchanged });
    }

    // ---- Commodities ----
    [HttpGet("api/masterdata/commodities")]
    public IActionResult GetCommodities()
    {
        return Json(_db.Commodities.OrderBy(c => c.CommodityName).ToList());
    }

    [HttpPost("api/masterdata/commodities")]
    public IActionResult AddCommodity([FromBody] Commodity commodity)
    {
        _db.Commodities.Add(commodity);
        _db.SaveChanges();
        return Json(commodity);
    }

    [HttpPut("api/masterdata/commodities")]
    public IActionResult UpdateCommodity([FromBody] JsonElement body)
    {
        var id = body.GetProperty("id").GetInt32();
        var existing = _db.Commodities.Find(id);
        if (existing == null) return NotFound();
        if (body.TryGetProperty("commodityName", out var name)) existing.CommodityName = name.GetString()!;
        if (body.TryGetProperty("active", out var active))
        {
            existing.Active = active.GetBoolean();
            if (!existing.Active) existing.UseAtKiosk = false;
        }
        if (body.TryGetProperty("useAtKiosk", out var kiosk)) existing.UseAtKiosk = kiosk.GetBoolean();
        _db.SaveChanges();
        return Json(existing);
    }

    [HttpDelete("api/masterdata/commodities/{id:int}")]
    public IActionResult DeleteCommodity(int id)
    {
        var entity = _db.Commodities.Find(id);
        if (entity == null) return NotFound();
        _db.Commodities.Remove(entity);
        _db.SaveChanges();
        return Json(new { success = true });
    }
}
