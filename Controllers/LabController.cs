using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VirtualClinic.Data;
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

        [HttpPost("AddPhoto")]
        public async Task<ActionResult> AddPhoto(IFormFile file)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            var lab = await _context.Labs.SingleOrDefaultAsync(x => x.Id == userId);
            if ( file == null || file.Length == 0 )
                return BadRequest("No file uploaded.");

            var stream = new MemoryStream();

            await file.CopyToAsync(stream);
            lab.Photo = stream.ToArray();
            await _context.SaveChangesAsync();
            return Ok("Photo Upploaded Successfully");
        }

        [HttpPut("EditLabData")]
        public async Task<ActionResult> UpdateLab(Lab lab)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            lab.Email = email;
            user.Id = userId;
            user.Name = lab.Name;
            user.Area = lab.Area;
            user.StreetAddress = lab.StreetAddress;
            user.LabDescript = lab.LabDescript;

            await _context.SaveChangesAsync();

            return Ok("Lab Data Updated Successfully !");
        }

        [HttpGet("GetCurrentLabProfile")]
        public async Task<ActionResult> GetCurrentLabProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);

            var userId = user.Id;
            var avgReviews = await _context.LabReviews.Where(p => p.LabId == userId).AverageAsync(p => p.Reviews);

            var result = new
            {
                user,
                Avg = avgReviews
            };

            return Ok(user);
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
                              lp.LabId,
                              t.TestsOrRisks,
                              l.Name,
                              lp.PatientId,
                              l.PhoneNumber,
                              lp.StatusNum,
                              Results = lp.Results.Replace("\n", "").Replace("\r", "")
                          };
            return Ok(results);
        }

        [HttpGet("GetLabPatientHistory")]
        public async Task<ActionResult> GetLabPatientHistory()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var ids = from ptr in _context.LabHistories
                      where ptr.labId == userId
                      select ptr.patientId;

            var results = from lp in _context.LabHistories
                          join t in _context.testsAndRisks on lp.TestId equals t.Id
                          join l in _context.Patients on lp.patientId equals l.Id
                          where lp.labId == user.Id
                          select new
                          {
                              lp.Price,
                              t.TestsOrRisks,
                              lp.TestId,
                              lp.patientId,
                              l.Name,
                              l.PhoneNumber,
                              lp.StatusNum,
                              Results = lp.Results.Replace("\n", "").Replace("\r", "")
                          };
            return Ok(results);
        }

        [HttpPost("LabResults")]
        public async Task<ActionResult> PostLabResults(int patientId, string results)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var patientToHistory = await _context.LabPatients
                    .SingleOrDefaultAsync(x => x.LabId == userId && x.PatientId == patientId);

            var patientHistory = new LabHistory
            {
                labId = userId,
                patientId = patientId,
                TestId = patientToHistory.TestId,
                Results = results,
                Price = patientToHistory.Price,
                StatusNum = 1,
            };
            _context.LabPatients.Remove(patientToHistory);
            await _context.LabHistories.AddAsync(patientHistory);
            await _context.SaveChangesAsync();

            return Ok("Patient Completed Tests !");
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePatientFromLabDb(int id)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var patientToDelete = await _context.LabPatients
                    .SingleOrDefaultAsync(x => x.LabId == userId && x.PatientId == id);

            var patientHistory = new LabHistory
            {
                labId = userId,
                patientId = id,
                TestId = patientToDelete.TestId,
                Results = patientToDelete.Results,
                Price = patientToDelete.Price,
                StatusNum = 2,
            };
            _context.LabPatients.Remove(patientToDelete);
            await _context.LabHistories.AddAsync(patientHistory);
            await _context.SaveChangesAsync();

            return Ok("Patient Cancelled Successfully !");
        }

        [HttpGet("SearchedLabs")]
        public async Task<ActionResult> SearchLabs(string test = null, string area = null)
        {
            IQueryable<Lab> labs = _context.Labs;

            if ( test != null )
            {
                labs = from l in _context.Labs
                       join lt in _context.LabsTestsAndRisks on l.Id equals lt.LabId
                       join t in _context.testsAndRisks on lt.TestsAndRisksId equals t.Id
                       where t.TestsOrRisks.Contains(test)
                       select l;
            }

            if ( area != null )
            {
                area = area.Trim();
                labs = labs.Where(l => l.Area.Contains(area));
            }

            var result = await labs
        .Select(l => new
        {
            l.Area,
            l.LabDescript,
            l.Name,
            l.Id,
            l.Photo,
            l.Email,
            l.StreetAddress,

            Avg = _context.LabReviews
                .Where(r => r.LabId == l.Id)
                .Average(r => r.Reviews)
        })
        .ToListAsync();

            if ( !result.Any() )
            {
                return NotFound();
            }

            return Ok(result);
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
                var labTest = await _context.LabsTestsAndRisks.FirstOrDefaultAsync(x => x.LabId == userId && x.TestsAndRisksId == testId);
                _context.LabsTestsAndRisks.Remove(labTest);
                await _context.LabsTestsAndRisks.AddAsync(labsTestsAndRisks);
                await _context.SaveChangesAsync();
            }
            else
            {
                await _context.LabsTestsAndRisks.AddAsync(labsTestsAndRisks);
                await _context.SaveChangesAsync();
            }

            return Ok("Test Price Assigned Successfully !");
        }

        [HttpGet("ViewLabReviews")]
        public async Task<ActionResult> ViewLabReviews()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Labs.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var reviews = _context.LabReviews.Include(x => x.Patient).Where(p => p.LabId == userId)
                .Select(o => new
                {
                    o.Patient.Name,
                    o.ReviewsComments,
                    o.Reviews
                });
            var avgReviews = await _context.LabReviews.AverageAsync(p => p.Reviews);

            var result = new
            {
                Avg = avgReviews,
                Reviews = reviews
            };

            return Ok(result);
        }
    }
}