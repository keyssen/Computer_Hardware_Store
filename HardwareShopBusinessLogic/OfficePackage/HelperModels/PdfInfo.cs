
using HardwareShopContracts.ViewModels;

namespace HardwareShopBusinessLogic.OfficePackage.HelperModels
{
    public class PdfInfo
    {
        public string FileName { get; set; } = string.Empty;

        public string Title { get; set; } = string.Empty;

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public List<ReportComponentsViewModel> ReportComponents { get; set; } = new();

        public List<ReportPurchaseViewModel> ReportPurchases { get; set; } = new();
    }
}