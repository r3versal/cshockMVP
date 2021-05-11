using System;
namespace CS.Common.Models
{
    public class Address
    {
        public Guid AddressId { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CompanyId { get; set; }
        public bool IsBilling { get; set; }
        public bool IsShipping { get; set; }
        public bool IsDefault { get; set; }
        public string Salutation { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string CountryCode { get; set; }
        public string Zipcode { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
    }
}
