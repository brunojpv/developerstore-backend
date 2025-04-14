using DeveloperStore.Application.Services;
using DeveloperStore.Domain.Interfaces;
using DeveloperStore.Infrastructure.Persistence;
using DeveloperStore.Infrastructure.Repositories;
using DeveloperStore.Infrastructure.Seeders;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adiciona o DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper: registra todos os profiles do assembly Application
builder.Services.AddAutoMapper(typeof(DeveloperStore.Application.Mappings.SaleProfile).Assembly);
builder.Services.AddAutoMapper(typeof(DeveloperStore.Application.Mappings.UserProfile).Assembly);
builder.Services.AddAutoMapper(typeof(DeveloperStore.Application.Mappings.ProductProfile).Assembly);
builder.Services.AddAutoMapper(typeof(DeveloperStore.Application.Mappings.CartProfile).Assembly);

// Reposit�rios e servi�os
builder.Services.AddScoped<ISaleRepository, SaleRepository>();
builder.Services.AddScoped<ISaleService, SaleService>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "DeveloperStore API",
        Version = "v1",
        Description = "API de avalia��o t�cnica com m�dulos de vendas, usu�rios, produtos, carrinhos e autentica��o.",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Bruno Vieira",
            Email = "bruno@email.com",
            Url = new Uri("https://github.com/brunojpv")
        }
    });

    // Ativar leitura de coment�rios XML (doc do m�todo)
    var xmlPath = Path.Combine(AppContext.BaseDirectory, "DeveloperStore.Api.xml");
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    DatabaseSeeder.Seed(db);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

await app.RunAsync();