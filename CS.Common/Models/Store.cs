using System;
namespace CS.Common.Models
{
    public class Store
    {
        public Guid StoreId { get; set; }
        public Guid UserId { get; set; }
        public string StoreName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public bool IsFeatured { get; set; }
        public string InstagramHandle { get; set; }
        public string YouTubePage { get; set; }
        public string FacebookPage { get; set; }
        public string TikTokHandle { get; set; }
        public Guid StripeUserId { get; set; }
    }
}
