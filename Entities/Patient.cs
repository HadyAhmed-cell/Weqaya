﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using VirtualClinic.Entities;

namespace VirtualClinic.Entities
{
    public class Patient : BaseEntity
    {
        public string Name { get; set; }
        public string Gender { get; set; }

        public int Age { get; set; }
        public string? SocialStatus { get; set; }

        public int? NoOfKids { get; set; }
        public int Weight { get; set; }
        public int Height { get; set; }
        public int? WaistDiameter { get; set; }
        public string? Syndicates { get; set; }

        [Editable(false)]
        public string? Email { get; set; }

        public string PhoneNumber { get; set; }
        public string? Notes { get; set; }

        public bool? DiabetesRelatives { get; set; }
        public bool? RelativesWithHeartAttacksOrHighColestrol { get; set; }
        public bool? Smoking { get; set; }
        public bool? MedicineForDiabetesOrPressure { get; set; }
        public bool? HighPressure { get; set; }
        public bool? Diabetes { get; set; }
        public string? City { get; set; }
        public string? Area { get; set; }
        public string? StreetAddress { get; set; }

        [ValidateNever]
        public IEnumerable<DoctorPatient>? doctorPatients { get; set; }

        [ValidateNever]
        public IEnumerable<LabPatient> labPatients { get; set; }

        [ValidateNever]
        public IEnumerable<PatientTestsOrRisks> PatientTestsAndRisks { get; set; }
    }
}