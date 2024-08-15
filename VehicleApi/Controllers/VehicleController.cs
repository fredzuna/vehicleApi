using Microsoft.AspNetCore.Mvc;
using YourNamespace.Models;
using YourNamespace.Services;
using System.Collections.Generic;

namespace YourNamespace.Controllers
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

        // GET: api/Vehicle
        [HttpGet]
        public ActionResult<IEnumerable<VehicleFee>> Get()
        {
            var vehicleFees = _vehicleService.GetVehicleFees();
            return Ok(vehicleFees);
        }

        // GET: api/Vehicle/5
        [HttpGet("{id}")]
        public ActionResult<VehicleFee> Get(int id)
        {
            var vehicleFee = _vehicleService.GetVehicleFeeById(id);
            if (vehicleFee == null)
            {
                return NotFound();
            }
            return Ok(vehicleFee);
        }

        // POST: api/Vehicle
        [HttpPost]
        public ActionResult Post([FromBody] VehicleFee vehicleFee)
        {
            _vehicleService.CreateVehicleFee(vehicleFee);
            return CreatedAtAction(nameof(Get), new { id = vehicleFee.Id }, vehicleFee);
        }

        // PUT: api/Vehicle/5
        [HttpPut("{id}")]
        public ActionResult Put(int id, [FromBody] VehicleFee vehicleFee)
        {
            _vehicleService.UpdateVehicleFee(id, vehicleFee);
            return NoContent();
        }

        // DELETE: api/Vehicle/5
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            _vehicleService.DeleteVehicleFee(id);
            return NoContent();
        }
    }
}
