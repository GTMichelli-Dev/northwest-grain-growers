namespace GrainManagement.Models.DTO
{
    public class LocationTotalsDTO
    {
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public long IntakeBushels {
            get   {
                return IntakeLbs / 60;
            }
        }
        public long IntakeLbs { get; set; }
        public long OutboundLbs { get; set; }

        public long OutboundBushels { get {
                return OutboundLbs / 60;
            }
        }
        public long TransferedToLbs { get; set; }
        public long TransferedToBushels
        {
            get
            {
                return TransferedToLbs / 60;
            }
        }
        public long TransferedFromLbs { get; set; }

        public long TransferedFromBushels
        {
            get
            {
                return TransferedFromLbs / 60;
            }
        }

    }
}
