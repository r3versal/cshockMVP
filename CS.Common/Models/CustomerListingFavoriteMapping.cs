using System;
namespace CS.Common.Models
{
    public class CustomerListingFavoriteMapping
    {
        public Guid FavoriteMappingId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid FavoriteListingId { get; set; }
    }
}
