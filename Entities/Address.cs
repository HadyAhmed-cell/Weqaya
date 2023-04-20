namespace VirtualClinic.Entities
{
    public class Address : BaseEntity
    {
        public string? City { get; set; }
        public string? Area { get; set; }
        public string? StreetAddress { get; set; }
    }
}