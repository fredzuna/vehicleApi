using YourNamespace.Models;
using System.Collections.Generic;

namespace YourNamespace.Services
{
    public interface IVehicleService
    {
        IEnumerable<VehicleFee> GetVehicleFees();
        VehicleFee GetVehicleFeeById(int id);
        void CreateVehicleFee(VehicleFee vehicleFee);
        void UpdateVehicleFee(int id, VehicleFee vehicleFee);
        void DeleteVehicleFee(int id);
    }
}
