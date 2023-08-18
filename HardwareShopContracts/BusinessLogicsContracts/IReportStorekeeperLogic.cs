using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public interface IReportStorekeeperLogic
    {
        /// <summary>
        /// Получение списка сборок с указанием, в каких товарах используются
        /// </summary>
        /// <returns></returns>
        List<ReportBuildGoodViewModel> GetBuildGood(List<GoodViewModel> goods);

        /// <summary>
        /// Получение сведений по комплектующим за период, 
        /// с указанием в каких товарах и сборках они использовались
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        List<ReportComponentsViewModel> GetComponents(ReportBindingModel model);

        /// <summary>
        /// Сохранение списка сборок по выбранным товарам в файл-Word
        /// </summary>
        /// <param name="model"></param>
        byte[] SaveBuildGoodToWordFile(ReportBindingModel model, List<GoodViewModel> goods);

        /// <summary>
        /// Сохранение списка сборок по выбранным товарам в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        byte[] SaveBuildGoodToExcelFile(ReportBindingModel model, List<GoodViewModel> goods);

        /// <summary>
        /// Отправление отчета на почту
        /// </summary>
        /// <param name="model"></param>
        bool SendReportOnMail(ReportBindingModel model);
    }
}
