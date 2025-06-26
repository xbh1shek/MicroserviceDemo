

using Catalogue.Domain.Abstractions;

namespace Catalogue.Domain.Entities;

public record ProductBrand(string Name,long Id) : ValueObject;
