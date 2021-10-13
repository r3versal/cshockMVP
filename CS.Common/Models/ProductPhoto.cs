using System;
namespace CS.Common.Models
{
    public class ProductPhoto
    {
        public Guid productPhotoId { get; set; }
        public int SortOrder { get; set; }
        public string PhotoURL { get; set; }
        public bool Featured { get; set; }
        public Guid productVariantId { get; set; }
        public string photoTitle { get; set; }
        public bool isSwatch { get; set; }
        public bool isActive { get; set; }
    }
}
