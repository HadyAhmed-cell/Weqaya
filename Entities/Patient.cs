using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VirtualClinic.Entities
{
    public class Patient : BaseEntity
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }

        public int Age { get; set; }
        public string? SocialStatus { get; set; }

        public int NoOfKids { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int WaistDiameter { get; set; }
        public string? Syndicates { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }

        public double PressureValue { get; set; }
        public string? Diabetes { get; set; }
        public int? testsAndRisksId { get; set; }
        public TestsAndRisks testsAndRisks { get; set; }

        public Address? Address { get; set; }

        public GeoLocation? GeoLocation { get; set; }

        [ValidateNever]
        public IEnumerable<DoctorPatient>? doctorPatients { get; set; }

        [ValidateNever]
        public IEnumerable<LabPatient> labPatients { get; set; }
    }
}