
using HardwareShopBusinessLogic.MailWorker;
using HardwareShopBusinessLogic.OfficePackage;
using HardwareShopBusinessLogic.OfficePackage.HelperModels;
using HardwareShopBusinessLogic.OfficePackage.Implements;
using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Implements.Worker;
using HardwareShopDatabaseImplement.Models.Worker;
using System.Reflection.PortableExecutable;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public class WorkerReportLogic : IWorkerReportLogic
    {

        private readonly IPurchaseStorage _purchaseStorage;

        private readonly AbstractSaveToExcel _saveToExcel;

        private readonly AbstractSaveToWord _saveToWord;

        private readonly AbstractSaveToPdf _saveToPdf;

        private readonly AbstractMailWorker _mailKitWorker;

        public WorkerReportLogic(IPurchaseStorage purchaseStorage, AbstractMailWorker mailKitWorker, AbstractSaveToPdf saveToPdf, AbstractSaveToExcel saveToExcel, AbstractSaveToWord saveToWord)
        {
            _purchaseStorage = purchaseStorage;
            _saveToExcel = saveToExcel;
            _saveToWord = saveToWord;
            _saveToPdf = saveToPdf;
            _mailKitWorker = mailKitWorker;
        }

        /// <summary>
        /// Получение списка компонент с указанием, в каких покупках используются
        /// </summary>
        /// <returns></returns>
        public List<ReportPurchaseComponentViewModel> GetPurchaseComponent(List<PurchaseViewModel> purchaseList)
        {
            var list = new List<ReportPurchaseComponentViewModel>();

            foreach (var p in purchaseList)
            {
                var purchase = _purchaseStorage.GetElement(new() { Id = p.Id })!;

                var record = new ReportPurchaseComponentViewModel
                {
                    Id = purchase.Id,
                    Builds = new List<(string Build, int count, List<(string Component, int count)>)>(),
                    TotalCount = 0,
                    TotalCost = purchase.Sum,
                };
                foreach (var build in purchase.PurchaseBuilds)
                {
                    List<(string Component, int count)> componentList = new List<(string Component, int count)>();
                    int buildTotalCount = 0;
                    foreach (var component in build.Value.Item1.BuildComponents)
                    {
                        componentList.Add(new(component.Value.Item1.ComponentName, component.Value.Item2));
                        buildTotalCount += component.Value.Item2;
                    }
                    record.Builds.Add(new(build.Value.Item1.BuildName, build.Value.Item2, componentList));
                    record.TotalCount += buildTotalCount * build.Value.Item2;
                }

                list.Add(record);
            }
            return list;
        }

        /// <summary>
        /// Получение списка покупок за определенный период
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ReportPurchaseViewModel> GetPurchase(ReportBindingModel model)
        {
            var list = new List<ReportPurchaseViewModel>();
            var purchases = _purchaseStorage.GetFilteredList(new PurchaseSearchModel { DateFrom = model.DateFrom, DateTo = model.DateTo, UserId = model.UserId });
            foreach (var p in purchases)
            {
                var purchase = _purchaseStorage.GetElement(new() { Id = p.Id })!;
                List<string> commentList = new List<string>();
                List<string> componentList = new List<string>();
                foreach (var build in purchase.PurchaseBuilds)
                {
                    foreach (var comment in build.Value.Item1.BuildComments)
                    {
                        commentList.Add(new(comment.Value.Text));
                    }
                    foreach (var component in build.Value.Item1.BuildComponents)
                    {
                        componentList.Add(component.Value.Item1.ComponentName);
                    }
                }
                var record = new ReportPurchaseViewModel
                {
                    Id = purchase.Id,
                    PurchaseDate = (DateTime)p.DatePurchase,
                    PurchaseSum = p.Sum,
                    Comments = commentList,
                    Components = componentList.Distinct().ToList()
                };
                list.Add(record);
            }
            return list;
        }


        /// <summary>
        /// Сохранение компонент с указаеним покупок в файл-Word
        /// </summary>
        /// <param name="model"></param>
        public byte[] SavePurchasesToWordFile(ReportBindingModel model, List<PurchaseViewModel> purchases)
        {
            _saveToWord.CreateBuildPurchaseReport(new WordInfo
            {
                FileName = model.FileName,
                Title = "Список Компонентов",
                PurchaseComponent = GetPurchaseComponent(purchases)
            });

            byte[] file = File.ReadAllBytes(model.FileName);
            File.Delete(model.FileName);
            return file;
        }

        /// <summary>
        /// Сохранение компонент с указаеним покупок в файл-Excel
        /// </summary>
        /// <param name="model"></param>
        public byte[] SavePurchasesToExcelFile(ReportBindingModel model, List<PurchaseViewModel> purchases)
        {
            _saveToExcel.CreatePurchaseComponentReport(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Список Компонентов",
                PurchaseComponent = GetPurchaseComponent(purchases)
            });
            byte[] file = File.ReadAllBytes(model.FileName);
            File.Delete(model.FileName);
            return file;
        }

        /// <summary>
        /// Сохранение отчёта по покупкам в файл-Pdf
        /// </summary>
        /// <param name="model"></param>
        public void SendByMailPurchaseReport(ReportBindingModel model)
        {
            model.FileName = "temp.pdf";
            _saveToPdf.GetPurchaseReportFile(new()
            {
                FileName = model.FileName,
                Title = "Отчет по покупкам",
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                ReportPurchases = GetPurchase(model)
            });
            byte[] file = File.ReadAllBytes(model.FileName);
            File.Delete(model.FileName);

            _mailKitWorker.MailSendAsync(new()
            {
                MailAddress = model.UserEmail,
                Subject = "Отчет по покупкам",
                Text = $"За период с {model.DateFrom.ToShortDateString()} " +
                    $"по {model.DateTo.ToShortDateString()}.",
                File = file
            });
        }
    }
}