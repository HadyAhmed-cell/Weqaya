﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Numerics;
using System.Security.Claims;
using System.Text;
using VirtualClinic.Data;
using VirtualClinic.Entities;
using VirtualClinic.Identity;

namespace VirtualClinic.Controllers
{
    //[Authorize(Roles = "Doctor")]
    public class DoctorController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly UserManager<AppUser> _userManager;

        public DoctorController(DataContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("AddDoctor")]
        public async Task<ActionResult> AddDoctor(Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            doctor.Photo = new byte[byte.MaxValue];
            string email = User.FindFirstValue(ClaimTypes.Email);
            doctor.Email = email;
            await _context.SaveChangesAsync();

            return Ok("Doctor Added Successfully !");
        }

        [HttpPost("AddPhoto")]
        public async Task<ActionResult> AddPhoto(IFormFile file)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            var doctor = await _context.Doctors.SingleOrDefaultAsync(x => x.Id == userId);
            if ( file == null || file.Length == 0 )
                return BadRequest("No file uploaded.");

            var stream = new MemoryStream();

            await file.CopyToAsync(stream);
            doctor.Photo = stream.ToArray();
            await _context.SaveChangesAsync();
            return Ok("Photo Upploaded Successfully");
        }

        //[HttpGet("GetPhoto")]
        //public async Task<ActionResult> GetPhoto()
        //{
        //    string email = User.FindFirstValue(ClaimTypes.Email);
        //    var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
        //    var userId = user.Id;
        //    // Retrieve the photo data as byte array from your database or source
        //    var photo = user.Photo;

        //    // Convert the byte array to a base64 string
        //    string base64String = Convert.ToBase64String(photo);

        //    // Return the base64 string as the response
        //    return Ok(new { photo = base64String });
        //}

        [HttpPost("EditDoctor")]
        public async Task<ActionResult> EditData(Doctor doctor)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            doctor.Email = email;
            user.Price = doctor.Price;
            user.Duration = doctor.Duration;
            user.Education = doctor.Education;
            user.DoctorInfo = doctor.DoctorInfo;
            user.SubSpeciatlity = doctor.SubSpeciatlity;
            user.Speciality = doctor.Speciality;
            user.Area = doctor.Area;
            user.Name = doctor.Name;
            user.TimeFrom = doctor.TimeFrom;
            user.TimeTo = doctor.TimeTo;
            user.StreetAddress = doctor.StreetAddress;

            await _context.SaveChangesAsync();

            return Ok("Data Updated Successfully !");
        }

        [HttpGet("GetDoctorProfile")]
        public async Task<ActionResult> GetCurrentDoctorProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            if ( user == null )
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpGet("ViewDoctorAppointment")]
        public async Task<ActionResult> GetDoctorAppointments()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var appointments = from ap in _context.Appointments
                               where ap.DoctorId == userId
                               select ap.AppointmentDateTime.ToString();

            return Ok(appointments);
        }

        [HttpGet("ViewDoctorPatients")]
        public async Task<ActionResult> ViewPatients()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            var ids = from ptr in _context.DoctorPatients
                      where ptr.doctorId == userId
                      select ptr.patientId;

            var patients = from tr in _context.Patients
                           from ty in _context.DoctorPatients
                           where ids.Contains(tr.Id) && ty.doctorId == userId && ids.Contains(ty.patientId)
                           group tr by tr.Id into g
                           select new
                           {
                               g.FirstOrDefault().Name,
                               PatientAppointment = g.FirstOrDefault().doctorPatients.FirstOrDefault().AppointmentDate.ToString(),
                               g.FirstOrDefault().Gender,
                               g.FirstOrDefault().Age,
                               g.FirstOrDefault().Weight,
                               g.FirstOrDefault().Height,
                               g.FirstOrDefault().PhoneNumber,
                           };

            return Ok(patients);
        }

        [HttpPost("DoctorNotes")]
        public async Task<ActionResult> PostDoctorNotes(int patientId, string notes)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var patient = await _context.DoctorPatients.FirstOrDefaultAsync(x => x.patientId == patientId && x.doctorId == userId);
            patient.DoctorNotes = notes;
            await _context.SaveChangesAsync();
            return Ok("Notes Added Successfully !");
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePatientFromDoctorDb(int id)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var patientToDelete = _context.DoctorPatients
                .Where(x => x.doctorId == userId && x.patientId == id);

            _context.DoctorPatients.RemoveRange(patientToDelete);
            await _context.SaveChangesAsync();
            return Ok("Patient Deleted Successfully !");
        }

        [HttpGet("SearchedDoctors")]
        public async Task<ActionResult> SearchDoctors(string specialty = null, string area = null, string name = null)
        {
            IQueryable<Doctor> doctors = _context.Doctors;

            if ( specialty != null )
            {
                specialty = specialty.Trim();
                doctors = doctors.Where(d => d.Speciality.Contains(specialty));
            }

            if ( name != null )
            {
                name = name.Trim();
                doctors = doctors.Where(d => d.Name.Contains(name));
            }

            if ( area != null )
            {
                area = area.Trim();
                doctors = doctors.Where(d => d.Area.Contains(area));
            }

            if ( !doctors.Any() )
            {
                return NotFound();
            }
            var result = await doctors
                .Select(x => new
                {
                    x.Name,
                    x.Id,
                    x.DoctorInfo,
                    x.Price,
                    x.Speciality,
                    x.SubSpeciatlity,
                    x.Education,
                    x.TimeTo,
                    x.TimeFrom,
                    x.Duration,
                    x.Area,
                    x.StreetAddress,
                    x.Photo,
                    //    Appointments = _context.Appointments
                    //.Where(a => a.DoctorId == x.Id)
                    //.Select(a => new
                    //{
                    //    Appointments = a.AppointmentDateTime.ToString()
                    //})
                    //.ToList()
                })
                .ToListAsync();

            return Ok(result);
        }

        [HttpPost("Appointments")]
        public async Task<ActionResult> AppointmentAdd(ICollection<string> appointments)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            Doctor doctor = await _context.Doctors.Include(a => a.Appointments).SingleOrDefaultAsync(x => x.Id == userId);
            _context.Appointments.RemoveRange(doctor.Appointments);
            _context.SaveChanges();

            foreach ( string appointment in appointments )
            {
                Appointment appointment1 = new Appointment
                {
                    AppointmentDateTime = DateTime.Parse(appointment),
                    DoctorId = userId,
                };
                await _context.Appointments.AddAsync(appointment1);
                await _context.SaveChangesAsync();
            }

            return Ok("Appointments Dates Added !");
        }
    }
}