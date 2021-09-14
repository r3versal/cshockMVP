using System;
namespace CS.Common.Models
{
    public class Customer
    {
        public Guid CustomerId { get; set; }
        public Guid MeasurementsId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public bool Active { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public string TimeZoneId { get; set; }
        public string InstagramHandle { get; set; }
        public Guid StripeUserID { get; set; }
        public bool IsMember { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
