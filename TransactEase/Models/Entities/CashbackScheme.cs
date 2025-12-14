using System;

namespace TransactEase.Models.Entities
{
    public class CashbackScheme
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public decimal Rate { get; set; }
    }
}
