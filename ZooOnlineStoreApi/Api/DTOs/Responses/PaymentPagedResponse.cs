using ZooOnlineStoreApi.Model.Payments;

namespace ZooOnlineStoreApi.Api.DTOs.Responses
{
    public class PaymentPagedResponse
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
