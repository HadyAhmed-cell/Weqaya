using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VirtualClinic.Entities
{
    public class Lab : BaseEntity
    {
        public Lab()
        {
        }

        public Lab(string name, double price, double reviews, string phoneNumber, string? labDescript, Address address, GeoLocation geoLocation)
        {
            Name = name;
            Price = price;
            Reviews = reviews;
            PhoneNumber = phoneNumber;
            LabDescript = labDescript;
            Address = address;
            GeoLocation = geoLocation;
        }

        public string Name { get; set; }
        public double Price { get; set; }
        public double Reviews { get; set; }
        public string PhoneNumber { get; set; }
        public string? LabDescript { get; set; }

        public Address? Address { get; set; }

        public GeoLocation? GeoLocation { get; set; }

        [ValidateNever]
        public IEnumerable<Patient> Patients { get; set; }
    }
}