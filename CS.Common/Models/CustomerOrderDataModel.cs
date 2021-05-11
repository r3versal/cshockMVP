using System;
using System.Collections.Generic;

namespace CS.Common.Models
{
    public class CustomerOrderDataModel
    {
        public List<OrderItem> orderItems { get; set; }
        public Measurements measurements { get; set; }
    }
}
