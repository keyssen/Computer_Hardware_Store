using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using Microsoft.Extensions.Logging;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public class BuildLogic : IBuildLogic
    {
        private readonly ILogger _logger;
        private readonly IBuildStorage _buildStorage;
        private readonly IComponentStorage _componentStorage;
        public BuildLogic(ILogger<BuildLogic> logger, IBuildStorage buildStorage, IComponentStorage componentStorage)
        {
            _logger = logger;
            _buildStorage = buildStorage;
            _componentStorage = componentStorage;
        }

        public List<BuildViewModel>? ReadList(BuildSearchModel? model)
        {
            _logger.LogInformation("ReadList. BuildName:{BuildName}. Id:{Id}", model?.BuildName, model?.Id);
            var list = model == null ? _buildStorage.GetFullList() : _buildStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public BuildViewModel? ReadElement(BuildSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. BuildName:{BuildName}. Id:{Id}", model.BuildName, model.Id);
            var element = _buildStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public bool Create(BuildBindingModel model)
        {
            CheckModel(model);
            if (_buildStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Update(BuildBindingModel model)
        {
            CheckModel(model);
            if (_buildStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(BuildBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_buildStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(BuildBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.BuildName))
            {
                throw new ArgumentNullException("Нет названия сборки", nameof(model.BuildName));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор у клиента", nameof(model.UserId));
            }
            if (model.Id < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор у сборки", nameof(model.Id));
            }
            _logger.LogInformation("Build. UserId:{UserId}. BuildName:{BuildName}. Price:{Price}. Id:{Id}", model.UserId, model.BuildName, model.Price, model.Id);
            var element = _buildStorage.GetElement(new BuildSearchModel
            {
                BuildName = model.BuildName
            });
            if (element != null && element.Id != model.Id)
            {
                throw new InvalidOperationException("Продукт с таким названием уже есть");
            }
        }
    }
}
