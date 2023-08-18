using DocumentFormat.OpenXml.Drawing;
using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HardwareShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ReportController : Controller
	{
        private readonly ILogger _logger;
        private readonly IReportStorekeeperLogic _reportStorekeeperLogic;
        private readonly IWorkerReportLogic _reportWorkerLogic;

        public ReportController(ILogger<ReportController> logger, IReportStorekeeperLogic reportStorekeeperLogic, IWorkerReportLogic workerReportLogic)
        {
            _logger = logger;
            _reportStorekeeperLogic = reportStorekeeperLogic;
            _reportWorkerLogic = workerReportLogic;
        }

        [HttpPost]
        public byte[] BuildGoodReport(GoodBindingModel model, string format)
        {
            try
            {
                byte[] file;
                switch (format)
                {
                    case "docx":
                        file = _reportStorekeeperLogic.SaveBuildGoodToWordFile(new ReportBindingModel { FileName = "temp.docx" }, model.Goods);
                        break;
                    case "xlsx":
                        file = _reportStorekeeperLogic.SaveBuildGoodToExcelFile(new ReportBindingModel { FileName = "temp.xlsx" }, model.Goods);
                        break;
                    default:
                        throw new FormatException("Неправильный формат файла");
                }
                return file;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка сборок по выбранным товарам");
                throw;
            }
        }

        [HttpPost]
        public List<ReportComponentsViewModel>? ComponentsReport(ReportBindingModel model)
        {
            try
            {
                var list = _reportStorekeeperLogic.GetComponents(model);
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения сведений по полученным пользователем комплектующим за период");
                throw;
            }
        }

        [HttpPost]
        public void ComponentsReportSendOnMail(ReportBindingModel model)
        {
            try
            {
                _reportStorekeeperLogic.SendReportOnMail(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения сведений по полученным пользователем комплектующим за период");
                throw;
            }
        }


		[HttpPost]
		public byte[] BuildPurchaseReport(PurchaseBindingModel model, string format)
		{
			try
			{
				byte[] file;
				switch (format)
				{
					case "docx":
						file = _reportWorkerLogic.SavePurchasesToWordFile(new ReportBindingModel { FileName = "temp.docx" }, model.Purchases);
						break;
					case "xlsx":
						file = _reportWorkerLogic.SavePurchasesToExcelFile(new ReportBindingModel { FileName = "temp.xlsx" }, model.Purchases);
						break;
					default:
						throw new FormatException("Неправильный формат файла");
				}
				return file;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка сохранения списка сборок по выбранным товарам");
				throw;
			}
		}


		[HttpPost]
		public List<ReportPurchaseViewModel>? GetPurchaseReportData(ReportBindingModel model)
		{
			try
			{
				var report = _reportWorkerLogic.GetPurchase(model);
				return report;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка получения данных для отчёта");
				throw;
			}
		}

        [HttpPost]
        public void SendByMailPurchaseReport(ReportBindingModel reportModel)
        {
            try
            {
                _reportWorkerLogic.SendByMailPurchaseReport(reportModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения сведений по полученным пользователем комплектующим за период");
                throw;
            }           
        }
    }

}
