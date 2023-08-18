namespace HardwareShopBusinessLogic.OfficePackage.HelperModels
{
    public class ExcelMergeParameters
    {
        public string CellFromName { get; set; } = string.Empty;

        public string CellToName { get; set; } = string.Empty;

        public string Merge => $"{CellFromName}:{CellToName}";
    }
}