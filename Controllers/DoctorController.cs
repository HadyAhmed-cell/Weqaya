using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
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
            DoctorReviews reviews = new DoctorReviews()
            {
            };
            await _context.SaveChangesAsync();

            return Ok("Doctor Added Successfully !");
        }

        [HttpGet("CheckUserData")]
        public async Task<ActionResult> CheckUserData()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var Num = 0;
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            if ( user != null )
            {
                Num = 1;
            }
            return Ok(new { statusNum = Num });
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
            //var reviews = await _context.DoctorReviews.AnyAsync(p => p.DoctorId == userId);
            //var avgReviews = 0;

            //var avgReviews =await _context.DoctorReviews.Where(p => p.DoctorId == userId).AverageAsync(p => p.Reviews);

            if ( user == null )
            {
                return NotFound();
            }

            var result = new
            {
                user,
                Avg = _context.DoctorReviews
            .Where(r => r.DoctorId == userId)
            .Average(r => (double?)r.Reviews) ?? 0
            };
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
                           join ty in _context.DoctorPatients on tr.Id equals ty.patientId
                           where ty.doctorId == userId && ty.patientId == tr.Id
                           select new
                           {
                               tr.Name,
                               tr.Id,
                               PatientAppointment = ty.AppointmentDate.ToString(),
                               tr.Gender,
                               tr.Age,
                               tr.Weight,
                               tr.Height,
                               tr.PhoneNumber,
                               ty.StatusNum,
                               DoctorNotes = ty.DoctorNotes.Replace("\n", "").Replace("\r", "")
                           };

            return Ok(patients.ToList());
        }

        [HttpGet("ViewDoctorPatientsHistory")]
        public async Task<ActionResult> ViewPatientsHistory()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;
            var ids = from ptr in _context.DoctorHistories
                      where ptr.doctorId == userId
                      select ptr.patientId;

            var patients = from tr in _context.Patients
                           join ty in _context.DoctorHistories on tr.Id equals ty.patientId
                           where ty.doctorId == userId && ty.patientId == tr.Id
                           select new
                           {
                               tr.Name,
                               tr.Id,
                               PatientAppointment = ty.AppointmentDate.ToString(),
                               tr.Gender,
                               tr.Age,
                               tr.Weight,
                               tr.Height,
                               tr.PhoneNumber,
                               ty.StatusNum,
                               DoctorNotes = ty.DoctorNotes.Replace("\n", "").Replace("\r", "")
                           };

            return Ok(patients.ToList());
        }

        [HttpPost("DoctorNotes")]
        public async Task<ActionResult> PostDoctorNotes(int patientId, string notes)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var doctorPatient = await _context.DoctorPatients
               .SingleOrDefaultAsync(x => x.patientId == patientId && x.doctorId == userId);

            var doctorHistory = new DoctorHistory
            {
                patientId = patientId,
                doctorId = userId,
                StatusNum = 1,
                DoctorNotes = notes,
                AppointmentDate = doctorPatient.AppointmentDate
            };
            _context.DoctorPatients.Remove(doctorPatient);
            await _context.DoctorHistories.AddAsync(doctorHistory);
            await _context.SaveChangesAsync();
            return Ok("Notes Added Successfully !");
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePatientFromDoctorDb(int id)
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var doctorPatient = await _context.DoctorPatients
               .SingleOrDefaultAsync(x => x.doctorId == userId && x.patientId == id);

            var doctorHistory = new DoctorHistory
            {
                doctorId = userId,
                patientId = id,
                StatusNum = 2,
                AppointmentDate = doctorPatient.AppointmentDate,
                DoctorNotes = "No Notes Yet!"
            };
            _context.DoctorPatients.Remove(doctorPatient);
            await _context.DoctorHistories.AddAsync(doctorHistory);
            await _context.SaveChangesAsync();
            return Ok("Patient Cancelled Successfully !");
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
                    Avg = _context.DoctorReviews
                .Where(r => r.DoctorId == x.Id)
                .Average(r => (double?)r.Reviews) ?? 0,
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
            await _context.SaveChangesAsync();

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

        [HttpGet("ViewDoctorReviews")]
        public async Task<ActionResult> ViewDoctorReviews()
        {
            string email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _context.Doctors.FirstOrDefaultAsync(x => x.Email == email);
            var userId = user.Id;

            var reviews = _context.DoctorReviews.Include(x => x.Patient).Where(p => p.DoctorId == userId)
                .Select(o => new
                {
                    o.Patient.Name,
                    o.ReviewsComments,
                    o.Reviews
                });
            var avgReviews = await _context.DoctorReviews.AverageAsync(p => p.Reviews);

            var result = new
            {
                Avg = avgReviews,
                Reviews = reviews
            };

            return Ok(result);
        }
    }
}