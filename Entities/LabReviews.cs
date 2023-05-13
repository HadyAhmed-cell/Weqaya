namespace VirtualClinic.Entities
{
    public class LabReviews
    {
        public int Id { get; set; }
        public int Reviews { get; set; }
        public int LabId { get; set; }
        public Lab Lab { get; set; }
    }
}