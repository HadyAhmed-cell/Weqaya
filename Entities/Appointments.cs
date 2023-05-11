namespace VirtualClinic.Entities
{
    public class Appointment
    {
        public int Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}