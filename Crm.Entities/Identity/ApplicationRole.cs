using Crm.Entities.Common;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Crm.Entities.Identity
{
    public class ApplicationRole : IdentityRole<Guid>
    {
        [MaxLength(EntityConstants.MediumTextMax)]
        public string? Description { get; set; }
    }
}
