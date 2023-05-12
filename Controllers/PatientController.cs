using Amazon;
using Amazon.Textract;
using Amazon.Textract.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VirtualClinic.Data;
using VirtualClinic.Entities;

namespace VirtualClinic.Controllers
{
    public class PatientController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IAmazonTextract _amazonTextract;

        public PatientController(DataContext context)
        {
            _context = context;
            _amazonTextract = new AmazonTextractClient("AKIAZLG4KFFZOPTXFC4I", "6b8yDSvsQQbV9iY7cWNiKkhnhIOBJC/Zgl0ubb+X", RegionEndpoint.USEast1);
        }

        [HttpGet]
        public async Task<ActionResult> GetAllPatients()
        {
            var patients = await _context.Patients

                .ToListAsync();
            return Ok(patients);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{GetPatientById}")]
        public async Task<ActionResult> GetPatientById(int GetPatientById)
        {
            var patient = await _context.Patients
                .Include(p => p.PatientTestsAndRisks).ThenInclude(r => r.TestsAndRisks)
                .FirstOrDefaultAsync(x => x.Id == GetPatientById);

            return Ok(patient);
        }

        [HttpPost("AddPatient")]
        public async Task<ActionResult> CreatePatient(Patient patient)
        {
            await _context.Patients.AddAsync(patient);
            string email = User.FindFirstValue(ClaimTypes.Email);
            patient.Email = email;
            await _context.SaveChangesAsync();

            return Ok(patient);
        }

        [HttpPut("EditPatientData")]
        public async Task<ActionResult> EditPatient(Patient patient, int id)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            patient.Email = email;
            patient.Id = id;
            _context.Patients.Update(patient);

            await _context.SaveChangesAsync();
            return Ok(patient);
        }

        [HttpGet("GetPatientProfile")]
        public async Task<ActionResult> GetPatientProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Id == userId);
            return Ok(patient);
        }

        [HttpPut("{PatientRecommendId}")]
        public async Task<ActionResult> RecommendTests(int PatientRecommendId)
        {
            Patient patient = await _context.Patients
                .Include(p => p.PatientTestsAndRisks)
                .SingleOrDefaultAsync(x => x.Id == PatientRecommendId);

            var patientId = patient.Id;

            _context.PatientTestsAndRisks.RemoveRange(patient.PatientTestsAndRisks);
            _context.SaveChanges();

            if ( patient.Age >= 45 && patient.Diabetes == true )
            {
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 1 && x.PatientId == PatientRecommendId) )
                {
                    if ( patient.Weight >= 90 || patient.HighPressure == true || patient.MedicineForDiabetesOrPressure == true )
                    {
                        var riskid = 1;
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = patientId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                        //_context.Entry(patient).State = EntityState.Detached;
                    }
                }
            }
            if ( patient.Gender == "Male" && (patient.Age >= 45 && patient.Age <= 65) && patient.Weight >= 90 )
            {
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 3 && x.PatientId == PatientRecommendId) )
                {
                    if ( patient.RelativesWithHeartAttacksOrHighColestrol == true || patient.Diabetes == true || patient.Smoking == true )
                    {
                        var riskid = 3;
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = patientId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                        //_context.Entry(patient).State = EntityState.Detached;
                    }
                }
            }

            if ( patient.Gender == "Female" && (patient.Age >= 55 && patient.Age <= 65) && patient.Weight >= 90 )
            {
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 3 && x.PatientId == PatientRecommendId) )
                {
                    if ( patient.RelativesWithHeartAttacksOrHighColestrol == true || patient.Diabetes == true || patient.Smoking == true )
                    {
                        var riskid = 3;
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = patientId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                    }
                }
                //_context.Entry(patient).State = EntityState.Modified;
            }

            if ( patient.Age >= 66 || patient.Weight >= 90 )
            {
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 3 && x.PatientId == PatientRecommendId) )
                {
                    if ( patient.RelativesWithHeartAttacksOrHighColestrol == true || patient.Diabetes == true || patient.Smoking == true )
                    {
                        var riskid = 3;
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = patientId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                        //_context.Entry(patient).State = EntityState.Detached;
                    }
                }
            }

            if ( patient.Diabetes == true )
            {
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 6 && x.PatientId == PatientRecommendId) )
                {
                    int[] testsarr = new int[] { 4, 5, 6, 7, 8 };
                    for ( int i = 0; i <= 4; i++ )
                    {
                        var riskid = testsarr[i];
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = patientId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                        //_context.Entry(patient).State = EntityState.Detached;
                    }
                }
                else
                {
                    int[] testsarr = new int[] { 4, 5, 7, 8 };
                    for ( int i = 0; i <= 3; i++ )
                    {
                        var riskid = testsarr[i];
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = patientId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                        //_context.Entry(patient).State = EntityState.Detached;
                    }
                }
            }
            if ( patient.Weight >= 90 )
            {
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 6 && x.PatientId == PatientRecommendId) )
                {
                    int[] testsarr = new int[] { 6, 9, 10 };
                    for ( int i = 0; i <= 2; i++ )
                    {
                        var riskid = testsarr[i];
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = patientId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                        //_context.Entry(patient).State = EntityState.Detached;
                    }
                }
                else
                {
                    int[] testsarr = new int[] { 9, 10 };
                    for ( int i = 0; i <= 1; i++ )
                    {
                        var riskid = testsarr[i];
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = patientId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                        //_context.Entry(patient).State = EntityState.Detached;
                    }
                }
                //_context.Entry(patient).State = EntityState.Modified;
            }

            //_context.SaveChanges();

            return Ok();
        }

        [HttpPost("AssignDoctor")]
        public async Task<ActionResult> AssignDoctor(int doctorId)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var patient1 = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var doctor1 = await _context.Doctors.FirstOrDefaultAsync(x => x.Id == doctorId);
            var userId = patient1.Id;
            bool patientDoctor = await _context.DoctorPatients.AnyAsync(x => x.patientId == userId && x.doctorId == doctorId);
            DoctorPatient doctorPatient = new()
            {
                doctorId = doctorId,
                patientId = userId,
                //doctor = doctor1,
                //patient = patient1,
                DoctorNotes = "No Notes For Now !"
            };
            if ( patientDoctor == true )
            {
                return BadRequest("Doctor Already Assigned !");
            }
            else
            {
                await _context.DoctorPatients.AddAsync(doctorPatient);
                await _context.SaveChangesAsync();
            }

            return Ok("Doctor Assigned Successfully !");
        }

        [HttpGet("ViewDoctorsAssigned")]
        public async Task<ActionResult> ViewDoctorsAssigned()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var doctorsAssigned = await _context.Patients
                .Include(x => x.doctorPatients)
                .ThenInclude(x => x.doctor)
                .Where(o => o.Id == userId)
                .ToListAsync();
            return Ok(doctorsAssigned);
        }

        [HttpPost("AssignLab")]
        public async Task<ActionResult> AssignLab(int labId)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var patient1 = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var lab1 = await _context.Labs.FirstOrDefaultAsync(x => x.Id == labId);
            var userId = patient1.Id;
            bool patientLab = await _context.LabPatients.AnyAsync(x => x.PatientId == userId && x.LabId == labId);
            LabPatient labPatient = new()
            {
                LabId = labId,
                PatientId = userId,
                //doctor = doctor1,
                //patient = patient1,
                Results = "No Results For Now !"
            };
            if ( patientLab == true )
            {
                return BadRequest("Lab Already Assigned !");
            }
            else
            {
                await _context.LabPatients.AddAsync(labPatient);
                await _context.SaveChangesAsync();
            }

            return Ok("Lab Assigned Successfully !");
        }

        [HttpGet("ViewLabsAssigned")]
        public async Task<ActionResult> ViewLabsAssigned()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var labsAssigned = await _context.Patients
                .Include(x => x.labPatients)
                .ThenInclude(x => x.Lab)
                .Where(o => o.Id == userId)
                .ToListAsync();
            return Ok(labsAssigned);
        }

        [HttpPost]
        public async Task<ActionResult> OCR(IFormFile image)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            if ( image == null || image.Length == 0 )
            {
                return BadRequest("Image file is required.");
            }

            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);

            var request = new DetectDocumentTextRequest
            {
                Document = new Amazon.Textract.Model.Document
                {
                    Bytes = memoryStream
                }
            };

            try
            {
                var response = await _amazonTextract.DetectDocumentTextAsync(request);

                var extractedText = "";
                foreach ( var block in response.Blocks )
                {
                    if ( block.BlockType == BlockType.LINE )
                    {
                        extractedText += block.Text + Environment.NewLine;
                    }
                }

                if ( extractedText.Contains("Diabetes", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 1;
                    var patientRisk = new PatientTestsOrRisks
                    {
                        PatientId = userId,
                        TestTestsAndRisksId = riskid,
                    };
                    _context.PatientTestsAndRisks.Add(patientRisk);
                    _context.SaveChanges();
                }
                if ( extractedText.Contains("Blood Sugar Fasting", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 4;
                    var patientRisk = new PatientTestsOrRisks
                    {
                        PatientId = userId,
                        TestTestsAndRisksId = riskid,
                    };
                    _context.PatientTestsAndRisks.Add(patientRisk);
                    _context.SaveChanges();
                }
                if ( extractedText.Contains("2 HPP Blood Glucose", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 5;
                    var patientRisk = new PatientTestsOrRisks
                    {
                        PatientId = userId,
                        TestTestsAndRisksId = riskid,
                    };
                    _context.PatientTestsAndRisks.Add(patientRisk);
                    _context.SaveChanges();
                }
                if ( extractedText.Contains("HbA1c", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 6;
                    var patientRisk = new PatientTestsOrRisks
                    {
                        PatientId = userId,
                        TestTestsAndRisksId = riskid,
                    };
                    _context.PatientTestsAndRisks.Add(patientRisk);
                    _context.SaveChanges();
                }
                if ( extractedText.Contains("GFR Renal", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 7;
                    var patientRisk = new PatientTestsOrRisks
                    {
                        PatientId = userId,
                        TestTestsAndRisksId = riskid,
                    };
                    _context.PatientTestsAndRisks.Add(patientRisk);
                    _context.SaveChanges();
                }
                if ( extractedText.Contains("Fundus Examination", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 8;
                    var patientRisk = new PatientTestsOrRisks
                    {
                        PatientId = userId,
                        TestTestsAndRisksId = riskid,
                    };
                    _context.PatientTestsAndRisks.Add(patientRisk);
                    _context.SaveChanges();
                }
                if ( extractedText.Contains("TSH Free T4", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 9;
                    var patientRisk = new PatientTestsOrRisks
                    {
                        PatientId = userId,
                        TestTestsAndRisksId = riskid,
                    };
                    _context.PatientTestsAndRisks.Add(patientRisk);
                    _context.SaveChanges();
                }
                if ( extractedText.Contains("CBC", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 10;
                    var patientRisk = new PatientTestsOrRisks
                    {
                        PatientId = userId,
                        TestTestsAndRisksId = riskid,
                    };
                    _context.PatientTestsAndRisks.Add(patientRisk);
                    _context.SaveChanges();
                }
                if ( extractedText.Contains("Cholestrol", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 3;
                    var patientRisk = new PatientTestsOrRisks
                    {
                        PatientId = userId,
                        TestTestsAndRisksId = riskid,
                    };
                    _context.PatientTestsAndRisks.Add(patientRisk);
                    _context.SaveChanges();
                }

                return Ok("Tests Scanned Successfully !");
            }
            catch ( Exception ex )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}