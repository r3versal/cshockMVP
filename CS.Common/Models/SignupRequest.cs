using System;

namespace CS.Common.Models
{
    public class SignupRequest
    {
        public string DeviceToken { get; set; }
        public string DeviceOS { get; set; }
        public string AppCode { get; set; }
        public string AppVersion { get; set; }
        public string Password { get; set; }

        public User User { get; set; }
    }
}
