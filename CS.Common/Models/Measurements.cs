using System;
namespace CS.Common.Models
{
    public class Measurements
    {
        public Guid MeasurementsId { get; set; }
        public decimal bust { get; set; }
        public decimal waist { get; set; }
        public decimal hips { get; set; }
        public decimal neck { get; set; }
        public decimal shoulder { get; set; }
        public decimal backWidth { get; set; }
        public decimal frontLength { get; set; }
        public decimal backLength { get; set; }
        public decimal crotch { get; set; }
        public decimal inseam { get; set; }
        public decimal outseam { get; set; }
        public decimal thigh { get; set; }
        public decimal knee { get; set; }
        public decimal leg { get; set; }
        public string email { get; set; }
        public Guid UserId { get; set; }
    }
}