using TruckControl.Data.Context;

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TruckControl.Application.DTOs;
using TruckControl.Domain.Entities;

namespace TruckControl.Application.Service
{
    public interface ITruckService
    {
        Task<TruckResponseDTO> CreateAsync(TruckRequestDTO request);
        Task<TruckResponseDTO?> UpdateAsync(int id, TruckRequestDTO request);
        Task<TruckResponseDTO?> GetByIdAsync(int id);
        Task<IEnumerable<TruckResponseDTO>> GetAllAsync();
        Task<bool> DeleteAsync(int id);
    }
    public class TruckService : ITruckService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public TruckService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TruckResponseDTO> CreateAsync(TruckRequestDTO request)
        {
            var truck = _mapper.Map<Truck>(request);

            _context.Trucks.Add(truck);
            await _context.SaveChangesAsync();

            return _mapper.Map<TruckResponseDTO>(truck);
        }

        public async Task<TruckResponseDTO?> UpdateAsync(int id, TruckRequestDTO request)
        {
            var product = await _context.Trucks.FindAsync(id);
            if (product == null) return null;

            _mapper.Map(request, product);
            await _context.SaveChangesAsync();

            return _mapper.Map<TruckResponseDTO>(product);
        }

        public async Task<TruckResponseDTO?> GetByIdAsync(int id)
        {
            var truck = await _context.Trucks.FindAsync(id);
            return truck == null ? null : _mapper.Map<TruckResponseDTO>(truck);
        }

        public async Task<IEnumerable<TruckResponseDTO>> GetAllAsync()
        {
            var truck = await _context.Trucks.ToListAsync();
            return _mapper.Map<IEnumerable<TruckResponseDTO>>(truck);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var truck = await _context.Trucks.FindAsync(id);
            if (truck == null) return false;

            _context.Trucks.Remove(truck);
            await _context.SaveChangesAsync();
            return true;
        }
    }
    
}

