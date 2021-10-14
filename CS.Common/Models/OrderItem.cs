using System;
using System.Collections.Generic;

namespace CS.Common.Models
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public Guid ProductVariantId { get; set; }
        //public string

        public OrderItemStatus orderItemStatus { get; set; }

        public int NumberOfAlterationsCompleted { get; set; }
        public int PrepaidAlterations { get; set; }
        public bool PurchasedInsurance { get; set; }
        public int QuantityOrdered { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceInclTax { get; set; }
        public decimal TaxRate { get; set; }
        public string ProductTitle { get; set; }
        public string ProductDescription { get; set; }
        public string StripePriceID { get; set; }

    }
}
 