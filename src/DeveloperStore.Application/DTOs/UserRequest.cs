namespace DeveloperStore.Application.DTOs
{
    public class UserRequest
    {
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public NameDto Name { get; set; } = new();
        public AddressDto Address { get; set; } = new();
        public string Phone { get; set; } = null!;
        public string Status { get; set; } = "Active";
        public string Role { get; set; } = "Customer";
    }

    public class NameDto
    {
        public string Firstname { get; set; } = null!;
        public string Lastname { get; set; } = null!;
    }

    public class AddressDto
    {
        public string City { get; set; } = null!;
        public string Street { get; set; } = null!;
        public int Number { get; set; }
        public string Zipcode { get; set; } = null!;
        public GeolocationDto Geolocation { get; set; } = new();
    }

    public class GeolocationDto
    {
        public string Lat { get; set; } = null!;
        public string Long { get; set; } = null!;
    }
}
