using VirtualClinic.Entities;

namespace VirtualClinic.Interfaces
{
    public interface ILabService
    {
        Task<Lab> CreateLabAsync(string name, double price, double reviews, string phoneNumber, string? labDescript, int addressId, int geoLocationId);
    }
}