using System;
namespace CS.Common.Models
{
    public class Review
    {
        public Guid ReviewId { get; set; }
        public Guid OrderId { get; set; }
        public string ReviewDesc { get; set; }
        public int StarRating { get; set; }
    }
}
