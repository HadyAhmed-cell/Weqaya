using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using VirtualClinic.Identity;

namespace VirtualClinic.Entities
{
    public class Doctor : BaseEntity
    {
        public string Name { get; set; }
        public string Speciality { get; set; }
        public string? PHD { get; set; }
        public double Reviews { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? AppoinmentsDatesAvailable { get; set; }
        public double Price { get; set; }
        public string? DoctorInfo { get; set; }
        public bool Availability { get; set; }
        public string? Syndicates { get; set; }

        [Editable(false)]
        public string? Email { get; set; }

        public byte[]? ProfilePhoto { get; set; }

        public string? City { get; set; }
        public string? Area { get; set; }
        public string? StreetAddress { get; set; }

        public IEnumerable<DoctorPatient>? doctorPatients { get; set; }
    }
}