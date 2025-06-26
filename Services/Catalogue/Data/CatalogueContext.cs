using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Catalogue;

namespace Catalogue.Data
{
    public class CatalogueContext : DbContext
    {
        public CatalogueContext (DbContextOptions<CatalogueContext> options)
            : base(options)
        {
        }

        public DbSet<Catalogue.CatalogueModel> CatalogueModel { get; set; } = default!;
    }
}
