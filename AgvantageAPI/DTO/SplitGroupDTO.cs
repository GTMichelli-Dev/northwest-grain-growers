namespace AgvantageAPI.DTO
{
    public class SplitGroupDTO
    {
        public int Id { get; set; }

        public long SplitGroupNumber { get; set; }
        public string Description { get; set; }=string.Empty;
        public long PrimaryAccountId { get; set; }
    }
}
