using DeveloperStore.Infrastructure.Data;
using Microsoft.Extensions.Logging;

namespace DeveloperStore.Infrastructure.Seeders
{
    public class FakeDataSeeder
    {
        private readonly AppDbContext _context;
        private readonly ILogger<FakeDataSeeder> _logger;

        public FakeDataSeeder(AppDbContext context, ILogger<FakeDataSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                if (!_context.Users.Any())
                {
                    _context.Users.AddRange(DevelopmentDataFactory.GenerateUsers(20));
                    _logger.LogInformation("Seeded Users");
                }

                if (!_context.Products.Any())
                {
                    _context.Products.AddRange(DevelopmentDataFactory.GenerateProducts(20));
                    _logger.LogInformation("Seeded Products");
                }

                if (!_context.Carts.Any())
                {
                    _context.Carts.AddRange(DevelopmentDataFactory.GenerateCarts(10));
                    _logger.LogInformation("Seeded Carts");
                }

                if (!_context.Sales.Any())
                {
                    _context.Sales.AddRange(DevelopmentDataFactory.GenerateSales(5));
                    _logger.LogInformation("Seeded Sales");
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation("Fake data seeding completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while seeding fake data.");
                throw new InvalidOperationException("Erro durante o processo de seeding de dados fictícios.", ex);
            }
        }
    }
}
