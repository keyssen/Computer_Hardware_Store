namespace HardwareShopContracts.SearchModels
{
    public class ComponentSearchModel
    {
        public int? Id { get; set; }
        public string? ComponentName { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
    }
}
