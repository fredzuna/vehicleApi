using Microsoft.EntityFrameworkCore;
using YourNamespace.Data;
using YourNamespace.Services;
var builder = WebApplication.CreateBuilder(args);

// Register services here
builder.Services.AddScoped<IVehicleService, VehicleService>();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=localdb.sqlite"));

var app = builder.Build();

// Ensure the database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        context.Database.EnsureCreated(); // Ensure the database schema is created
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();