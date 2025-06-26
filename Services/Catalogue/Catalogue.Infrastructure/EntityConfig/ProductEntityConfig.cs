using Catalogue.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalogue.Infrastructure.EntityConfig
{
    public  class ProductEntityConfig : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> entityTypeBuilder)
        {
            entityTypeBuilder.ToTable("Catalogues");
            entityTypeBuilder.ComplexProperty(m => m.Price, v =>
            {
                v.Property(p => p.Currency).HasColumnName("Currency");
                v.Property(p => p.Amount).HasColumnName("Amount");
            });
            entityTypeBuilder.ComplexProperty(m => m.Brands, v =>
            {
                v.Property(p => p.Name ).HasColumnName("BrandName");
                v.Property(p => p.Id).HasColumnName("BrandId");
            });

            entityTypeBuilder.ComplexProperty(m => m.Types, v =>
            {
                v.Property(p => p.Name).HasColumnName("TypeName");
                v.Property(p => p.Id).HasColumnName("TypeId");
            });

            entityTypeBuilder.OwnsMany(
                p => p.ProductImages, pi =>
                {
                    pi.Ignore(cc => cc.DomainEvents);
                    pi.ToTable("productImages");
                    pi.WithOwner().HasForeignKey("ProductId");
                    pi.HasKey("Id");
                  
                });
        }
    }
}
