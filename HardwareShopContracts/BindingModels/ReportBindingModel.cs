namespace HardwareShopContracts.BindingModels
{
    public class ReportBindingModel
    {
        public string UserEmail { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public int UserId { get; set; }
    }
}
