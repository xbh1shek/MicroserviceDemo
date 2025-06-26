using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Catalogue.Data;
using Catalogue.Migrations;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

//var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
//var dbName = Environment.GetEnvironmentVariable("DB_NAME");
//var dbPassword = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
//var connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPassword};TrustServerCertificate=True";

builder.Services.AddDbContext<CatalogueContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionStrings:CatalogueContext"],
    x => x.MigrationsAssembly("Catalogue")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CatalogueContext>();
    db.Database.Migrate();
}
//Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
