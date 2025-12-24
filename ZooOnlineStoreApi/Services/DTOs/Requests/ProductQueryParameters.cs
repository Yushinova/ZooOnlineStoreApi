
namespace ZooOnlineStoreApi.Services.DTOs.Requests
{
    public class ProductQueryParameters
    {
        //пагинация
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        //фильтрация
        public bool? IsActive { get; set; }
        public bool? IsPromotion { get; set; }
        public int? CategoryId { get; set; }
        public int? PetTypeId { get; set; }

        //сортировка
        public string? Name { get; set; }
        public double? Rating { get; set; }
        public string? Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
