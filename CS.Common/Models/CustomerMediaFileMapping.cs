using System;
namespace CS.Common.Models
{
    public class CustomerMediaFileMapping
    {
        public Guid CustomerMediaFileMappingId { get; set; }
        public Guid CustomerId { get; set; }
        public string MediafileLink { get; set; }
        public int OrderShown { get; set; }
        public bool IsMain { get; set; }
    }
}
