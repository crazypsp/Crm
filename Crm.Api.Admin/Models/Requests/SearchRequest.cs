using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Crm.Api.Admin.Models.Requests
{
    public class SearchRequest
    {
        [FromQuery(Name = "search")]
        public string? Search { get; set; }

        [FromQuery(Name = "page")]
        [Range(1, 1000, ErrorMessage = "Sayfa numarası 1-1000 arasında olmalıdır")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        [Range(1, 200, ErrorMessage = "Sayfa boyutu 1-200 arasında olmalıdır")]
        public int PageSize { get; set; } = 50;

        [FromQuery(Name = "sortBy")]
        public string? SortBy { get; set; }

        [FromQuery(Name = "sortDirection")]
        public string SortDirection { get; set; } = "asc";

        [FromQuery(Name = "isActive")]
        public bool? IsActive { get; set; }

        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }
    }
}