using System;
using System.Collections.Generic;

namespace CS.Common.Models
{
    public class Order
    {
        public string email { get; set; }
        public bool isMemberCheckout { get; set; }
        public Guid OrderId { get; set; }
        public Guid ReviewId { get; set; }
        public string OrderNumber { get; set; }
        public Guid TransactionId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid BillingAddressId { get; set; }
        public Guid ShippingAddressId { get; set; }

        public string DeliveryTrackingId { get; set; }
        public string DeliveryCarrier { get; set; }
        public bool IsShipped { get; set; }
        public bool IsDelivered { get; set; }
        public bool Reviewed { get; set; }

        public string OrderStatus { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        
        public List<OrderItem> OrderItems {get; set;}
    }
}
