namespace VirtualClinic.Entities
{
    public class LabsTestsAndRisks
    {
        public int LabId { get; set; }
        public Lab? Lab { get; set; }
        public int TestsAndRisksId { get; set; }
        public TestsAndRisks? TestsAndRisks { get; set; }
        public double? Price { get; set; }
    }
}