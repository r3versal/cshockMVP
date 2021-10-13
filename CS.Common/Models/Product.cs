using System;
using System.Collections.Generic;

namespace CS.Common.Models
{
    public class Product
    {
        //COMMENT: ProductId is the same for variants of same listing
        public Guid ProductId { get; set; }
        //COMMENT: ProductVariantId is primary key in DB
        public Guid ProductVariantId { get; set; }
        public bool IsFeatured { get; set; }
        public bool Active { get; set; }
        public bool IsDiscount { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsAlteration { get; set; }
        public bool IsVirtualFitting { get; set; }
        public bool IsFitting { get; set; }
        public string StripePriceId { get; set; }
        public List<ProductPhoto> ProductPhotos {get;set;}
        public ProductMeasurements productMeasurements { get; set; }
        public Guid measurementsId { get; set; }
        public string ProductDescription { get; set; }
        public string productTitle { get; set; }
        public string ProductCareInstructions { get; set; }
        public string productDescriptionShort { get; set; }
        public string SKU { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
    }
}
