using System;
namespace CS.Common.Models
{
    public class Measurements
    {
        public Guid MeasurementsId { get; set; }
        public decimal bust { get; set; }
        public decimal waist { get; set; }
        public decimal hip { get; set; }
        public decimal centerLength { get; set; }
        public decimal fullLength { get; set; }
        public decimal shoulderSlope { get; set; }
        public decimal strap { get; set; }
        public decimal bustDepth { get; set; }
        public decimal bustSpan { get; set; }
        public decimal sideLength { get; set; }
        public decimal backNeck { get; set; }
        public decimal shoulderLength { get; set; }
        public decimal acrossShoulder { get; set; }
        public decimal acrossChest { get; set; }
        public decimal acrossBack { get; set; }
        public decimal bustArc { get; set; }
        public decimal backArc { get; set; }
        public decimal waistArc { get; set; }
        public decimal dartPlacement { get; set; }
        public decimal abdomenArc { get; set; }
        public decimal hipArc { get; set; }
        public decimal crotchDepth { get; set; }
        public decimal hipDepth { get; set; }
        public decimal sideHipDepth { get; set; }
        public decimal waistToAnkle { get; set; }
        public decimal crotchLength { get; set; }
        public decimal upperThigh { get; set; }
        public decimal knee { get; set; }
        public decimal calf { get; set; }
        public decimal ankle { get; set; }
        public decimal overarmLength { get; set; }
        public decimal underarmLength { get; set; }
        public decimal toElbow { get; set; }
        public decimal bicep { get; set; }
        public decimal elbow { get; set; }
        public decimal wrist { get; set; }
        public decimal aroundHand { get; set; }
        public decimal crotch { get; set; }
        public decimal inseam { get; set; }
        public decimal outseam { get; set; }
        public string email { get; set; }
        public Guid UserId { get; set; }
    }
}