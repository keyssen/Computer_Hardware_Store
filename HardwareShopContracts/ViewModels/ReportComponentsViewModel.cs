namespace HardwareShopContracts.ViewModels
{
    public class ReportComponentsViewModel
    {
        public string ComponentName { get; set; } = string.Empty;

        public int TotalCount { get; set; }

        public List<Tuple<string, int>> GoodOrBuilds { get; set; } = new();
    }
}
