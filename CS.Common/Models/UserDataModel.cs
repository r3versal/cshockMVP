using System;

namespace CS.Common.Models
{
    public class UserDataModel
    {
        public Guid UserId { get; set; }
        public int AccessFailedCount { get; set; }
        public string Email { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public bool Active { get; set; }
        public bool IsSystemAccount { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOnUtc { get; set; }
        public string UpdatedBy { get; set; }
        public string AdminComment { get; set; }
        public string SystemName { get; set; }
        public string LastIpAddress { get; set; }
    }
}
