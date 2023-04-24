﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VirtualClinic.Data;
using VirtualClinic.Entities;
using VirtualClinic.Identity;

namespace VirtualClinic.Controllers
{
    [Authorize(Roles = "Doctor")]
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
        public async Task<ActionResult> AddDoctor([FromQuery] Doctor doctor)
        {
            await _context.Doctors.AddAsync(doctor);
            string email = User.FindFirstValue(ClaimTypes.Email);
            doctor.Email = email;
            await _context.SaveChangesAsync();

            return Ok("Doctor Added Successfully !");
        }

        [HttpPost("EditDoctor")]
        public async Task<ActionResult> EditData([FromQuery] Doctor doctor)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            doctor.Email = email;
            _context.Doctors.Update(doctor);
            await _context.SaveChangesAsync();

            return Ok("Data Updated Successfully !");
        }

        [HttpGet("GetDoctorProfile")]
        public async Task<ActionResult> GetCurrentDoctorProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            var patients = await _context.Doctors
                .Include(p => p.doctorPatients)
                .ThenInclude(o => o.patient)
                .Where(i => i.Id == userId)
                .ToListAsync();

            if ( user == null )
            {
                return NotFound();
            }
            return Ok(patients);
        }

        [HttpPost("DoctorNotes")]
        public async Task<ActionResult> PostDoctorNotes(int id, string notes)
        {
            var patient = await _context.DoctorPatients.FirstOrDefaultAsync(x => x.patientId == id);
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
    }
}