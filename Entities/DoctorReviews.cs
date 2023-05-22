namespace VirtualClinic.Entities
{
    public class DoctorReviews
    {
        public int Reviews { get; set; }
        public string ReviewsComments { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }

        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}