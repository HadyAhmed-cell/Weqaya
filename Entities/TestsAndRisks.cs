using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VirtualClinic.Entities
{
    public class TestsAndRisks : BaseEntity
    {
        public string TestsOrRisks { get; set; }
        public IEnumerable<PatientTestsOrRisks> PatientTestsOrRisks { get; set; }
        public IEnumerable<PatientTestsOrRisksOcr> PatientTestsOrRisksOcr { get; set; }

        [ValidateNever]
        public IEnumerable<LabsTestsAndRisks>? LabsTestsAndRisks { get; set; }
    }
}