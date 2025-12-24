
namespace ZooOnlineStoreApi.Models
{
    public class PaymentFilter
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? Status { get; set; }
        public string? SortBy { get; set; }
        public int? UserId { get; set; }
        public bool SortDescending { get; set; } = true;
    }
    public class PaymentPaged
    {
        public List<Payment> Items { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;
    }
}
