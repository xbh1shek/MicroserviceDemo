

namespace Catalogue.Domain.Entities
{
    internal class CatalogueException: Exception
    {
        private readonly string? _paramName;
        public string? ParamName => _paramName;
        public Type ExceptionType => typeof(CatalogueException);

        public CatalogueException()
        { }

        public CatalogueException(string message)
            : base(message)
        { }

        public CatalogueException(string message, Exception innerException)
            : base(message, innerException)
        { }
        public CatalogueException(string message, string? paramName)
          : base(message)
        {
            _paramName = paramName;
        }
    }
}
