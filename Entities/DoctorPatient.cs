namespace VirtualClinic.Entities
{
    public class DoctorPatient
    {
        public int doctorId { get; set; }
        public Doctor doctor { get; set; }
        public int patientId { get; set; }

        public Patient patient { get; set; }
        public string? DoctorNotes { get; set; }
    }
}