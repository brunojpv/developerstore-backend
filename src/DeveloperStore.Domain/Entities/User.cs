namespace DeveloperStore.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Name Name { get; set; } = new();
        public Address Address { get; set; } = new();
        public string Phone { get; set; } = null!;
        public string Status { get; set; } = "Active"; // Active, Inactive, Suspended
        public string Role { get; set; } = "Customer"; // Customer, Manager, Admin
    }

    public class Name
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
    }

    public class Address
    {
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public int Number { get; set; }
        public string Zipcode { get; set; } = null!;
        public Geolocation Geolocation { get; set; } = new();
    }

    public class Geolocation
    {
        public string Lat { get; set; } = null!;
        public string Long { get; set; } = null!;
    }
}
