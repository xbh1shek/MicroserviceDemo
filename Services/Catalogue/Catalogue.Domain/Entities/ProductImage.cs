using Catalogue.Domain.Abstractions;

namespace Catalogue.Domain.Entities
{
    public sealed class ProductImage : Entity
    {
        public string ImageUrl { get; private set; } = string.Empty;
        private ProductImage() { }
        public ProductImage(string imageUrl)
        {
            ImageUrl = imageUrl.NotNullOrWhiteSpace(nameof(imageUrl));
        }

        internal void SetImageUrl(string imageUrl)
        {
            ImageUrl=imageUrl;
        }
    }
}
