using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VirtualClinic.Entities
{
    public class Patient : BaseEntity
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string? SocialStatus { get; set; }

        public int NoOfKids { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int WaistDiameter { get; set; }
        public string? Syndicates { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Diseases { get; set; }
        public double PressureValue { get; set; }
        public string? Diabetes { get; set; }

        public Address? Address { get; set; }

        public GeoLocation? GeoLocation { get; set; }

        [ValidateNever]
        public IEnumerable<Doctor>? Doctors { get; set; }

        [ValidateNever]
        public IEnumerable<Lab>? Labs { get; set; }
    }
}