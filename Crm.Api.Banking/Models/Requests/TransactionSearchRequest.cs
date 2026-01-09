using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Crm.Api.Banking.Models.Requests
{
    public class TransactionSearchRequest
    {
        [FromQuery(Name = "search")]
        public string? Search { get; set; }

        [FromQuery(Name = "importId")]
        public Guid? ImportId { get; set; }

        [FromQuery(Name = "startDate")]
        public DateTime? StartDate { get; set; }

        [FromQuery(Name = "endDate")]
        public DateTime? EndDate { get; set; }

        [FromQuery(Name = "mappingStatus")]
        public string? MappingStatus { get; set; }

        [FromQuery(Name = "page")]
        [Range(1, 1000, ErrorMessage = "Sayfa numarası 1-1000 arasında olmalıdır")]
        public int Page { get; set; } = 1;

        [FromQuery(Name = "pageSize")]
        [Range(1, 200, ErrorMessage = "Sayfa boyutu 1-200 arasında olmalıdır")]
        public int PageSize { get; set; } = 50;
    }
}