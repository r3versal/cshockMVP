using System;
namespace CS.Common.Models
{
    public class CustomerStoreFavoriteMapping
    {
        public Guid FavoriteMappingId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid FavoriteStoreId { get; set; }
    }
}
