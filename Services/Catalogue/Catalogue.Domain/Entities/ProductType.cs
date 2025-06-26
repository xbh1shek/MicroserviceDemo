using Catalogue.Domain.Abstractions;

namespace Catalogue.Domain.Entities;

public record ProductType(string Name,long Id) : ValueObject;
