using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;
using System.ComponentModel;
namespace HardwareShopContracts.ViewModels
{
    public class PurchaseViewModel : IPurchaseModel
    {
        public int Id { get; set; }

        [DisplayName("Цена")]
        public double Sum { get; set; }

        [DisplayName("Статус покупки")]
        public PurchaseStatus PurchaseStatus { get; set; } = PurchaseStatus.Неизвестен;

        [DisplayName("Дата оплаты")]
        public DateTime? DatePurchase { get; set; }

        public int UserId { get; set; }

        public Dictionary<int, (IGoodModel, int)> PurchaseGoods { get; set; } = new();

        public Dictionary<int, (IBuildModel, int)> PurchaseBuilds { get; set; } = new();
    }
}
