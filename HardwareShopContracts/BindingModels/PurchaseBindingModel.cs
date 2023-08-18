using HardwareShopContracts.ViewModels;
using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;

namespace HardwareShopContracts.BindingModels
{
    public class PurchaseBindingModel : IPurchaseModel
    {
        public int Id { get; set; }

        public double Sum { get; set; }

        public PurchaseStatus PurchaseStatus { get; set; } = PurchaseStatus.Неизвестен;

        public DateTime? DatePurchase { get; set; }

        public int UserId { get; set; }

        public Dictionary<int, (IGoodModel, int)> PurchaseGoods { get; set; } = new();

        public Dictionary<int, (IBuildModel, int)> PurchaseBuilds { get; set; } = new();

		public List<GoodViewModel> ListPurchaseGoods
		{
			get;
			set;
		} = new();

		public List<int> PurchaseGoodsCounts
		{
			get;
			set;
		} = new();

		public List<PurchaseViewModel> Purchases
		{
			get;
			set;
		} = new();
	}
}
