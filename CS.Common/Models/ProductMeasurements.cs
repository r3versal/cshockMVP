using System;
namespace CS.Common.Models
{
    public class ProductMeasurements
    {
        public Guid productMeasurementsId { get; set; }
        public bool bust { get; set; }
        public bool waist { get; set; }
        public bool hip { get; set; }
        public bool centerLength { get; set; }
        public bool fullLength { get; set; }
        public bool shoulderSlope { get; set; }
        public bool strap { get; set; }
        public bool bustDepth { get; set; }
        public bool bustSpan { get; set; }
        public bool sideLength { get; set; }
        public bool backNeck { get; set; }
        public bool shoulderLength { get; set; }
        public bool acrossShoulder { get; set; }
        public bool acrossChest { get; set; }
        public bool acrossBack { get; set; }
        public bool bustArc { get; set; }
        public bool backArc { get; set; }
        public bool waistArc { get; set; }
        public bool dartPlacement { get; set; }
        public bool abdomenArc { get; set; }
        public bool hipArc { get; set; }
        public bool crotchDepth { get; set; }
        public bool hipDepth { get; set; }
        public bool sideHipDepth { get; set; }
        public bool waistToAnkle { get; set; }
        public bool crotchLength { get; set; }
        public bool upperThigh { get; set; }
        public bool knee { get; set; }
        public bool calf { get; set; }
        public bool ankle { get; set; }
        public bool overarmLength { get; set; }
        public bool underarmLength { get; set; }
        public bool toElbow { get; set; }
        public bool bicep { get; set; }
        public bool elbow { get; set; }
        public bool wrist { get; set; }
        public bool aroundHand { get; set; }
        public bool crotch { get; set; }
        public bool inseam { get; set; }
        public bool outseam { get; set; }
        public Guid productId { get; set; }
        public Guid productVariantId { get; set; }
    }
}