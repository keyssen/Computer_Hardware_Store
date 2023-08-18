namespace HardwareShopContracts.ViewModels
{
    public class ReportPurchaseComponentViewModel
    {
        public int Id { get; set; }

        public List<(string Build, int count, List<(string Component, int count)>)> Builds { get; set; } = new();

        public int TotalCount { get; set; }

        public double TotalCost { get; set; }


    }
}
