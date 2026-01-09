namespace Crm.Api.Admin.Models.Responses
{
    public class RoleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsSystemRole { get; set; }
        public int UserCount { get; set; }
        public List<string> Permissions { get; set; } = new();
    }
}