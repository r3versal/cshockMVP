using System;
namespace CS.Common.Models
{
    public class ProductMeasurements
    {
        public Guid productMeasurementsId { get; set; }
        public bool bust { get; set; }
        public bool waist { get; set; }
        public bool hips { get; set; }
        public bool neck { get; set; }
        public bool shoulder { get; set; }
        public bool backWidth { get; set; }
        public bool frontLength { get; set; }
        public bool backLength { get; set; }
        public bool crotch { get; set; }
        public bool inseam { get; set; }
        public bool outseam { get; set; }
        public bool thigh { get; set; }
        public bool knee { get; set; }
        public bool leg { get; set; }
        public Guid productId { get; set; }
        public Guid productVariantId { get; set; }
    }
}