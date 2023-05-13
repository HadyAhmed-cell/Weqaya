using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VirtualClinic.Entities
{
    public class DoctorPatient
    {
        public int doctorId { get; set; }

        [ValidateNever]
        public Doctor? doctor { get; set; }

        public int patientId { get; set; }

        [ValidateNever]
        public Patient? patient { get; set; }

        public string? DoctorNotes { get; set; }

        public DateTime? AppointmentDate { get; set; }
    }
}