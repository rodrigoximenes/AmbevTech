namespace Ambev.DeveloperEvaluation.Domain.Entities
{
    public class SaleItem
    {
        public Guid SaleId { get; set; }  
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }

        public void ApplyDiscount()
        {
            if (Quantity >= 4 && Quantity <= 9)
                Discount = 0.1m;
            else if (Quantity >= 10 && Quantity <= 20)
                Discount = 0.2m;
            else
                Discount = 0m;

            TotalAmount = (UnitPrice * Quantity) * (1 - Discount);
        }
    }
}
