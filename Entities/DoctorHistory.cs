using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VirtualClinic.Entities
{
    public class DoctorHistory
    {
        public int Id { get; set; }
        public int doctorId { get; set; }

        public int patientId { get; set; }

        public string? DoctorNotes { get; set; }

        public int StatusNum { get; set; }

        public DateTime? AppointmentDate { get; set; }
    }
}