
using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public interface IWorkerReportLogic
    {
        /// <summary>
        /// Получение списка компонент с указанием, в каких покупках используются
        /// </summary>
        /// <returns></returns>
        List<ReportPurchaseComponentViewModel> GetPurchaseComponent(List<PurchaseViewModel> purchaseList);

        /// <summary>
        /// Получение списка покупок за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        List<ReportPurchaseViewModel> GetPurchase(ReportBindingModel model);

        /// <summary>
        /// Сохранение компонент с указаеним покупок в файл-Word
        /// </summary>
        /// <param name="model"></param>
        byte[] SavePurchasesToWordFile(ReportBindingModel model, List<PurchaseViewModel> purchases);

        /// <summary>
        /// Сохранение компонент с указаеним покупок в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        byte[] SavePurchasesToExcelFile(ReportBindingModel model, List<PurchaseViewModel> purchases);

        /// <summary>
        /// Сохранение отчёта по покупкам в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        void SendByMailPurchaseReport(ReportBindingModel model);
    }
}