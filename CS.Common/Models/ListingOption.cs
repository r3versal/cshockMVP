using System;
namespace CS.Common.Models
{
    public class ListingOption
    {
        public Guid ListingOptionId { get; set; }
        public Guid ListingOptionGroupId { get; set; }
        public string OptionDesc { get; set; }
    }
}
