﻿namespace VirtualClinic.Entities
{
    public class PatientTestsOrRisks
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public int TestTestsAndRisksId { get; set; }
        public TestsAndRisks TestsAndRisks { get; set; }
    }
}