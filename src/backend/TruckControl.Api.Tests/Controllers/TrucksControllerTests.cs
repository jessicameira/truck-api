using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Moq;
using FluentAssertions;
using Xunit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TruckControl.Application.DTOs;
using TruckControl.Application.Service;

namespace TruckControl.API.Tests.Unit.Controllers
{
    public class TrucksControllerTests
    {
        private readonly Mock<ITruckService> _serviceMock;
        private readonly TrucksController _controller;

        public TrucksControllerTests()
        {
            _serviceMock = new Mock<ITruckService>();
            _controller = new TrucksController(_serviceMock.Object);
            
            var user = new ClaimsPrincipal(new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, "testuser")
            }, "mock"));
            
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };
        }

        private TruckRequestDTO CreateValidRequestDTO()
        {
            return new TruckRequestDTO
            {
                Id = 1,
                Model = Domain.Enums.ModelEnum.FH,
                ChassisNumber = "1HGCM82633A123456",
                Color = "Black",
                YearManufacture = 2025,
                Plant = Domain.Enums.PlantEnum.BR
                        
            };
        }

        private TruckResponseDTO CreateValidResponseDTO(int id = 1)
        {
            return new TruckResponseDTO
            {
                Id = 1,
                Model = Domain.Enums.ModelEnum.FH,
                ChassisNumber = "1HGCM82633A123456",
                Color = "Black",
                YearManufacture = 2025,
                Plant = Domain.Enums.PlantEnum.BR
            };
        }

        [Fact]
        public void Controller_Should_HaveAuthorizeAttribute()
        {
            var attribute = typeof(TrucksController)
                .GetCustomAttributes(typeof(AuthorizeAttribute), true)
                .FirstOrDefault() as AuthorizeAttribute;

            attribute.Should().NotBeNull();
        }

        [Fact]
        public async Task Create_WithValidRequest_ShouldReturnCreated()
        {
            // Arrange
            var request = CreateValidRequestDTO();
            var expectedResponse = CreateValidResponseDTO(1);
            
            _serviceMock.Setup(x => x.CreateAsync(request))
                       .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Create(request);

            // Assert - Correção principal aqui
            var actionResult = result.Result;
            actionResult.Should().BeOfType<CreatedAtActionResult>();
            
            var createdResult = actionResult as CreatedAtActionResult;
            createdResult.ActionName.Should().Be(nameof(_controller.GetById));
            createdResult.RouteValues["id"].Should().Be(expectedResponse.Id);
            createdResult.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Create_WithNullRequest_ShouldReturnBadRequest()
        {
            // Arrange
            TruckRequestDTO request = null;

            // Act
            var result = await _controller.Create(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Update_WithValidData_ShouldReturnOk()
        {
            // Arrange
            int id = 1;
            var request = CreateValidRequestDTO();
            var expectedResponse = CreateValidResponseDTO(id);
            
            _serviceMock.Setup(x => x.UpdateAsync(id, request))
                       .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.Update(id, request);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result.Result;
            okResult.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task Update_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            int id = 999;
            var request = CreateValidRequestDTO();
            
            _serviceMock.Setup(x => x.UpdateAsync(id, request))
                       .ReturnsAsync((TruckResponseDTO)null);

            // Act
            var result = await _controller.Update(id, request);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetById_WithExistingId_ShouldReturnOk()
        {
            // Arrange
            int id = 1;
            var expectedResponse = CreateValidResponseDTO(id);
            
            _serviceMock.Setup(x => x.GetByIdAsync(id))
                       .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result.Result;
            okResult.Value.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetById_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            int id = 999;
            
            _serviceMock.Setup(x => x.GetByIdAsync(id))
                       .ReturnsAsync((TruckResponseDTO)null);

            // Act
            var result = await _controller.GetById(id);

            // Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetAll_ShouldReturnOkWithList()
        {
            // Arrange
            var expectedList = new List<TruckResponseDTO>
            {
                CreateValidResponseDTO(1),
                CreateValidResponseDTO(2)
            };
            
            _serviceMock.Setup(x => x.GetAllAsync())
                       .ReturnsAsync(expectedList);

            // Act
            var result = await _controller.GetAll();

            // Assert
            result.Result.Should().BeOfType<OkObjectResult>();
            var okResult = (OkObjectResult)result.Result;
            okResult.Value.Should().BeEquivalentTo(expectedList);
        }

        [Fact]
        public async Task Delete_WithExistingId_ShouldReturnNoContent()
        {
            // Arrange
            int id = 1;
            
            _serviceMock.Setup(x => x.DeleteAsync(id))
                       .ReturnsAsync(true);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Delete_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            int id = 999;
            
            _serviceMock.Setup(x => x.DeleteAsync(id))
                       .ReturnsAsync(false);

            // Act
            var result = await _controller.Delete(id);

            // Assert
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_WithServiceException_ShouldReturnBadRequest()
        {
            // Arrange
            var request = CreateValidRequestDTO();
            
            _serviceMock.Setup(x => x.CreateAsync(request))
                       .ThrowsAsync(new ArgumentException("Invalid data"));

            // Act
            var result = await _controller.Create(request);

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void Create_WithInvalidModelState_ShouldReturnBadRequest()
        {
            // Arrange
            var request = CreateValidRequestDTO();
            _controller.ModelState.AddModelError("Model", "Model is required");

            // Act
            var result = _controller.Create(request).Result; // Síncrono para teste de ModelState

            // Assert
            result.Result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}