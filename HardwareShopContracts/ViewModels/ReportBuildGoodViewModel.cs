namespace HardwareShopContracts.ViewModels
{
    public class ReportBuildGoodViewModel
    {
        public string GoodName { get; set; } = string.Empty;
        public List<string> Builds
        {
            get;
            set;
        } = new();
    }
}
