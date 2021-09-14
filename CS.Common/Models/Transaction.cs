using System;
namespace CS.Common.Models
{
    public class Transaction
    {
        public Guid TransactionId { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

        public string TransactionType { get; set; }
        public string StripeTransactionId { get; set; }
        public bool Discounted { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }
        public decimal PreTaxTotal { get; set; }
        public decimal AppliedDiscountAmount { get; set; }
    }
}
