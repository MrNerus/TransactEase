using System;

namespace TransactEase.Models.Entities
{
    public class Card
    {
        public string CardNumber { get; set; } = string.Empty;
        public string CardType { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public int OrganizationId { get; set; }
        public int UserId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Cvv { get; set; } = string.Empty;
    }
}
