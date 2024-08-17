using VehicleApi.Dtos;
using VehicleApi.Models;

public class VehicleFeeCalculator
{
  private readonly VehicleFee _vehicleFee;
  private readonly List<AssociationFee> _associationFees;

  private readonly BasicBuyerFee _basicBuyerFee;

  public VehicleFeeCalculator(VehicleFee vehicleFee, BasicBuyerFee basicBuyerFee, List<AssociationFee> associationFees)
  {
    _vehicleFee = vehicleFee ?? throw new ArgumentNullException(nameof(vehicleFee));
    _associationFees = associationFees ?? throw new ArgumentNullException(nameof(associationFees));
    _basicBuyerFee = basicBuyerFee ?? throw new ArgumentNullException(nameof(basicBuyerFee));
  }

  public decimal CalculateTotalPrice(decimal basePrice)
  {
    var basicBuyerPrice = _basicBuyerFee.CalculateBasicBuyerPrice(basePrice);
    var specialPrice = CalculateSpecialFee(basePrice);
    var associationPrice = CalculateAssociationFee(basePrice);
    var storagePrice = _vehicleFee.StorageFee;

    return basePrice + basicBuyerPrice + specialPrice + associationPrice + storagePrice;
  }

  public VehicleChargesDto GetVehicleCharges(decimal basePrice)
  {
    return new VehicleChargesDto
    {
      Basic = _basicBuyerFee.CalculateBasicBuyerPrice(basePrice),
      Special = CalculateSpecialFee(basePrice),
      Association = CalculateAssociationFee(basePrice),
      Storage = _vehicleFee.StorageFee
    };
  }

  private decimal CalculateSpecialFee(decimal basePrice)
  {
    return basePrice * (_vehicleFee.SpecialFeePercentage / 100);
  }

  private decimal CalculateAssociationFee(decimal basePrice)
  {
    foreach (var fee in _associationFees)
    {
      if (basePrice >= fee.StartAmount &&
          (fee.EndAmount == null || basePrice <= fee.EndAmount.Value))
      {
        return fee.Amount;
      }
    }
    return 0;
  }
}
