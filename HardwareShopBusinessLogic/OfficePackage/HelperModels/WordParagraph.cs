namespace HardwareShopBusinessLogic.OfficePackage.HelperModels
{
    public class WordParagraph
    {
        public List<(string, WordTextProperties)> Texts { get; set; } = new();

        public WordTextProperties? TextProperties { get; set; }
    }
}