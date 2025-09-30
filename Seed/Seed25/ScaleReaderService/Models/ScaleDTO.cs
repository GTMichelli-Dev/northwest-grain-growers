

    public class ScaleDTO
    {
      
    
        public string Description { get; set; }="Unknown";
        public bool Motion { get; set; } = false;
        public bool Ok { get; set; } = false;
        public  int Weight{ get; set; }
        public string Status { get; set; }="Connecting";
      
        public DateTime LastUpdate { get; set; }=DateTime.Now.AddDays(-100);



    }
