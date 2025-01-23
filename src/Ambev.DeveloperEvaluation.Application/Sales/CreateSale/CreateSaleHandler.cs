using Ambev.DeveloperEvaluation.Domain.Entities;
using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleHandler : IRequestHandler<CreateSaleCommand, Guid>
    {
        private readonly ISaleRepository _saleRepository;
        private readonly ICustomerService _customerService;
        private readonly IBranchService _branchService;
        private readonly IProductService _productService;

        public CreateSaleHandler(
            ISaleRepository saleRepository,
            ICustomerService customerService,
            IBranchService branchService,
            IProductService productService)
        {
            _saleRepository = saleRepository;
            _customerService = customerService;
            _branchService = branchService;
            _productService = productService;
        }

        public async Task<Guid> Handle(CreateSaleCommand command, CancellationToken cancellationToken)
        {
            var customer = await _customerService.GetCustomerByIdAsync(command.CustomerId);
            var branch = await _branchService.GetBranchByIdAsync(command.BranchId);

            if (customer == null || branch == null)
                throw new InvalidOperationException("Customer or Branch not found");

            var sale = new Sale
            {
                Id = Guid.NewGuid(),
                SaleNumber = command.SaleNumber,
                SaleDate = command.SaleDate,
                CustomerId = customer.Id,
                CustomerName = customer.Name, 
                BranchId = branch.Id,
                BranchName = branch.Name, 
            };

            foreach (var item in command.Items)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product == null)
                    throw new InvalidOperationException($"Product with ID {item.ProductId} not found");

                if (item.Quantity > 20)
                    throw new InvalidOperationException("Cannot sell more than 20 identical items");

                var saleItem = new SaleItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name, 
                    UnitPrice = product.Price, 
                    Quantity = item.Quantity
                };
                saleItem.ApplyDiscount();
                sale.Items.Add(saleItem);
            }

            sale.TotalAmount = sale.Items.Sum(i => i.TotalAmount);
            await _saleRepository.CreateAsync(sale, cancellationToken);

            return sale.Id;
        }
    }
}
