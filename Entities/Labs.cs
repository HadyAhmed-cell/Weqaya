using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using VirtualClinic.Identity;

namespace VirtualClinic.Entities
{
    public class Lab : BaseEntity
    {
        public Lab()
        {
        }

        public Lab(string name, double price, double reviews, string phoneNumber, string? labDescript, string city, string area, string street)
        {
            Name = name;
            Price = price;
            Reviews = reviews;
            PhoneNumber = phoneNumber;
            LabDescript = labDescript;
            City = city;
            Area = area;
            StreetAddress = street;
        }

        public string Name { get; set; }
        public double Price { get; set; }
        public double Reviews { get; set; }
        public string PhoneNumber { get; set; }
        public string? LabDescript { get; set; }

        [Editable(false)]
        public string? Email { get; set; }

        public string? City { get; set; }
        public string? Area { get; set; }
        public string? StreetAddress { get; set; }

        [ValidateNever]
        public IEnumerable<LabPatient> LabPatients { get; set; }
    }
}