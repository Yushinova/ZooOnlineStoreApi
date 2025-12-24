
namespace ZooOnlineStoreApi.Services.DTOs.Requests
{
    public class PaymentRequestParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Status { get; set; }
        public string? SortBy { get; set; }
        public int? UserId { get; set; }
        public bool SortDescending { get; set; } = true;
    }
}
