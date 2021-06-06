using System;
using System.Collections.Generic;

namespace CS.Common.Models
{
    public class CustomerOrder
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string OrderNumber { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BillingAddressId { get; set; }
        public Guid ShippingAddressId { get; set; }
        public bool IsShipped { get; set; }
        public bool IsDelivered { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal Total { get; set; }
        public decimal TotalExclTax { get; set; }
        public bool ReturnRequested { get; set; }
        public string CustomerComment { get; set; }
        public string PONumber { get; set; }
        public string StoreComment { get; set; }
        public DateTime PaidOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Guid StripeUserId { get; set; }
        public bool Active { get; set; }
        public bool IsFeatured { get; set; }
        public Guid ReviewId { get; set; }
        public string DeliveryTrackingId { get; set; }
        public List<OrderItem> orderItems { get; set; }
    }
}
