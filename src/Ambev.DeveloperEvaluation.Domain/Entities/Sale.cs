namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class Sale
    {
        public Guid Id { get; set; }
        public string SaleNumber { get; set; }
        public DateTime SaleDate { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsCancelled { get; set; }
        public decimal Discount { get; set; }
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public Guid BranchId { get; set; }
        public string BranchName { get; set; }
        public List<SaleItem> Items { get; set; } = new();

        public decimal CalculateTotalAmount()
        {
            decimal itemsTotal = Items.Sum(item => item.Quantity * item.UnitPrice * (1 - item.Discount));
            decimal discountAmount = itemsTotal * Discount; 

            TotalAmount = itemsTotal - discountAmount;
            return TotalAmount;
        }
    }
}
