using TruckControl.Domain.Enums;

namespace TruckControl.Application.DTOs
{
    public class TruckRequestDTO
    {
        public int Id { get; set; }
        public required ModelEnum Model { get; set; }
        public string? ChassisNumber { get; set; }
        public string? Color { get; set; }
        public int YearManufacture { get; set; }
        public PlantEnum Plant { get; set; }
    }

    public class TruckResponseDTO
    {
        public int Id { get; set; }
        public required ModelEnum Model { get; set; }
        public string? ChassisNumber { get; set; }
        public string? Color { get; set; }
        public int YearManufacture { get; set; }
        public PlantEnum Plant { get; set; }
    }
    
    
}