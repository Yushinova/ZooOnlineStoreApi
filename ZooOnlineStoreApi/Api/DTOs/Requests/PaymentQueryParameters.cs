using System.ComponentModel.DataAnnotations;

namespace ZooOnlineStoreApi.Api.DTOs.Requests
{
    public class PaymentQueryParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Status { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; } = true;
    }
}
