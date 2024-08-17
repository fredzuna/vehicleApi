using Microsoft.AspNetCore.Mvc;
using VehicleApi.Services;
using VehicleApi.Dtos;
using System.Collections.Generic;

namespace VehicleApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        // POST: api/Vehicle
        [HttpPost("calculate-price")]
        public ActionResult CalculateVehiclePrice([FromBody] VehicleBasePriceDto vehiclePrice)
        {
            try
            {
                var vehicleTotalPrice = _vehicleService.GetCalculateVehiclePrice(vehiclePrice);
                return Ok(vehicleTotalPrice);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An internal server error occurred. Please try again later. " + ex.Message);
            }
        }
    }
}
