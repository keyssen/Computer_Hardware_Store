namespace HardwareShopContracts.SearchModels
{
    public class BuildSearchModel
    {
        public int? Id { get; set; }

        public string? BuildName { get; set; } = string.Empty;

        public int? UserId { get; set; }
    }
}
