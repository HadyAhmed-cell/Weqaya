using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VirtualClinic.Entities
{
    public class LabPatient
    {
        public int LabId { get; set; }

        [ValidateNever]
        public Lab? Lab { get; set; }

        public int PatientId { get; set; }

        [ValidateNever]
        public Patient? Patient { get; set; }

        public string? Results { get; set; }
    }
}