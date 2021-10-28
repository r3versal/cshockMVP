using System;
namespace CS.Common.Models
{
    public class Fitting
    {
        public Guid fittingId { get; set; }
        public Guid userId { get; set; }
        public Guid orderId { get; set; }
        public bool isVirtual { get; set; }
        public DateTime dateRequested { get; set; }
        public string timeRequested { get; set; }
        public string timeZone { get; set; }
        public DateTime createdOn { get; set; }
    }
}
