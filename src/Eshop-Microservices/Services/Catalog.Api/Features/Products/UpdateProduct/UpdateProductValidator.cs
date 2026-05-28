using Catalog.Api.Features.Products.CreateProduct;
using FluentValidation;

namespace Catalog.Api.Features.Products.UpdateProduct;
public class UpdateProductValidator: AbstractValidator<UpdateProductCommand>
{
    public UpdateProductValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Product id is required");

        RuleFor(p => p.Name)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("product name is required")
            .Length(2, 150).WithMessage("Product name length should be between 2 and 150 characters");

        RuleFor(p => p.Description)
            .MaximumLength(2000).WithMessage("Product description must not exceed 2000 characters");

        RuleFor(p => p.ImageFile)
            .NotEmpty().WithMessage("Image file is required");

        RuleFor(p => p.Price)
            .GreaterThan(0).WithMessage("Product price must be greater than zero");

        RuleFor(p => p.Categories)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage("Categories must not be empty")
            .Must(categories => categories.All(category => !string.IsNullOrWhiteSpace(category)))
            .WithMessage("Category names must not be empty");
    }
}
