namespace DeveloperStore.Domain.Entities
{
    public class Address
    {
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public int Number { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public GeoLocation Geolocation { get; set; } = new();
    }
}
