namespace VirtualClinic.Entities
{
    public class LabReviews
    {
        public int Reviews { get; set; }
        public string ReviewsComments { get; set; }
        public int LabId { get; set; }
        public Lab Lab { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}