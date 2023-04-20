using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualClinic.Data;
using VirtualClinic.Entities;

namespace VirtualClinic.Controllers
{
    public class PatientController : BaseApiController
    {
        private readonly DataContext _context;

        public PatientController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetAllPatients()
        {
            var patients = await _context.Patients

                .ToListAsync();
            return Ok(patients);
        }

        [HttpGet("{GetPatientById}")]
        public async Task<ActionResult> GetPatientById(int GetPatientById)
        {
            var patient = await _context.Patients
                .Include(p => p.PatientTestsAndRisks).ThenInclude(r => r.TestsAndRisks)
                .FirstOrDefaultAsync(x => x.Id == GetPatientById);

            return Ok(patient);
        }

        [HttpPost]
        public ActionResult CreatePatient([FromQuery] Patient patient)
        {
            _context.Patients.Add(patient);

            return Ok(patient);
        }

        [HttpPut]
        public async Task<ActionResult> EditPatient([FromQuery] Patient patient)
        {
            _context.Patients.Update(patient);
            await _context.SaveChangesAsync();
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

        //[HttpPut("{EditRecommendationId}")]
        //public async Task<ActionResult> EditRecommendation(int EditRecommendationId)
        //{
        //    Patient patient = _context.Patients
        //        .Include(p => p.PatientTestsAndRisks)
        //        .FirstOrDefault(x => x.Id == EditRecommendationId);

        //    var patientId = patient.Id;

        //    _context.PatientTestsAndRisks.RemoveRange(patient.PatientTestsAndRisks);
        //    _context.SaveChanges();

        //    if ( patient.Age >= 45 )
        //    {
        //        if ( patient.Weight >= 90 || patient.HighPressure == true || patient.MedicineForDiabetesOrPressure == true || patient.Diabetes == true )
        //        {
        //            var riskid = 1;
        //            var patientRisk = new PatientTestsOrRisks
        //            {
        //                PatientId = patientId,
        //                TestTestsAndRisksId = riskid,
        //            };
        //            _context.PatientTestsAndRisks.Add(patientRisk);
        //        }
        //    }
        //    if ( patient.Gender == "Male" && (patient.Age >= 45 && patient.Age <= 65) )
        //    {
        //        if ( patient.RelativesWithHeartAttacksOrHighColestrol == true || patient.Diabetes == true || patient.Smoking == true )
        //        {
        //            var riskid = 3;
        //            var patientRisk = new PatientTestsOrRisks
        //            {
        //                PatientId = patientId,
        //                TestTestsAndRisksId = riskid,
        //            };
        //            _context.PatientTestsAndRisks.Add(patientRisk);
        //        }
        //    }

        //    if ( patient.Gender == "Female" && (patient.Age >= 55 && patient.Age <= 65) )
        //    {
        //        if ( patient.RelativesWithHeartAttacksOrHighColestrol == true || patient.Diabetes == true || patient.Smoking == true )
        //        {
        //            var riskid = 3;
        //            var patientRisk = new PatientTestsOrRisks
        //            {
        //                PatientId = patientId,
        //                TestTestsAndRisksId = riskid,
        //            };
        //            _context.PatientTestsAndRisks.Add(patientRisk);
        //        }
        //    }

        //    if ( patient.Age >= 65 )
        //    {
        //        if ( patient.RelativesWithHeartAttacksOrHighColestrol == true || patient.Diabetes == true || patient.Smoking == true )
        //        {
        //            var riskid = 3;
        //            var patientRisk = new PatientTestsOrRisks
        //            {
        //                PatientId = patientId,
        //                TestTestsAndRisksId = riskid,
        //            };
        //            _context.PatientTestsAndRisks.Add(patientRisk);
        //        }
        //    }

        //    if ( patient.Diabetes == true )
        //    {
        //        for ( int i = 4; i <= 8; i++ )
        //        {
        //            var riskid = i;
        //            var patientRisk = new PatientTestsOrRisks
        //            {
        //                PatientId = patientId,
        //                TestTestsAndRisksId = riskid,
        //            };
        //            _context.PatientTestsAndRisks.Add(patientRisk);
        //        }
        //    }
        //    if ( patient.Weight >= 90 )
        //    {
        //        int[] testsarr = new int[] { 6, 3, 9, 10 };
        //        for ( int i = 0; i <= 3; i++ )
        //        {
        //            var riskid = testsarr[i];
        //            var patientRisk = new PatientTestsOrRisks
        //            {
        //                PatientId = patientId,
        //                TestTestsAndRisksId = riskid,
        //            };
        //            _context.PatientTestsAndRisks.Add(patientRisk);
        //        }
        //    }

        //    _context.SaveChanges();

        //    return Ok();
        //}
    }
}