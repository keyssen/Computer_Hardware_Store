using HardwareShopDataModels.Enums;

namespace HardwareShopDataModels.Models
{
    public interface IPurchaseModel : IId
    {
        double Sum { get; }

        PurchaseStatus PurchaseStatus { get; }

        DateTime? DatePurchase { get; }

        int UserId { get; }

        Dictionary<int, (IGoodModel, int)> PurchaseGoods { get; }

        Dictionary<int, (IBuildModel, int)> PurchaseBuilds { get; }
    }
}
