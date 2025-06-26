using Inventory.Data;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<InventoryContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryContext") ?? throw new InvalidOperationException("Connection string 'InventoryContext' not found."),
     x => x.MigrationsAssembly("Inventory")));


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InventoryContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
  

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();
