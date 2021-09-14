using System;
using System.Collections.Generic;

namespace CS.Common.Models
{
    public class CustomerOrderDataModel
    {
        public List<OrderItem> orderItems { get; set; }
        public CustomerOrder customerOrder { get; set; }
        public Order order { get; set; }
        public Measurements measurements { get; set; }
        public CustomerAddress customerAddress { get; set; }
        public Guid userId { get; set; }
    }
}
