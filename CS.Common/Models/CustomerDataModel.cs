using System;
namespace CS.Common.Models
{
    public class CustomerDataModel
    {
        public Customer Customer { get; set; }
        public Address Address { get; set; }
        public Measurements Measurements { get; set; }
    }
}
