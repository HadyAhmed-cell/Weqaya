namespace VirtualClinic.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; }
        public string Speciality { get; set; }
        public string PHD { get; set; }
        public double Reviews { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime AppoinmentsDatesAvailable { get; set; }
        public double Price { get; set; }
        public string DoctorInfo { get; set; }
        public byte[] ProfilePhoto { get; set; }

        public Address Address { get; set; }

        public GeoLocation GeoLocation { get; set; }
        public IEnumerable<DoctorPatient> doctorPatients { get; set; }
    }
}