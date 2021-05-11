using System;
namespace CS.Common.Models
{
    public class Listing
    {
        public Guid ListingId { get; set; }
        public Guid StoreId { get; set; }
        public string ListingName { get; set; }
        public string ListingDesc { get; set; }
        public bool IsFeatured { get; set; }
        public bool Active { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAlteration { get; set; }
        public bool IsCustomReqListing { get; set; }
    }
}
