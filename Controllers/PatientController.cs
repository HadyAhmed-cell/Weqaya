using AForge.Imaging.Filters;
using Amazon;
using Amazon.Textract;
using Amazon.Textract.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging.Signing;
using System.Drawing;
using System.Drawing.Imaging;
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

        [HttpGet("GetDoctorById")]
        public async Task<ActionResult> GetDoctorById(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if ( doctor == null )
            {
                return NotFound();
            }

            var appointments = await _context.Appointments
                .Where(a => a.DoctorId == id)
                .Select(a => a.AppointmentDateTime.ToString())
                .ToListAsync();

            var result = new
            {
                doctor.Name,
                doctor.Id,
                doctor.DoctorInfo,
                doctor.Price,
                doctor.Speciality,
                doctor.SubSpeciatlity,
                doctor.Education,
                doctor.TimeTo,
                doctor.TimeFrom,
                doctor.Duration,
                doctor.Area,
                doctor.StreetAddress,
                doctor.Photo,

                appointments
            };

            return Ok(result);
        }

        [HttpGet("GetLabById")]
        public async Task<ActionResult> GetLabById(int id)
        {
            var lab = await _context.Labs.FindAsync(id);

            if ( lab == null )
            {
                return NotFound();
            }

            var labTests = await (from lt in _context.LabsTestsAndRisks
                                  join t in _context.testsAndRisks on lt.TestsAndRisksId equals t.Id
                                  where lt.LabId == id
                                  select new
                                  {
                                      t.TestsOrRisks,
                                      lt.Price
                                  }).ToListAsync();

            var result = new
            {
                lab.Name,
                lab.Id,
                lab.Area,
                lab.Photo,
                lab.LabDescript,
                labTests
            };

            return Ok(result);
        }

        [HttpGet("GetPatientTests")]
        public async Task<ActionResult> GetPatientTests()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            var testsIds = from ptr in _context.PatientTestsAndRisks
                           where ptr.PatientId == userId
                           select ptr.TestTestsAndRisksId;

            var tests = from tr in _context.testsAndRisks
                        where testsIds.Contains(tr.Id)
                        select tr.TestsOrRisks;
            return Ok(tests);
        }

        [HttpPost("AddPatient")]
        public async Task<ActionResult> CreatePatient(Patient patient)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            patient.Email = email;
            await _context.Patients.AddAsync(patient);

            await _context.SaveChangesAsync();

            return Ok(patient);
        }

        [HttpPut("EditPatientData")]
        public async Task<ActionResult> EditPatient(Patient patient)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            patient.Email = email;
            user.Id = userId;
            user.Name = patient.Name;
            user.Weight = patient.Weight;
            user.Height = patient.Height;
            user.PhoneNumber = patient.PhoneNumber;
            user.DiabetesRelatives = patient.DiabetesRelatives;
            user.RelativesWithHeartAttacksOrHighColestrol = patient.RelativesWithHeartAttacksOrHighColestrol;
            user.Smoking = patient.Smoking;
            user.MedicineForDiabetesOrPressure = patient.MedicineForDiabetesOrPressure;
            user.HighPressure = patient.HighPressure;
            user.Diabetes = patient.Diabetes;

            await _context.SaveChangesAsync();
            return Ok("Patient Updated");
        }

        [HttpGet("GetPatientProfile")]
        public async Task<ActionResult> GetPatientProfile()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Id == userId);
            return Ok(user);
        }

        [HttpPut("Recommendation")]
        public async Task<ActionResult> RecommendTests()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var patient = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = patient.Id;

            var patientId = userId;
            var patientTests = _context.PatientTestsAndRisks
    .Where(x => x.PatientId == userId);

            if ( patientTests != null )
            {
                _context.PatientTestsAndRisks.RemoveRange(patientTests);
                _context.SaveChanges();
            }

            if ( patient.Age >= 45 && (patient.Diabetes.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) || patient.DiabetesRelatives.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase)) )
            {
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 1 && x.PatientId == userId) )
                {
                    if ( patient.Weight >= 90 || patient.HighPressure.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) || patient.MedicineForDiabetesOrPressure.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) )
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
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 3 && x.PatientId == userId) )
                {
                    if ( patient.RelativesWithHeartAttacksOrHighColestrol.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) || patient.Diabetes.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) || patient.Smoking.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) )
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
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 3 && x.PatientId == userId) )
                {
                    if ( patient.RelativesWithHeartAttacksOrHighColestrol.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) || patient.Diabetes.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) || patient.Smoking.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) )
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
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 3 && x.PatientId == userId) )
                {
                    if ( patient.RelativesWithHeartAttacksOrHighColestrol.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) || patient.Diabetes.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) || patient.Smoking.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) )
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

            if ( patient.Diabetes.Equals("True".Trim(), StringComparison.OrdinalIgnoreCase) )
            {
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 6 && x.PatientId == userId) )
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
                if ( !_context.PatientTestsAndRisks.Any(x => x.TestTestsAndRisksId == 6 && x.PatientId == userId) )
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
        public async Task<ActionResult> AssignDoctor(int doctorId, [FromBody] string appointments)
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
                AppointmentDate = DateTime.Parse(appointments),
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

        [HttpGet("GetDoctorConfirmation")]
        public async Task<ActionResult> ViewDoctorConfirmation(int docId)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            var confirmDoctor = from tr in _context.Doctors
                                from ty in _context.DoctorPatients
                                where tr.Id == docId && ty.patientId == userId && ty.doctorId == docId
                                group tr by tr.Id into g
                                select new
                                {
                                    DoctorAssigned = g.FirstOrDefault().Name,
                                    Appointment = g.FirstOrDefault().doctorPatients.FirstOrDefault().AppointmentDate.ToString(),
                                    Specialization = g.FirstOrDefault().Speciality,
                                    Price = g.FirstOrDefault().Price,
                                    Area = g.FirstOrDefault().Area,
                                };

            return Ok(confirmDoctor);
        }

        [HttpGet("ViewDoctorsAssigned")]
        public async Task<ActionResult> ViewDoctorsAssigned()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var ids = from ptr in _context.DoctorPatients
                      where ptr.patientId == userId
                      select ptr.doctorId;

            var doctors = from tr in _context.Doctors
                          from ty in _context.DoctorPatients
                          where ids.Contains(tr.Id) && ty.patientId == userId && ids.Contains(ty.doctorId)
                          group tr by tr.Id into g
                          select new
                          {
                              DoctorAssigned = g.FirstOrDefault().Name,
                              Appointment = g.FirstOrDefault().doctorPatients.FirstOrDefault().AppointmentDate.ToString(),
                              Specialization = g.FirstOrDefault().Speciality,
                              SubSpeciality = g.FirstOrDefault().SubSpeciatlity
                          };

            return Ok(doctors);
        }

        [HttpPost("AssignLab")]
        public async Task<ActionResult> AssignLab(int labId, int testId, int price)
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
                TestId = testId,
                Price = price,
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

            var ids = from ptr in _context.LabPatients
                      where ptr.PatientId == userId
                      select ptr.LabId;

            var results = from lp in _context.LabPatients
                          join t in _context.testsAndRisks on lp.TestId equals t.Id
                          join l in _context.Labs on lp.LabId equals l.Id
                          where lp.PatientId == user.Id
                          select new
                          {
                              lp.Price,
                              TestName = t.TestsOrRisks,
                              LabName = l.Name
                          };
            return Ok(results);
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

            var sourceImage = new Bitmap(memoryStream);

            // Apply image enhancement techniques
            var grayscaleFilter = new Grayscale(0.2125, 0.7154, 0.0721);
            var gaussianFilter = new GaussianBlur(2, 7);
            var contrastFilter = new ContrastStretch();
            var sharpenFilter = new Sharpen();

            var processedImage = grayscaleFilter.Apply(sourceImage);
            processedImage = gaussianFilter.Apply(processedImage);
            contrastFilter.ApplyInPlace(processedImage);
            sharpenFilter.ApplyInPlace(processedImage);

            // Save the processed image back to the memory stream
            var processedMemoryStream = new MemoryStream();
            processedImage.Save(processedMemoryStream, ImageFormat.Jpeg);
            processedMemoryStream.Position = 0;

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
                    bool LabTests = await _context.PatientTestsAndRisks.AnyAsync(x => x.PatientId == userId && x.TestTestsAndRisksId == riskid);
                    if ( !LabTests )
                    {
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = userId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                    }
                }
                if ( extractedText.Contains("Blood Sugar Fasting", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 4;
                    bool LabTests = await _context.PatientTestsAndRisks.AnyAsync(x => x.PatientId == userId && x.TestTestsAndRisksId == riskid);
                    if ( !LabTests )
                    {
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = userId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                    }
                }
                if ( extractedText.Contains("2 HPP Blood Glucose", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 5;
                    bool LabTests = await _context.PatientTestsAndRisks.AnyAsync(x => x.PatientId == userId && x.TestTestsAndRisksId == riskid);
                    if ( !LabTests )
                    {
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = userId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                    }
                }
                if ( extractedText.Contains("HbA1c", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 6;
                    bool LabTests = await _context.PatientTestsAndRisks.AnyAsync(x => x.PatientId == userId && x.TestTestsAndRisksId == riskid);
                    if ( !LabTests )
                    {
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = userId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                    }
                }
                if ( extractedText.Contains("GFR Renal", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 7;
                    bool LabTests = await _context.PatientTestsAndRisks.AnyAsync(x => x.PatientId == userId && x.TestTestsAndRisksId == riskid);
                    if ( !LabTests )
                    {
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = userId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                    }
                }
                if ( extractedText.Contains("Fundus Examination", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 8;
                    bool LabTests = await _context.PatientTestsAndRisks.AnyAsync(x => x.PatientId == userId && x.TestTestsAndRisksId == riskid);
                    if ( !LabTests )
                    {
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = userId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                    }
                }
                if ( extractedText.Contains("TSH Free T4", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 9;
                    bool LabTests = await _context.PatientTestsAndRisks.AnyAsync(x => x.PatientId == userId && x.TestTestsAndRisksId == riskid);
                    if ( !LabTests )
                    {
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = userId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                    }
                }
                if ( extractedText.Contains("CBC", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 10;
                    bool LabTests = await _context.PatientTestsAndRisks.AnyAsync(x => x.PatientId == userId && x.TestTestsAndRisksId == riskid);
                    if ( !LabTests )
                    {
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = userId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                    }
                }
                if ( extractedText.Contains("Cholestrol", StringComparison.OrdinalIgnoreCase) )
                {
                    var riskid = 3;
                    bool LabTests = await _context.PatientTestsAndRisks.AnyAsync(x => x.PatientId == userId && x.TestTestsAndRisksId == riskid);
                    if ( !LabTests )
                    {
                        var patientRisk = new PatientTestsOrRisks
                        {
                            PatientId = userId,
                            TestTestsAndRisksId = riskid,
                        };
                        _context.PatientTestsAndRisks.Add(patientRisk);
                        _context.SaveChanges();
                    }
                }

                return Ok("Tests Scanned Successfully");
            }
            catch ( Exception ex )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("ViewPatientTests")]
        public async Task<ActionResult> ViewPatientTests()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var testIds = await (from p in _context.PatientTestsAndRisks
                                 where p.PatientId == userId
                                 select p.TestTestsAndRisksId).ToListAsync();

            var tests = await (from l in _context.PatientTestsAndRisks
                               join t in _context.testsAndRisks on l.TestTestsAndRisksId equals t.Id
                               where l.PatientId == userId && testIds.Contains(l.TestTestsAndRisksId)
                               select new
                               {
                                   TestName = t.TestsOrRisks,
                                   // Add additional properties from the testsAndRisks table as needed
                               }).ToListAsync();

            return Ok(tests);
        }

        [HttpGet("GetDatesAvailable")]
        public async Task<ActionResult> GetDates(int id)
        {
            var datesAvailable = from ptr in _context.Appointments
                                 where ptr.DoctorId == id
                                 select ptr.AppointmentDateTime.ToString();

            return Ok(datesAvailable);
        }

        [HttpGet("SearchedLabsWithPatientTests")]
        public async Task<ActionResult> SearchLabs()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            // Retrieve the tests for the patient
            var patientTests = _context.PatientTestsAndRisks
                .Where(pt => pt.PatientId == userId)
                .Select(pt => pt.TestTestsAndRisksId)
                .ToList();

            // Retrieve the labs that have the same tests
            var labsWithTotalPrice = _context.LabsTestsAndRisks
                .Where(ltr => patientTests.Contains(ltr.TestsAndRisksId))
                .GroupBy(ltr => ltr.Lab)
                .Select(g => new
                {
                    LabName = g.Key.Name,
                    TotalPrice = g.Sum(ltr => ltr.Price)
                })
                .ToList();

            if ( !labsWithTotalPrice.Any() )
            {
                return NotFound();
            }

            return Ok(labsWithTotalPrice);
        }

        [HttpDelete("DeleteAssignedDoctor")]
        public async Task<ActionResult> DeleteAssignedDoctor(int id)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Patients.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var doctorPatient = _context.DoctorPatients
                .Where(x => x.patientId == userId && x.doctorId == id);

            _context.DoctorPatients.RemoveRange(doctorPatient);
            await _context.SaveChangesAsync();

            return Ok("Doctor Deleted Successfully !");
        }
    }
}