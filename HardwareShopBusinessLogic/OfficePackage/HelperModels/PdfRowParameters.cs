using HardwareShopBusinessLogic.OfficePackage.HelperEnums;

namespace HardwareShopBusinessLogic.OfficePackage.HelperModels
{
    public class PdfRowParameters
    {
        public List<string> Texts { get; set; } = new();

        public string Style { get; set; } = string.Empty;

        public PdfParagraphAlignmentType ParagraphAlignment { get; set; }
    }
}