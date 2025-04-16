using DeveloperStore.Api.Controllers;
using DeveloperStore.Application.Mappings;
using DeveloperStore.Application.UseCases.Sales;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Infrastructure.Data;
using DeveloperStore.Infrastructure.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres") ??
                      "Host=localhost;Port=5432;Database=DeveloperStoreDb;Username=devstore;Password=devstore123"));

builder.Services.AddMediatR(typeof(CreateSaleHandler).Assembly);
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "DeveloperStore API", Version = "v1" });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapSalesEndpoints();
app.MapProductsEndpoints();
app.MapUsersEndpoints();
app.MapCartsEndpoints();

await app.RunAsync();

public partial class Program
{
    protected Program() { }
}
