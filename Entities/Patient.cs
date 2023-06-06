using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace VirtualClinic.Entities
{
    public class Patient : BaseEntity
    {
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public int? Age { get; set; }
        public int? Weight { get; set; }
        public int? Height { get; set; }

        [Editable(false)]
        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? DiabetesRelatives { get; set; }
        public string? RelativesWithHeartAttacksOrHighColestrol { get; set; }
        public string? Smoking { get; set; }
        public string? MedicineForDiabetesOrPressure { get; set; }
        public string? HighPressure { get; set; }
        public string? Diabetes { get; set; }

        [ValidateNever]
        public IEnumerable<DoctorPatient>? doctorPatients { get; set; }

        [ValidateNever]
        public IEnumerable<LabPatient> labPatients { get; set; }

        [ValidateNever]
        public IEnumerable<PatientTestsOrRisks> PatientTestsAndRisks { get; set; }

        public IEnumerable<PatientTestsOrRisksOcr> PatientTestsOrRisksOcr { get; set; }

        public IEnumerable<DoctorReviews> DoctorReviews { get; set; }

        public IEnumerable<LabReviews> LabReviews { get; set; }
    }
}