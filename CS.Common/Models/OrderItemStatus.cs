using System;
namespace CS.Common.Models
{
    public class OrderItemStatus
    {
        public bool NeedsAlteration { get; set; }
        public bool ReturnRequested { get; set; }
        public bool ReturnCompleted { get; set; }
    }
}
