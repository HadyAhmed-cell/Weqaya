using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace VirtualClinic.Entities
{
    public class LabHistory
    {
        public int Id { get; set; }
        public int labId { get; set; }

        public int patientId { get; set; }

        public string? Results { get; set; }

        public int TestId { get; set; }
        public int StatusNum { get; set; }
        public int Price { get; set; }
    }
}