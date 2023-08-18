namespace HardwareShopContracts.ViewModels
{
    public class ReportPurchaseViewModel
    {
        public int Id { get; set; }

        public DateTime PurchaseDate { get; set; }

        public double PurchaseSum { get; set; }

        public List<string> Comments { get; set; } = new();

        public List<string> Components { get; set; } = new();
    }
}
