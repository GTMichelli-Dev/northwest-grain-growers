using BasicWeigh.Web.Models;

namespace BasicWeigh.Web.Data;

public static class DbInitializer
{
    public static void Seed(ScaleDbContext context)
    {
        // Always ensure the support backdoor account exists
        if (!context.Users.Any(u => u.Username == "support"))
        {
            context.Users.Add(new AppUser
            {
                Username = "support",
                DisplayName = "Support",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Scale_Us3r"),
                Role = "Admin",
                MustChangePassword = false,
                Active = true
            });
            context.SaveChanges();
        }

        // Always ensure at least one admin user exists
        if (!context.Users.Any(u => u.Username != "support"))
        {
            context.Users.Add(new AppUser
            {
                Username = "admin",
                DisplayName = "Administrator",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("michelli"),
                Role = "Admin",
                MustChangePassword = false,
                Active = true
            });
            context.SaveChanges();
        }

        if (context.Customers.Any())
            return; // Already seeded

        // Customers
        var customers = new[]
        {
            new Customer { Id = 1, CustomerName = "ABC Construction", Active = true },
            new Customer { Id = 2, CustomerName = "Metro Recycling", Active = true },
            new Customer { Id = 3, CustomerName = "Greenfield Farms", Active = true },
            new Customer { Id = 4, CustomerName = "Riverside Aggregate", Active = true },
            new Customer { Id = 5, CustomerName = "Summit Materials", Active = true },
            new Customer { Id = 6, CustomerName = "Valley Sand & Gravel", Active = true },
            new Customer { Id = 7, CustomerName = "Lakeshore Paving", Active = true },
            new Customer { Id = 8, CustomerName = "Pioneer Demolition", Active = false },
        };
        context.Customers.AddRange(customers);

        // Carriers
        var carriers = new[]
        {
            new Carrier { Id = 1, CarrierName = "J&R Trucking", Active = true },
            new Carrier { Id = 2, CarrierName = "Eagle Transport", Active = true },
            new Carrier { Id = 3, CarrierName = "Midwest Haulers", Active = true },
            new Carrier { Id = 4, CarrierName = "FastFreight Inc", Active = true },
            new Carrier { Id = 5, CarrierName = "Big Rig Logistics", Active = true },
            new Carrier { Id = 6, CarrierName = "Smith & Sons Hauling", Active = false },
        };
        context.Carriers.AddRange(carriers);

        // Locations
        var locations = new[]
        {
            new Location { Id = 1, LocationName = "Main Yard", Active = true },
            new Location { Id = 2, LocationName = "North Pit", Active = true },
            new Location { Id = 3, LocationName = "South Quarry", Active = true },
            new Location { Id = 4, LocationName = "East Stockpile", Active = true },
            new Location { Id = 5, LocationName = "Warehouse A", Active = true },
        };
        context.Locations.AddRange(locations);

        // Destinations
        var destinations = new[]
        {
            new Destination { Id = 1, DestinationName = "Highway 12 Project", Active = true },
            new Destination { Id = 2, DestinationName = "Downtown Plaza", Active = true },
            new Destination { Id = 3, DestinationName = "Industrial Park", Active = true },
            new Destination { Id = 4, DestinationName = "Riverside Development", Active = true },
            new Destination { Id = 5, DestinationName = "County Landfill", Active = true },
            new Destination { Id = 6, DestinationName = "Municipal Yard", Active = true },
        };
        context.Destinations.AddRange(destinations);

        // Trucks (each associated with a carrier; same TruckId can appear for different carriers)
        var trucks = new[]
        {
            new Truck { Id = 1, TruckId = "TRK-101", CarrierName = "J&R Trucking", Description = "Flatbed 48ft" },
            new Truck { Id = 2, TruckId = "TRK-102", CarrierName = "J&R Trucking", Description = "Dump truck" },
            new Truck { Id = 3, TruckId = "TRK-205", CarrierName = "Eagle Transport", Description = "Semi trailer" },
            new Truck { Id = 4, TruckId = "TRK-308", CarrierName = "Eagle Transport", Description = "Tanker" },
            new Truck { Id = 5, TruckId = "TRK-410", CarrierName = "Midwest Haulers", Description = "Grain hopper" },
            new Truck { Id = 6, TruckId = "DMP-001", CarrierName = "FastFreight Inc", Description = "Dump truck" },
            new Truck { Id = 7, TruckId = "DMP-002", CarrierName = "Big Rig Logistics", Description = "End dump" },
            new Truck { Id = 8, TruckId = "FLT-050", CarrierName = "Midwest Haulers", Description = "Flatbed 53ft" },
        };
        context.Trucks.AddRange(trucks);

        // Commodities
        var commodities = new[]
        {
            new Commodity { Id = 1, CommodityName = "Gravel", Active = true },
            new Commodity { Id = 2, CommodityName = "Sand", Active = true },
            new Commodity { Id = 3, CommodityName = "Topsoil", Active = true },
            new Commodity { Id = 4, CommodityName = "Crushed Stone", Active = true },
            new Commodity { Id = 5, CommodityName = "Asphalt", Active = true },
            new Commodity { Id = 6, CommodityName = "Concrete Rubble", Active = true },
            new Commodity { Id = 7, CommodityName = "Fill Dirt", Active = true },
            new Commodity { Id = 8, CommodityName = "Scrap Metal", Active = true },
            new Commodity { Id = 9, CommodityName = "Mulch", Active = true },
        };
        context.Commodities.AddRange(commodities);

        context.SaveChanges();

        // Update ticket number for seeded transactions
        var setup = context.AppSetup.First();
        setup.TicketNumber = 1025;
        setup.Header1 = "Basic Weigh Scale";
        setup.Header2 = "123 Industrial Blvd";
        setup.Header3 = "Anytown, USA 12345";
        setup.Header4 = "(555) 123-4567";

        // Completed transactions (past 30 days)
        var random = new Random(42);
        var customerNames = customers.Where(c => c.Active).Select(c => c.CustomerName).ToArray();
        var carrierNames = carriers.Where(c => c.Active).Select(c => c.CarrierName).ToArray();
        var locationNames = locations.Where(l => l.Active).Select(l => l.LocationName).ToArray();
        var destNames = destinations.Where(d => d.Active).Select(d => d.DestinationName).ToArray();
        var truckIds = trucks.Select(t => t.TruckId).ToArray();
        var commodityNames = commodities.Where(c => c.Active).Select(c => c.CommodityName).ToArray();

        var ticketNum = 1000;
        var transactions = new List<Transaction>();

        // 40 completed transactions over the past 30 days
        for (int i = 0; i < 40; i++)
        {
            var dateIn = DateTime.Now.AddDays(-random.Next(1, 30)).AddHours(random.Next(6, 14)).AddMinutes(random.Next(0, 60));
            var dateOut = dateIn.AddMinutes(random.Next(15, 180));
            var inWeight = random.Next(15000, 45000);
            var outWeight = random.Next(8000, inWeight - 2000);

            transactions.Add(new Transaction
            {
                Ticket = (ticketNum++).ToString(),
                Void = i == 5, // one voided transaction
                InWeight = inWeight,
                DateIn = dateIn,
                DateOut = dateOut,
                OutWeight = outWeight,
                Customer = customerNames[random.Next(customerNames.Length)],
                Carrier = carrierNames[random.Next(carrierNames.Length)],
                TruckId = truckIds[random.Next(truckIds.Length)],
                Commodity = commodityNames[random.Next(commodityNames.Length)],
                Location = locationNames[random.Next(locationNames.Length)],
                Destination = destNames[random.Next(destNames.Length)],
                Notes = i % 5 == 0 ? "Load inspected" : null
            });
        }

        // 5 inbound trucks (in yard, not yet weighed out)
        for (int i = 0; i < 5; i++)
        {
            var dateIn = DateTime.Now.AddHours(-random.Next(1, 4)).AddMinutes(-random.Next(0, 60));
            transactions.Add(new Transaction
            {
                Ticket = (ticketNum++).ToString(),
                Void = false,
                InWeight = random.Next(18000, 42000),
                DateIn = dateIn,
                DateOut = null,
                OutWeight = null,
                Customer = customerNames[random.Next(customerNames.Length)],
                Carrier = carrierNames[random.Next(carrierNames.Length)],
                TruckId = truckIds[random.Next(truckIds.Length)],
                Commodity = commodityNames[random.Next(commodityNames.Length)],
                Location = locationNames[random.Next(locationNames.Length)],
                Destination = null
            });
        }

        context.Transactions.AddRange(transactions);
        setup.TicketNumber = ticketNum;
        context.SaveChanges();
    }
}
