using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using Microsoft.Extensions.Logging;

namespace HardwareShopBusinessLogic.BusinessLogics.Storekeeper
{
    public class GoodLogic : IGoodLogic
    {
        private readonly ILogger _logger;
        private readonly IGoodStorage _goodStorage;
        public GoodLogic(ILogger<GoodLogic> logger, IGoodStorage goodStorage)
        {
            _logger = logger;
            _goodStorage = goodStorage;
        }
        public bool Create(GoodBindingModel model)
        {
            CheckModel(model);
            if (_goodStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(GoodBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id: {Id}", model.Id);
            if (_goodStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        public GoodViewModel? ReadElement(GoodSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. GoodName: {GoodName}. Id: {Id}",
                model.GoodName, model.Id);
            var element = _goodStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id: {Id}", element.Id);
            return element;
        }

        public List<GoodViewModel>? ReadList(GoodSearchModel? model)
        {
            _logger.LogInformation("ReadList. GoodName: {GoodName}. Id: {Id}",
                model?.GoodName, model?.Id);
            var list = model == null ? _goodStorage.GetFullList() :
                _goodStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count: {Count}", list.Count);
            return list;
        }

        public bool Update(GoodBindingModel model)
        {
            CheckModel(model);
            if (_goodStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(GoodBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.GoodName))
            {
                throw new ArgumentNullException("Нет названия товара", nameof(model.GoodName));
            }
            if (model.Price <= 0)
            {
                throw new ArgumentNullException("Цена товара должна быть больше 0",
                    nameof(model.Price));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор у пользователя",
                    nameof(model.Price));
            }
            _logger.LogInformation("Good. GoodName: {GoodName}. Price: {Price}. Id: {Id}",
                model.GoodName, model.Price, model.Id);
            var element = _goodStorage.GetElement(new GoodSearchModel
            {
                GoodName = model.GoodName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Товар с таким названием уже есть");
            }
        }
    }
}
