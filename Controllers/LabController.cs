using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VirtualClinic.Data;
using VirtualClinic.Entities;
using VirtualClinic.Interfaces;

namespace VirtualClinic.Controllers
{
    public class LabController : BaseApiController
    {
        private readonly IGenericRepository<Lab> _labRepo;

        private readonly DataContext _context;
        private readonly ILabService _service;

        public LabController(IGenericRepository<Lab> labRepo, DataContext context, ILabService service)
        {
            _labRepo = labRepo;

            _context = context;
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult> GetAllLabs()
        {
            var labs = await _labRepo.GetAllAsync();

            return Ok(labs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetLabById(int id)
        {
            Lab lab = _context.Labs.Include(a => a.Address)
                .Include(g => g.GeoLocation)
                .FirstOrDefault(l => l.Id == id);
            if ( lab == null )
            {
                return BadRequest();
            }

            return Ok(lab);
        }

        [HttpPost]
        public async Task<ActionResult<Lab>> CreateLab([FromQuery] Lab lab, int addressId, int geoLocation)
        {
            var labToCreate = await _service.CreateLabAsync(lab.Name, lab.Price, lab.Reviews, lab.PhoneNumber, lab.LabDescript, addressId, geoLocation);
            if ( labToCreate == null )
            {
                return BadRequest();
            }

            return Ok(labToCreate);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateLab([FromForm] Lab lab, int id)
        {
            if ( id != lab.Id )
            {
                return BadRequest();
            }
            if ( ModelState.IsValid )
            {
                _context.Labs.Update(lab);
                await _context.SaveChangesAsync();
            }

            return Ok(lab);
        }
    }
}