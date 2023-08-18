using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDataModels.Enums;
using Microsoft.Extensions.Logging;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public class PurchaseLogic : IPurchaseLogic
    {
        private readonly ILogger _logger;
        private readonly IPurchaseStorage _purchaseStorage;
        public PurchaseLogic(ILogger<PurchaseLogic> logger, IPurchaseStorage purchaseStorage)
        {
            _logger = logger;
            _purchaseStorage = purchaseStorage;
        }

        public List<PurchaseViewModel>? ReadList(PurchaseSearchModel? model)
        {
            _logger.LogInformation("ReadList. Id:{Id}", model?.Id);
            var list = model == null ? _purchaseStorage.GetFullList() : _purchaseStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public List<PurchaseViewModel>? ReadOrderList(PurchaseSearchModel model)
        {
            _logger.LogInformation("ReadOrderList. Id:{Id}", model.Id);
            var list = _purchaseStorage.GetReportFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadOrderList return null list");
                return null;
            }
            _logger.LogInformation("ReadOrderList. Count:{Count}", list.Count);
            return list;
        }

        public PurchaseViewModel? ReadElement(PurchaseSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. Id:{Id}", model.Id);
            var element = _purchaseStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public bool DeliveryPurchase(PurchaseBindingModel model)
        {
            var viewModel = _purchaseStorage.GetElement(new PurchaseSearchModel { Id = model.Id });
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (viewModel.PurchaseStatus + 1 != PurchaseStatus.Выдан)
            {
                _logger.LogWarning("Change status operation failed");
                return false;
            }
            model.PurchaseStatus = PurchaseStatus.Выдан;
            model.DatePurchase = DateTime.Now;
            CheckModel(model, false);
            if (_purchaseStorage.Update(model, false) == null)
            {
                _logger.LogWarning("Change status operation failed");
                return false;
            }
            return true;
        }

        public bool Create(PurchaseBindingModel model)
        {
            CheckModel(model);
            if (model.PurchaseStatus != PurchaseStatus.Неизвестен)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            model.PurchaseStatus = PurchaseStatus.Выполняется;
            if (_purchaseStorage.Insert(model) == null)
            {
                model.PurchaseStatus = PurchaseStatus.Неизвестен;
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Update(PurchaseBindingModel model)
        {
            if (model.PurchaseStatus == PurchaseStatus.Выдан)
            {
                _logger.LogWarning("Update status operation failed");
                return false;
            }
            CheckModel(model);
            if (_purchaseStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(PurchaseBindingModel model)
        {
            if (model.PurchaseStatus == PurchaseStatus.Выдан)
            {
                _logger.LogWarning("Delete status operation failed");
                return false;
            }
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_purchaseStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(PurchaseBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (model.Id < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор у продукта", nameof(model.Id));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор у клиента", nameof(model.UserId));
            }
            if (model.Sum <= 0)
            {
                throw new ArgumentNullException("Сумма заказа должна быть больше 0", nameof(model.Sum));
            }
            _logger.LogInformation("Purchase.  UserId:{UserId}. PurchaseID:{Id}. Sum:{ Sum}", model.UserId, model.Id, model.Sum);
        }
    }
}
