using VirtualClinic.Data;
using VirtualClinic.Entities;
using VirtualClinic.Interfaces;

namespace VirtualClinic.Services
{
    public class LabService : ILabService
    {
        private readonly DataContext _context;
        private readonly IGenericRepository<Lab> _repository;
        private readonly IUnitOfWork _unitOfWork;

        public LabService(DataContext context, IGenericRepository<Lab> repository, IUnitOfWork unitOfWork)
        {
            _context = context;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Lab> CreateLabAsync(string name, double price, double reviews, string phoneNumber, string? labDescript, int addressId, int geoLocationId)
        {
            var address = await _unitOfWork.Repositary<Address>().GetByIdAsync(addressId);
            var geoLocation = await _unitOfWork.Repositary<GeoLocation>().GetByIdAsync(geoLocationId);
            var labFinal = new Lab(name, price, reviews, phoneNumber, labDescript, address, geoLocation);
            _unitOfWork.Repositary<Lab>().Add(labFinal);
            await _unitOfWork.Complete();
            return labFinal;
        }
    }
}