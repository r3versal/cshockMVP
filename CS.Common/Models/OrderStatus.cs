using System;
namespace CS.Common.Models
{
    public class OrderStatus
    {
        public bool OrderRecieved { get; set; }
        public bool Started { get; set; }
        public bool CustomPatternComplete { get; set; }
        public bool Cut { get; set; }
        public bool Sewn { get; set; }
        public bool Shipped { get; set; }
        public bool CustomerRecieved { get; set; }
    }

}
