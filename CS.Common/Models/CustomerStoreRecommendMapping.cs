using System;
namespace CS.Common.Models
{
    public class CustomerStoreRecommendMapping
    {
        public Guid FavoriteMappingId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid RecommendStoreId { get; set; }
    }
}
