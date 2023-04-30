using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Claims;
using VirtualClinic.Data;
using VirtualClinic.Dto;
using VirtualClinic.Entities;
using VirtualClinic.Interfaces;

namespace VirtualClinic.Controllers
{
    //[Authorize(Roles = "Lab")]
    public class LabController : BaseApiController
    {
        private readonly IGenericRepository<Lab> _labRepo;

        private readonly DataContext _context;

        public LabController(IGenericRepository<Lab> labRepo, DataContext context)
        {
            _labRepo = labRepo;

            _context = context;
        }

        [HttpGet("GetAllLabs")]
        public async Task<ActionResult> GetAllLabs()
        {
            var labs = await _labRepo.GetAllAsync();

            return Ok(labs);
        }

        [Authorize(Roles = "Lab")]
        [HttpGet("{id}")]
        public async Task<ActionResult> GetLabById(int id)
        {
            Lab lab = _context.Labs
                .Include(p => p.LabPatients).ThenInclude(p => p.Patient)
                .FirstOrDefault(l => l.Id == id);
            if ( lab == null )
            {
                return BadRequest();
            }

            return Ok(lab);
        }

        [HttpPost("AddLab")]
        public async Task<ActionResult> AddLab(Lab lab)
        {
            await _context.Labs.AddAsync(lab);
            string email = User.FindFirstValue(ClaimTypes.Email);
            lab.Email = email;

            await _context.SaveChangesAsync();

            return Ok("Lab Added Successfully !");
        }

        [HttpPut("EditLabData")]
        public async Task<ActionResult> UpdateLab(Lab lab)
        {
            if ( ModelState.IsValid )
            {
                string email = User.FindFirstValue(ClaimTypes.Email);
                lab.Email = email;
                _context.Labs.Update(lab);

                await _context.SaveChangesAsync();
            }

            return Ok("Lab Data Updated Successfully !");
        }

        [HttpGet("GetCurrentLabProfile")]
        public async Task<ActionResult> GetCurrentLabProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            var patients = await _context.Labs
                .Include(p => p.LabPatients)
                .ThenInclude(o => o.Patient)
                .Where(i => i.Id == userId)
                .ToListAsync();

            if ( user == null )
            {
                return NotFound();
            }
            return Ok(patients);
        }

        [HttpPost("LabResults")]
        public async Task<ActionResult> PostLabResults(int patientId, string labResults)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var patient = await _context.LabPatients.FirstOrDefaultAsync(x => x.PatientId == patientId && x.LabId == userId);
            patient.Results = labResults;
            await _context.SaveChangesAsync();
            return Ok("Results Added Successfully !");
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePatientFromLabDb(int id)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var patientToDelete = _context.LabPatients
                .Where(x => x.LabId == userId && x.PatientId == id);

            _context.LabPatients.RemoveRange(patientToDelete);
            await _context.SaveChangesAsync();
            return Ok("Patient Deleted Successfully !");
        }
    }
}