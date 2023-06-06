using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace VirtualClinic.Entities
{
    public class Lab : BaseEntity
    {
        public string? Name { get; set; }
        public string? LabDescript { get; set; }

        [Editable(false)]
        public string? Email { get; set; }

        public string? Area { get; set; }
        public string? StreetAddress { get; set; }
        public byte[]? Photo { get; set; }

        [ValidateNever]
        public IEnumerable<LabPatient>? LabPatients { get; set; }

        public ICollection<LabReviews>? Reviews { get; set; }

        [ValidateNever]
        public IEnumerable<LabsTestsAndRisks>? LabsTestsAndRisks { get; set; }
    }
}