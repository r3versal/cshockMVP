using System;
namespace CS.Common.Models
{
    public class ListingCategory
    {
        public Guid ListingCategoryId { get; set; }
        public Guid StoreId { get; set; }
        public string CategoryDesc { get; set; }
    }
}
