using Bogus;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Infrastructure.Persistence;

namespace DeveloperStore.Infrastructure.Seeders
{
    public static class DatabaseSeeder
    {
        private static readonly string[] Roles = new[] { "Customer", "Manager", "Admin" };

        public static void Seed(ApplicationDbContext context)
        {
            if (context.Products.Any()) return;

            var faker = new Faker("pt_BR");

            // Produtos
            var products = new Faker<Product>()
                .RuleFor(p => p.Title, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
                .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
                .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
                .RuleFor(p => p.Rating, f => new Rating
                {
                    Rate = f.Random.Decimal(1, 5),
                    Count = f.Random.Int(0, 1000)
                })
                .Generate(20);

            // Usuários
            var users = new Faker<User>()
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Password, _ => "1234")
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.Role, f => f.PickRandom(Roles))
                .RuleFor(u => u.Status, _ => "Active")
                .RuleFor(u => u.Name, f => new Name
                {
                    Firstname = f.Name.FirstName(),
                    Lastname = f.Name.LastName()
                })
                .RuleFor(u => u.Address, f => new Address
                {
                    City = f.Address.City(),
                    Street = f.Address.StreetName(),
                    Number = f.Random.Int(1, 9999),
                    Zipcode = f.Address.ZipCode(),
                    Geolocation = new Geolocation
                    {
                        Lat = f.Address.Latitude().ToString("F4"),
                        Long = f.Address.Longitude().ToString("F4")
                    }
                })
                .Generate(10);

            // Carrinhos
            var carts = new List<Cart>();
            for (int i = 0; i < 10; i++)
            {
                carts.Add(new Cart
                {
                    UserId = users[i % users.Count].Id,
                    Date = DateTime.SpecifyKind(DateTime.UtcNow.AddDays(-i), DateTimeKind.Utc),
                    Products = new List<CartItem>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        ProductId = products[i % products.Count].Id,
                        Quantity = faker.Random.Int(1, 5)
                    }
                }
                });
            }

            context.Products.AddRange(products);
            context.Users.AddRange(users);
            context.Carts.AddRange(carts);
            context.SaveChanges();

            var savedProducts = context.Products.ToList();

            // Vendas
            var sales = new Faker<Sale>()
                .RuleFor(s => s.Id, _ => Guid.NewGuid())
                .RuleFor(s => s.SaleNumber, f => $"S-{f.Random.Number(1000, 9999)}")
                .RuleFor(s => s.SaleDate, f => DateTime.SpecifyKind(f.Date.Recent(30), DateTimeKind.Utc))
                .RuleFor(s => s.Customer, f => f.Name.FullName())
                .RuleFor(s => s.Branch, f => f.Company.CompanyName())
                .RuleFor(s => s.Items, f =>
                {
                    var qtdItems = f.Random.Int(1, 5);

                    return new Faker<SaleItem>()
                        .RuleFor(i => i.Id, _ => Guid.NewGuid())
                        .RuleFor(i => i.ProductId, _ => f.PickRandom(savedProducts).Id)
                        .RuleFor(i => i.Quantity, _ => f.Random.Int(1, 20))
                        .RuleFor(i => i.UnitPrice, _ => f.Random.Decimal(10, 1000))
                        .RuleFor(i => i.Discount, _ => f.Random.Decimal(0, 0.2m))
                        .Generate(qtdItems);
                })
                .Generate(10);

            context.Sales.AddRange(sales);
            context.SaveChanges();
        }
    }
}
