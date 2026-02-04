namespace GrainManagement.Models.DTO
{
    public class UserEditDto
    {
        public long UserId { get; set; }
        public int Pin { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }   
        public List<int> PrivilegeIds { get; set; } = new();
        public string PrivilegeNames { get; set; } // for display only (comma separated)
    }
}
