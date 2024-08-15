using YourNamespace.Data;
using YourNamespace.Models;
using System.Collections.Generic;
using System.Linq;

namespace YourNamespace.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly AppDbContext _context;

        public VehicleService(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<VehicleFee> GetVehicleFees()
        {
            return _context.VehicleFees.ToList();
        }

        public VehicleFee GetVehicleFeeById(int id)
        {
            return _context.VehicleFees.FirstOrDefault(vf => vf.Id == id);
        }

        public void CreateVehicleFee(VehicleFee vehicleFee)
        {
            _context.VehicleFees.Add(vehicleFee);
            _context.SaveChanges();
        }

        public void UpdateVehicleFee(int id, VehicleFee vehicleFee)
        {
            var existingFee = _context.VehicleFees.FirstOrDefault(vf => vf.Id == id);
            if (existingFee != null)
            {
                existingFee.SpecialFeePercentage = vehicleFee.SpecialFeePercentage;
                existingFee.StorageFee = vehicleFee.StorageFee;
                _context.SaveChanges();
            }
        }

        public void DeleteVehicleFee(int id)
        {
            var vehicleFee = _context.VehicleFees.FirstOrDefault(vf => vf.Id == id);
            if (vehicleFee != null)
            {
                _context.VehicleFees.Remove(vehicleFee);
                _context.SaveChanges();
            }
        }
    }
}
