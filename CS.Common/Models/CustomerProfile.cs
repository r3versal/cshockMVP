using System;
using System.Collections.Generic;

namespace CS.Common.Models
{
    public class CustomerProfile
    {
        public Guid CustomerProfileId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public string Gender { get; set; }
        public string InstagramHandle { get; set; }
        public DateTime createdOn { get; set; }
        public DateTime updatedOn { get; set; }
    }
}