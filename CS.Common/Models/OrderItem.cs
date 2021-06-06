using System;
using System.Collections.Generic;

namespace CS.Common.Models
{
    public class OrderItem
    {
        public Guid OrderItemId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime CreatedOn { get; set; }
        public Guid OrderId { get; set; }
        public Guid ListingId { get; set; }
        public bool IsStarted { get; set; }
        public bool IsCut { get; set; }
        public bool IsPatternMade { get; set; }
        public bool IsComplete { get; set; }
        public bool NeedsAlterations { get; set; }
        public int NumberOfAlterations { get; set; }
        public bool PrepaidAlterations { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitPriceInclTax { get; set; }
        public decimal TaxRate { get; set; }
        public Guid CategoryId { get; set; }
        public string Title { get; set; }
        public string StripeID { get; set; }
        public string StripePriceID { get; set; }
        public string description { get; set; }
        public string featuredImage { get; set; }
        public List<string> images { get; set; }
        public int count { get; set; }
    }
}
 