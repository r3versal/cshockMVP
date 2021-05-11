using System;
namespace CS.Common.Models
{
    public class TOTP
    {
        public Guid TOTPId { get; set; }
        public Guid UserId { get; set; }
        public string SessionId { get; set; }
        public string DeviceId { get; set; }
        public bool IsMobile { get; set; }
        public bool IsEmail { get; set; }
        public DateTime CreatedOn { get; set; }
        public string TwoFAString { get; set;}
    }
}
