namespace HardwareShopBusinessLogic.OfficePackage.HelperModels
{
    public class WordRow
    {
        public List<(string, WordTextProperties)> Rows { get; set; } = new();
    }
}
