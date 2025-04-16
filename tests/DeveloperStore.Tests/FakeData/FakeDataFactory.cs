using Bogus;
using DeveloperStore.Domain.Entities;
using DeveloperStore.Domain.Enum;

namespace DeveloperStore.Tests.FakeData
{
    public static class FakeDataFactory
    {
        public static Faker<User> UserFaker => new Faker<User>()
            .RuleFor(u => u.Id, f => f.Random.Int(1, 1000))
            .RuleFor(u => u.Username, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber())
            .RuleFor(u => u.Status, f => f.PickRandom<UserStatus>())
            .RuleFor(u => u.Role, f => f.PickRandom<UserRole>())
            .RuleFor(u => u.Name, f => new Name
            {
                FirstName = f.Name.FirstName(),
                LastName = f.Name.LastName()
            })
            .RuleFor(u => u.Address, f => new Address
            {
                City = f.Address.City(),
                Street = f.Address.StreetName(),
                Number = f.Random.Int(1, 500),
                ZipCode = f.Address.ZipCode(),
                Geolocation = new GeoLocation
                {
                    Lat = f.Address.Latitude().ToString(),
                    Long = f.Address.Longitude().ToString()
                }
            });

        public static Faker<Product> ProductFaker => new Faker<Product>()
            .RuleFor(p => p.Id, f => f.Random.Int(1, 1000))
            .RuleFor(p => p.Title, f => f.Commerce.ProductName())
            .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
            .RuleFor(p => p.Category, f => f.Commerce.Categories(1)[0])
            .RuleFor(p => p.Image, f => f.Image.PicsumUrl())
            .RuleFor(p => p.Price, f => f.Finance.Amount(10, 1000))
            .RuleFor(p => p.Rating, f => new ProductRating
            {
                Rate = f.Random.Decimal(1, 5),
                Count = f.Random.Int(0, 500)
            });

        public static Faker<Cart> CartFaker => new Faker<Cart>()
            .RuleFor(c => c.Id, f => f.Random.Int(1, 1000))
            .RuleFor(c => c.UserId, f => f.Random.Int(1, 1000))
            .RuleFor(c => c.Date, f => f.Date.Past())
            .RuleFor(c => c.Products, f => new List<CartItem>
            {
                new CartItem {
                    ProductId = f.Random.Int(1, 1000),
                    Quantity = f.Random.Int(1, 5)
                },
                new CartItem {
                    ProductId = f.Random.Int(1, 1000),
                    Quantity = f.Random.Int(1, 5)
                }
            });

        public static Faker<Sale> SaleFaker => new Faker<Sale>()
            .RuleFor(s => s.Id, f => f.Random.Int(1, 1000))
            .RuleFor(s => s.SaleNumber, f => f.Random.Guid().ToString())
            .RuleFor(s => s.Date, f => f.Date.Recent())
            .RuleFor(s => s.CustomerId, f => f.Random.Int(1, 1000))
            .RuleFor(s => s.Branch, f => f.Company.CompanyName())
            .RuleFor(s => s.Cancelled, f => f.Random.Bool())
            .RuleFor(s => s.Items, f => new List<SaleItem>
            {
                new SaleItem
                {
                    ProductId = f.Random.Int(1, 1000),
                    Quantity = f.Random.Int(1, 20),
                    UnitPrice = f.Finance.Amount(10, 500)
                }
            });

        public static IEnumerable<User> GenerateUsers(int count = 10) => UserFaker.Generate(count);
        public static IEnumerable<Product> GenerateProducts(int count = 10) => ProductFaker.Generate(count);
        public static IEnumerable<Cart> GenerateCarts(int count = 10) => CartFaker.Generate(count);
        public static IEnumerable<Sale> GenerateSales(int count = 10) => SaleFaker.Generate(count);
    }
}
