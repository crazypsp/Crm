using System.ComponentModel.DataAnnotations;

namespace Crm.Api.Admin.Models.Requests
{
    public class AssignRoleRequest
    {
        [Required(ErrorMessage = "Rol adı zorunludur")]
        public string RoleName { get; set; } = string.Empty;
    }
}