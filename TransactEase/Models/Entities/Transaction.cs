using System;

namespace TransactEase.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ReceiverId { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CashbackId { get; set; }
        public int OrganizationId { get; set; }
        public decimal CashbackAmount { get; set; }
    }
}
