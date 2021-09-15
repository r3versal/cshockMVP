using System;
namespace CS.Common.Models
{
    public class Device
    {
        public Guid UserId { get; set; }
        public string RefreshToken { get; set; }
        public double Timestamp { get; set; }
    }
}
