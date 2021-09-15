using System;
namespace CS.Common.Models
{
    public class PasswordResetRequest
    {
        public string token { get; set; }
        public Guid userId { get; set; }
        public string password { get; set; }
    }
}
