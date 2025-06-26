using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace Catalogue.Infrastructure.DBContext
{
    public class CatalogueQueryDbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public int TenantId { get; }
        public int UserId { get; }
        public CatalogueQueryDbContext(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            TenantId = Convert.ToInt32(_httpContextAccessor.HttpContext.Items["TenantId"]);
            UserId = Convert.ToInt32(_httpContextAccessor.HttpContext.Items["userId"]);
        }

        public MySqlConnection CreateConnection()
           => new MySqlConnection(_configuration.GetConnectionString("CatalogueConnectionString"));
    }
}
