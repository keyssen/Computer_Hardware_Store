using HardwareShopContracts.ViewModels;

namespace HardwareShopBusinessLogic.OfficePackage.HelperModels
{
    public class WordInfo
    {
        public string FileName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public List<ReportBuildGoodViewModel> BuildGood { get; set; } = new();

		public List<ReportPurchaseComponentViewModel> PurchaseComponent { get; set; } = new();
	}
}