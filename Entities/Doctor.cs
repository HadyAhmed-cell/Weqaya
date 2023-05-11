﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using VirtualClinic.Identity;

namespace VirtualClinic.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; }
        public string? Speciality { get; set; }
        public string? Education { get; set; }
        public double? Reviews { get; set; }

        public double? Price { get; set; }
        public string? DoctorInfo { get; set; }
        public string? SubSpeciatlity { get; set; }
        public int? TimeFrom { get; set; }
        public int? TimeTo { get; set; }
        public int? Duration { get; set; }

        [Editable(false)]
        public string? Email { get; set; }

        public string? Area { get; set; }
        public string? StreetAddress { get; set; }
        public byte[]? Photo { get; set; }
        public ICollection<Appointment>? Appointments { get; set; }

        [ValidateNever]
        public IEnumerable<DoctorPatient>? doctorPatients { get; set; }
    }
}