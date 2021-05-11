using System;
namespace CS.Common.Models
{
    public class StoreMediaFileMapping
    {
        public Guid StoreMediaFileMappingId { get; set; }
        public Guid StoreId { get; set; }
        public string MediafileLink { get; set; }
        public int OrderShown { get; set; }
        public bool isMain { get; set; }
    }
}
