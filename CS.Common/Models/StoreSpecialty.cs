using System;
namespace CS.Common.Models
{
    public class StoreSpecialty
    {
        public Guid StoreSpecialtyId { get; set; }
        public Guid StoreId { get; set; }
        public string SpecialtyDesc { get; set; }
    }
}
