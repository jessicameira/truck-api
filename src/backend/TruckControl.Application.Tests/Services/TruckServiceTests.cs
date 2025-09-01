using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using TruckControl.Application.DTOs;
using TruckControl.Application.Service;
using TruckControl.Data.Context;
using TruckControl.Domain.Entities;
using TruckControl.Domain.Enums;
using Xunit;

namespace TruckControl.Application.Tests.Services
{
    public class TruckServiceTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly Mock<IMapper> _mockMapper;
        private readonly TruckService _truckService;
        private readonly DbContextOptions<AppDbContext> _options;

        public TruckServiceTests()
        {
            _options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: $"TruckTestDb_{Guid.NewGuid()}")
                .Options;

            _context = new AppDbContext(_options);
            _mockMapper = new Mock<IMapper>();
            _truckService = new TruckService(_context, _mockMapper.Object);

            _context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        private TruckRequestDTO CreateValidTruckRequest()
        {
            return new TruckRequestDTO
            {
                Model = ModelEnum.FH,
                YearManufacture = 2024,
                Color = "Red",
                ChassisNumber = "CHS123456789",
                Plant = PlantEnum.BR
            };
        }

        private TruckResponseDTO CreateValidTruckResponse(int id)
        {
            return new TruckResponseDTO
            {
                Id = id,
                Model = ModelEnum.FH,
                YearManufacture = 2024,
                Color = "Red",
                ChassisNumber = "CHS123456789",
                Plant = PlantEnum.BR,
            };
        }

        private Truck CreateValidTruckEntity(int id)
        {
            return new Truck
            {
                Id = id,
                Model = ModelEnum.FH,
                YearManufacture = 2024,
                Color = "Red",
                ChassisNumber = "CHS123456789",
                Plant = PlantEnum.BR,
            };
        }

        [Fact]
        public async Task CreateAsync_ValidRequest_ReturnsTruckResponseDTO()
        {
            var request = CreateValidTruckRequest();
            var truckEntity = CreateValidTruckEntity(1);
            var expectedResponse = CreateValidTruckResponse(1);

            _mockMapper.Setup(m => m.Map<Truck>(request)).Returns(truckEntity);
            _mockMapper.Setup(m => m.Map<TruckResponseDTO>(truckEntity)).Returns(expectedResponse);

            var result = await _truckService.CreateAsync(request);

            Assert.NotNull(result);
            Assert.Equal(1, result.Id);

            var savedTruck = await _context.Trucks.FindAsync(1);
            Assert.NotNull(savedTruck);
        }

        [Fact]
        public async Task UpdateAsync_ExistingTruck_ReturnsUpdatedTruckResponseDTO()
        {
            var id = 1;
            var existingTruck = CreateValidTruckEntity(id);
            await _context.Trucks.AddAsync(existingTruck);
            await _context.SaveChangesAsync();

            var request = new TruckRequestDTO
            {
                Model = ModelEnum.FM,
                YearManufacture = 2025,
                Color = "Blue",
                ChassisNumber = "CHS987654321",
                Plant = PlantEnum.US
            };

            var updatedResponse = new TruckResponseDTO
            {
                Id = id,
                Model = ModelEnum.FM,
                YearManufacture = 2025,
                Color = "Blue",
                ChassisNumber = "CHS987654321",
                Plant = PlantEnum.US,
            };

            _mockMapper.Setup(m => m.Map(request, existingTruck));
            _mockMapper.Setup(m => m.Map<TruckResponseDTO>(existingTruck)).Returns(updatedResponse);

            var result = await _truckService.UpdateAsync(id, request);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
            Assert.Equal("Blue", result.Color);
            Assert.Equal(ModelEnum.FM, result.Model);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingTruck_ReturnsNull()
        {
            var id = 999;
            var request = CreateValidTruckRequest();

            var result = await _truckService.UpdateAsync(id, request);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingTruck_ReturnsTruckResponseDTO()
        {
            var id = 1;
            var truckEntity = CreateValidTruckEntity(id);
            await _context.Trucks.AddAsync(truckEntity);
            await _context.SaveChangesAsync();

            var expectedResponse = CreateValidTruckResponse(id);
            _mockMapper.Setup(m => m.Map<TruckResponseDTO>(truckEntity)).Returns(expectedResponse);

            var result = await _truckService.GetByIdAsync(id);

            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingTruck_ReturnsNull()
        {
            var id = 999;

            var result = await _truckService.GetByIdAsync(id);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllAsync_WithTrucks_ReturnsTruckList()
        {
            var trucks = new List<Truck>
            {
                CreateValidTruckEntity(1),
                CreateValidTruckEntity(2),
                CreateValidTruckEntity(3)
            };

            await _context.Trucks.AddRangeAsync(trucks);
            await _context.SaveChangesAsync();

            var expectedResponses = new List<TruckResponseDTO>
            {
                CreateValidTruckResponse(1),
                CreateValidTruckResponse(2),
                CreateValidTruckResponse(3)
            };

            _mockMapper.Setup(m => m.Map<IEnumerable<TruckResponseDTO>>(It.IsAny<List<Truck>>()))
                       .Returns(expectedResponses);

            var result = await _truckService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_EmptyDatabase_ReturnsEmptyList()
        {
            var emptyResponseList = new List<TruckResponseDTO>();
            _mockMapper.Setup(m => m.Map<IEnumerable<TruckResponseDTO>>(It.IsAny<List<Truck>>()))
                       .Returns(emptyResponseList);

            var result = await _truckService.GetAllAsync();

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task DeleteAsync_ExistingTruck_ReturnsTrue()
        {
            var id = 1;
            var truckToDelete = CreateValidTruckEntity(id);
            await _context.Trucks.AddAsync(truckToDelete);
            await _context.SaveChangesAsync();

            var result = await _truckService.DeleteAsync(id);

            Assert.True(result);

            var deletedTruck = await _context.Trucks.FindAsync(id);
            Assert.Null(deletedTruck);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingTruck_ReturnsFalse()
        {
            var id = 999;

            var result = await _truckService.DeleteAsync(id);

            Assert.False(result);
        }
    }
}