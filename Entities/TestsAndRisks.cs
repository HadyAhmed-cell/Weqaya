namespace VirtualClinic.Entities
{
    public class TestsAndRisks : BaseEntity
    {
        public string TestsOrRisks { get; set; }
        public IEnumerable<PatientTestsOrRisks> PatientTestsOrRisks { get; set; }
    }
}