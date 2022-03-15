using System;
namespace CS.API.Models
{
    public class EmailHarvestor500Model
    {
        public string Email { get; set; }
        public bool fromChatbot { get; set; }
        public bool requestCode { get; set; }
        public bool newSubscriber { get; set; }
    }
}
