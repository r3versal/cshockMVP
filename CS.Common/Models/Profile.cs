using System;
namespace CS.Common.Models
{
    public class Profile
    {
        public Guid ProfileId { get; set; }
        public Guid UserId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthdate { get; set; }
        public bool IsStore { get; set; }
        public string Gender { get; set; }
        public string TimeZoneId { get; set; }
        public string InstagramHandle { get; set; }
        public string ImageUrl { get; set; } 
        public string Profile_MeasurementsId { get; set; }
    }
}
