namespace GrainManagement
{
    public class UserEditDto
    {
        public Guid Uid { get; set; }
        public int UserPin { get; set; }
        public string Name { get; set; }   
        public List<int> PrivilegeIds { get; set; } = new();
        public string PrivilegeNames { get; set; } // for display only (comma separated)
    }
}
