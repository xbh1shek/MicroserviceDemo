
using Catalogue.Domain.Abstractions;

namespace Catalogue.Domain.Entities;

public sealed class Product : AggregateRoot
{
    public string Name { get; }
    public string Summary { get; }
    public string Description { get; }
    public ProductBrand Brands { get; }
    public ProductType Types { get; }
    public Price Price { get; }

    private List<ProductImage> _productImages = new List<ProductImage>();
    public IReadOnlyCollection<ProductImage> ProductImages => _productImages.AsReadOnly();

    private Product() { }

    public Product(string name, string summary, string description, ProductBrand brands, ProductType types, Price price)
    {
        Name = name.NotNullOrWhiteSpace(nameof(name));
        Summary = summary;
        Description = description;
        Brands = brands;
        Types = types;
        Price = price.NotNull(nameof(price));
    }

    public void AddProductImage(ProductImage productImage)
    {
        if (_productImages.Any(x => x.ImageUrl.Equals(productImage.ImageUrl, StringComparison.OrdinalIgnoreCase)))
            throw new CatalogueException($"Image already exists");
        _productImages.Add(productImage);
    }

    public void RemoveProductImage(long productImageId)
    {
        var img = _productImages.FirstOrDefault(x => x.Id == productImageId);
        if (img is null)
            throw new CatalogueException("Image not found");
        _productImages.Remove(img);
    }

    public void UpdateProductImage(long productImageId, string imageUrl)
    {
        var img = _productImages.FirstOrDefault(x => x.Id == productImageId);
        if (img is null)
            throw new CatalogueException("Image not found");

        img.SetImageUrl(imageUrl);
    }
}