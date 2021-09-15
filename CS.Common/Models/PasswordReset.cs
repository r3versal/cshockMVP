using System;
namespace CS.Common.Models
{
    public class PasswordReset
    {
        public Guid PasswordResetId { get; set; }
        public Guid UserId { get; set; }
        public string newHash { get; set; }
        public DateTime expire { get; set; }
    }
}
