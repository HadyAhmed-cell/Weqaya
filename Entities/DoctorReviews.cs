namespace VirtualClinic.Entities
{
    public class DoctorReviews
    {
        public int Id { get; set; }
        public int Reviews { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}