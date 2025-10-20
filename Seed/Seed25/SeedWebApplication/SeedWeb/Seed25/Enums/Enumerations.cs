namespace Seed25
{
    public class Enumerations
    {
        public enum TicketStatus
        {
            Incomplete,
            Finished,
            Pending,
            Voided
        }

        public enum TicketType
        {
            Unknown,
            Bag,
            ReturnBag,
            ReturnBulk,
            ReturnTote,
            Tote,
            Truck
        }
    }
}
