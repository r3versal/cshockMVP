using System;
namespace CS.Common.Models
{
    public class ListingMediaFileMapping
    {
        public Guid ListingMediaFileMappingId { get; set; }
        public Guid ListingId { get; set; }
        public string MediafileLink { get; set; }
        public int OrderShown { get; set; }
        public bool IsMain { get; set; }
    }
}
