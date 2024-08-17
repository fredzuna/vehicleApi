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
            var vehicleTotalPrice = _vehicleService.GetCalculateVehiclePrice(vehiclePrice);
            return Ok(vehicleTotalPrice);
        }       
    }
}
