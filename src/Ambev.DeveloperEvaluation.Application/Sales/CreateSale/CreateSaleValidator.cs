using Ambev.DeveloperEvaluation.Domain.Entities;
using FluentValidation;

namespace Ambev.DeveloperEvaluation.Application.Sales.CreateSale
{
    public class CreateSaleCommandValidator : AbstractValidator<CreateSaleCommand>
    {
        public CreateSaleCommandValidator()
        {
            RuleFor(sale => sale.SaleNumber)
                .NotEmpty().WithMessage("Sale number is required.")
                .Length(1, 50);

            RuleFor(sale => sale.SaleDate)
                .NotNull().WithMessage("Sale date is required.")
                .LessThanOrEqualTo(DateTime.Now).WithMessage("Sale date must be in the past or present.");

            RuleFor(sale => sale.Items)
                .NotEmpty().WithMessage("Sale must have at least one item.");

            RuleForEach(sale => sale.Items).SetValidator(new CreateSaleItemValidator());
        }
    }

    public class CreateSaleItemValidator : AbstractValidator<CreateSaleItemRequest>
    {
        public CreateSaleItemValidator()
        {
            RuleFor(item => item.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");

            RuleFor(item => item.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be between 1 and 20.")
                .LessThanOrEqualTo(20);

            RuleFor(item => item.UnitPrice)
                .GreaterThan(0).WithMessage("Unit Price must be greater than 0.");

            RuleFor(item => item.Discount)
                .GreaterThanOrEqualTo(0).WithMessage("Discount cannot be negative.")
                .LessThanOrEqualTo(1).WithMessage("Discount cannot be more than 100%.");

            RuleFor(item => item).Custom((item, context) =>
            {
                // Chamando o método de validação de domínio
                var saleItem = new SaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Discount = item.Discount
                };

                saleItem.ApplyDiscount();

                if (saleItem.Discount != item.Discount)
                {
                    context.AddFailure("Discount", "Invalid discount for the quantity.");
                }
            });
        }
    }
}
