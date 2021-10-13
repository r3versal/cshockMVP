using System;
using System.Collections.Generic;

namespace CS.Common.Models
{
    public class Profile
    {
        public CustomerProfile customerProfile { get; set; }
        public Measurements profileMeasurements { get; set; }
        public List<Order> orderHistory { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
    }
}
