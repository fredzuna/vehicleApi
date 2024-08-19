using Moq;
using Microsoft.AspNetCore.Mvc;
using VehicleApi.Controllers;
using VehicleApi.Services;
using VehicleApi.Dtos;
using VehicleApi.Models;

namespace VehicleApi.Tests.Controllers
{
    public class VehicleControllerTests
    {
        private readonly Mock<IVehicleService> _mockVehicleService;
        private readonly VehicleController _controller;

        public VehicleControllerTests()
        {
            _mockVehicleService = new Mock<IVehicleService>();
            _controller = new VehicleController(_mockVehicleService.Object);
        }

        [Fact]
        public void CalculateVehiclePrice_ReturnsOkResult_WhenCalculationIsSuccessful()
        {
            // Arrange
            var vehicleBasePrice = new VehicleBasePriceDto
            {
                BasePrice = 10000,
                Type = VehicleTypeEnum.Common
            };

            var expectedTotalPrice = new VehicleTotalPriceDto
            {
                TotalPrice = 13000,  // Total of all charges added to base price
                VehicleCharges = new VehicleChargesDto
                {
                    Basic = 1000,
                    Special = 1500,
                    Association = 500,
                    Storage = 0
                }
            };

            _mockVehicleService.Setup(service => service.GetCalculateVehiclePrice(vehicleBasePrice))
                .Returns(expectedTotalPrice);
            
            var result = _controller.CalculateVehiclePrice(vehicleBasePrice);
            
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<VehicleTotalPriceDto>(okResult.Value);
            Assert.Equal(expectedTotalPrice.TotalPrice, returnValue.TotalPrice);
            Assert.Equal(expectedTotalPrice.VehicleCharges.Basic, returnValue.VehicleCharges.Basic);
            Assert.Equal(expectedTotalPrice.VehicleCharges.Special, returnValue.VehicleCharges.Special);
            Assert.Equal(expectedTotalPrice.VehicleCharges.Association, returnValue.VehicleCharges.Association);
            Assert.Equal(expectedTotalPrice.VehicleCharges.Storage, returnValue.VehicleCharges.Storage);
        }

        [Fact]
        public void CalculateVehiclePrice_ReturnsInternalServerError_WhenExceptionIsThrown()
        {
            // Arrange
            var vehicleBasePrice = new VehicleBasePriceDto
            {
                BasePrice = 10000,
                Type = VehicleTypeEnum.Common
            };

            _mockVehicleService.Setup(service => service.GetCalculateVehiclePrice(vehicleBasePrice))
                .Throws(new Exception("Unexpected error."));

            // Act
            var result = _controller.CalculateVehiclePrice(vehicleBasePrice);

            // Assert
            var statusCodeResult = Assert.IsType<ObjectResult>(result)!; // Using null-forgiving operator
            Assert.Equal(500, statusCodeResult.StatusCode);
            Assert.Contains("An internal server error occurred. Please try again later.", statusCodeResult.Value.ToString());
        }
    }
}
