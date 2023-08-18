using HardwareShopBusinessLogic.MailWorker;
using HardwareShopBusinessLogic.OfficePackage;
using HardwareShopBusinessLogic.OfficePackage.HelperModels;
using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;

namespace HardwareShopBusinessLogic.BusinessLogics.Storekeeper
{
    public class ReportStorekeeperLogic : IReportStorekeeperLogic
    {
        private readonly IComponentStorage _componentStorage;

        private readonly IGoodStorage _goodStorage;

        private readonly AbstractSaveToExcel _saveToExcel;

        private readonly AbstractSaveToWord _saveToWord;

        private readonly AbstractSaveToPdf _saveToPdf;

        private readonly AbstractMailWorker _mailWorker;

        public ReportStorekeeperLogic(IComponentStorage componentStorage, AbstractSaveToExcel abstractSaveToExcel, AbstractSaveToWord abstractSaveToWord, IGoodStorage goodStorage, AbstractMailWorker abstractMailWorker, AbstractSaveToPdf saveToPdf)
        {
            _componentStorage = componentStorage;
            _saveToExcel = abstractSaveToExcel;
            _saveToWord = abstractSaveToWord;
            _goodStorage = goodStorage;
            _mailWorker = abstractMailWorker;
            _saveToPdf = saveToPdf;
        }
        public List<ReportBuildGoodViewModel> GetBuildGood(List<GoodViewModel> goods)
        {
            var result = new List<ReportBuildGoodViewModel>();

            foreach (var g in goods)
            {
                var good = _goodStorage.GetElement(new() { Id = g.Id })!;
                var builds = good.GoodComponents
                    //получили сборки и количество компонентов
                    .Select(x => _componentStorage.GetComponentBuilds(new() { Id = x.Key })
                    //если кол-во компонентов в товаре == кол-ву в сборке
                        .Where(y => x.Value.Item2 == y.Item2))
                    .SelectMany(x => x.Select(x => x.Item1))
                    .Distinct()
                    .ToList();
                ReportBuildGoodViewModel record = new()
                {
                    GoodName = good.GoodName,
                    Builds = builds
                };
                result.Add(record);
            }
            return result;
        }
        /// Получение сведений по комплектующим за период, 
        /// с указанием в каких товарах и сборках они использовались
		public List<ReportComponentsViewModel> GetComponents(ReportBindingModel model)
        {
            var result = new List<ReportComponentsViewModel>();
            var components = _componentStorage.GetFilteredList(new()
            {
                UserId = model.UserId,
                DateFrom = model.DateFrom,
                DateTo = model.DateTo
            });

            foreach (var component in components)
            {
                var builds = component.ComponentBuilds
                    .Select(x => Tuple.Create(x.Value.Item1.BuildName, x.Value.Item2))
                    .ToList();

                var goods = _componentStorage
                    .GetComponentGoods(new() { Id = component.Id })
                    .ToList();

                ReportComponentsViewModel record = new()
                {
                    ComponentName = component.ComponentName,
                    TotalCount = builds.Sum(x => x.Item2) + goods.Sum(x => x.Item2),
                    GoodOrBuilds = builds.Concat(goods).ToList()
                };

                result.Add(record);
            }
            return result;
        }

        public byte[] SaveBuildGoodToWordFile(ReportBindingModel model, List<GoodViewModel> goods)
        {
            _saveToWord.CreateBuildGoodReport(new WordInfo
            {
                FileName = model.FileName,
                Title = "Cписок сборок по выбранным товарам",
                BuildGood = GetBuildGood(goods)
            });

            byte[] file = File.ReadAllBytes(model.FileName);
            File.Delete(model.FileName);
            return file;
        }

        public byte[] SaveBuildGoodToExcelFile(ReportBindingModel model, List<GoodViewModel> goods)
        {
            _saveToExcel.CreateBuildGoodReport(new ExcelInfo
            {
                FileName = model.FileName,
                Title = "Cписок сборок по выбранным товарам",
                BuildGood = GetBuildGood(goods)
            });

            byte[] file = File.ReadAllBytes(model.FileName);
            File.Delete(model.FileName);
            return file;
        }

        public bool SendReportOnMail(ReportBindingModel model)
        {
            model.FileName = "temp.pdf";
            _saveToPdf.CreateComponentsReport(new PdfInfo
            {
                FileName = model.FileName,
                Title = "Отчет по комплектующим",
                DateFrom = model.DateFrom,
                DateTo = model.DateTo,
                ReportComponents = GetComponents(model)
            });
            byte[] file = File.ReadAllBytes(model.FileName);
            File.Delete(model.FileName);

            _mailWorker.MailSendAsync(new()
            {
                MailAddress = model.UserEmail,
                Subject = "Отчет по комплектующим",
                Text = $"Отчет по полученным вами комлектующим за период с {model.DateFrom} по {model.DateTo} в формате Pdf.",
                File = file
            });
            return true;
        }
    }
}
