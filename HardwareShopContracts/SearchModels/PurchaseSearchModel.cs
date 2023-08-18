
using HardwareShopDataModels.Enums;

namespace HardwareShopContracts.SearchModels
{
    public class PurchaseSearchModel
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public PurchaseStatus? PurchaseStatus { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
