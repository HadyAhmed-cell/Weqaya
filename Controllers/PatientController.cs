using Amazon;
using Amazon.Textract;
using Amazon.Textract.Model;
using IronOcr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Security.Claims;
using Tesseract;
using VirtualClinic.Data;
using VirtualClinic.Entities;
using Image = System.Drawing.Image;
using Rectangle = System.Drawing.Rectangle;

namespace VirtualClinic.Controllers
{
    public class PatientController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly IAmazonTextract _amazonTextract;
        private readonly IWebHostEnvironment _environment;

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
            if ( image == null || image.Length == 0 )
            {
                return BadRequest("Image file is required.");
            }

            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);

            var request = new DetectDocumentTextRequest
            {
                Document = new Document
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

                if ( extractedText.Contains("CBC") )
                {
                    await Console.Out.WriteLineAsync("U have to CBC Test");
                }

                return Ok(extractedText);
            }
            catch ( Exception ex )
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }

            //var stream = new MemoryStream();

            //await file.CopyToAsync(stream);
            //var img = Image.FromStream(stream);
            //var ocr = new IronTesseract();
            //var input = new OcrInput(img);

            //input.Deskew();

            //var result = ocr.Read(input);
            //await Console.Out.WriteLineAsync(result.Text);
            //return Ok(result.Text);
            //    if ( file == null || file.Length == 0 )
            //    {
            //        return BadRequest("No file was uploaded.");
            //    }

            //    var stream = new MemoryStream();

            //    await file.CopyToAsync(stream);
            //    var img1 = stream.ToArray();

            //    try
            //    {
            //        using var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.TesseractAndLstm);
            //        using var img = Pix.LoadFromMemory(img1);
            //        img.Deskew();
            //        img.ConvertRGBToGray();
            //        using var page = engine.Process(img);
            //        var text = page.GetText();

            //        return Ok(page.GetText());
            //    }
            //    catch ( Exception ex )
            //    {
            //        return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            //    }
            //}

            //public string ExtractTextFromImage(IFormFile file)
            //{
            //    string extractedText = string.Empty;

            //    using ( var engine = new TesseractEngine("./tessdata", "eng", EngineMode.Default) )
            //    {
            //        using ( var imgStream = new MemoryStream() )
            //        {
            //            file.CopyTo(imgStream);
            //            imgStream.Position = 0;

            //            using ( var img = Pix.LoadFromMemory(imgStream.ToArray()) )
            //            {
            //                using ( var page = engine.Process(img) )
            //                {
            //                    extractedText = page.GetText();
            //                }
            //            }
            //        }
            //    }

            //    return extractedText;
            //}
        }
    }
}