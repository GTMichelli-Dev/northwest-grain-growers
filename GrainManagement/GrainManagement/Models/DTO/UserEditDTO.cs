namespace GrainManagement.Models.DTO
{
    public class UserEditDto
    {
        public long Id { get; set; }
        public int UserPin { get; set; }
        public string Name { get; set; }   
        public List<int> PrivilegeIds { get; set; } = new();
        public string PrivilegeNames { get; set; } // for display only (comma separated)
    }
}
