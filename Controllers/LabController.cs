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
            lab.Photo = new byte[byte.MaxValue];

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
            var labProfile = await _context.Labs.SingleOrDefaultAsync();

            return Ok(labProfile);
        }

        [HttpGet("GetLabTests")]
        public async Task<ActionResult> GetLabTests()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var tests = from tr in _context.testsAndRisks
                        join ty in _context.LabsTestsAndRisks on tr.Id equals ty.TestsAndRisksId
                        where ty.LabId == userId
                        group tr by tr.Id into g
                        select new
                        {
                            TestsAvailable = g.FirstOrDefault().TestsOrRisks,
                            TestPrice = g.FirstOrDefault().LabsTestsAndRisks.FirstOrDefault().Price
                        };
            return Ok(tests);
        }

        [HttpGet("GetLabPatient")]
        public async Task<ActionResult> GetLabPatient()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var ids = from ptr in _context.LabPatients
                      where ptr.LabId == userId
                      select ptr.PatientId;

            var results = from lp in _context.LabPatients
                          join t in _context.testsAndRisks on lp.TestId equals t.Id
                          join l in _context.Patients on lp.PatientId equals l.Id
                          where lp.LabId == user.Id
                          select new
                          {
                              lp.Price,
                              TestName = t.TestsOrRisks,
                              PatientName = l.Name,
                              PatientNumber = l.PhoneNumber
                          };
            return Ok(results);
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

        [HttpPost("ChooseTests")]
        public async Task<ActionResult> ChooseTest(int testId, double price)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var lab = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var test = await _context.testsAndRisks.FirstOrDefaultAsync(x => x.Id == testId);
            var userId = lab.Id;
            bool LabTests = await _context.LabsTestsAndRisks.AnyAsync(x => x.LabId == userId && x.TestsAndRisksId == testId);
            LabsTestsAndRisks labsTestsAndRisks = new()
            {
                TestsAndRisksId = testId,
                LabId = userId,
                //doctor = doctor1,
                //patient = patient1,
                Price = price
            };
            if ( LabTests == true )
            {
                return BadRequest("Test Already Priced !");
            }
            else
            {
                await _context.LabsTestsAndRisks.AddAsync(labsTestsAndRisks);
                await _context.SaveChangesAsync();
            }

            return Ok("Test Price Assigned Successfully !");
        }
    }
}