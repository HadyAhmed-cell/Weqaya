namespace VirtualClinic.Entities
{
    public class LabPatient
    {
        public int LabId { get; set; }
        public Lab Lab { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public string Results { get; set; }
    }
}