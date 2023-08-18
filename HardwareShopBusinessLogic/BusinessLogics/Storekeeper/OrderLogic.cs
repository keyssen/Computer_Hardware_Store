using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BuisnessLogicsContracts;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDataModels.Enums;
using Microsoft.Extensions.Logging;

namespace HardwareShopBusinessLogic.BusinessLogics.Storekeeper
{
    public class OrderLogic : IOrderLogic
    {
        private readonly ILogger _logger;
        private readonly IOrderStorage _orderStorage;

        public OrderLogic(ILogger<OrderLogic> logger, IOrderStorage orderStorage)
        {
            _logger = logger;
            _orderStorage = orderStorage;
        }
        public bool CreateOrder(OrderBindingModel model)
        {
            CheckModel(model);
            if (model.Status != OrderStatus.Неизвестен)
            {
                _logger.LogWarning("Insert operation failed. Order status incorrect.");
                return false;
            }
            model.Status = OrderStatus.Принят;
            if (_orderStorage.Insert(model) == null)
            {
                model.Status = OrderStatus.Неизвестен;
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public List<OrderViewModel>? ReadList(OrderSearchModel? model)
        {
            _logger.LogInformation("Order. OrderId: {Id}", model?.Id);
            var list = model == null ? _orderStorage.GetFullList() : _orderStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count: {Count}", list.Count);
            return list;
        }

        private bool StatusUpdate(OrderBindingModel model, OrderStatus newStatus)
        {
            var viewModel = _orderStorage.GetElement(new OrderSearchModel { Id = model.Id });
            if (viewModel == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (viewModel.Status + 1 != newStatus)
            {
                _logger.LogWarning("Status update to " + newStatus.ToString() + " operation failed. Order status incorrect.");
                return false;
            }
            model.Status = newStatus;
            if (model.Status == OrderStatus.Готов) model.DateImplement = DateTime.Now;
            else
            {
                model.DateImplement = viewModel.DateImplement;
            }
            CheckModel(model, false);
            if (_orderStorage.Update(model) == null)
            {
                model.Status--;
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(OrderBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (model.GoodId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор у товара", nameof(model.GoodId));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор у пользователя", nameof(model.UserId));
            }
            if (model.Count <= 0)
            {
                throw new ArgumentNullException("Количество товара в заказе должно быть больше 0", nameof(model.Count));
            }
            if (model.Sum <= 0)
            {
                throw new ArgumentNullException("Сумма заказа должна быть больше 0", nameof(model.Sum));
            }
            _logger.LogInformation("Order. OrderId: {Id}. Sum: {Sum}. GoodId: {GoodId}", model.Id, model.Sum, model.GoodId);
        }

        public bool TakeOrderInWork(OrderBindingModel model)
        {
            return StatusUpdate(model, OrderStatus.Выполняется);
        }

        public bool FinishOrder(OrderBindingModel model)
        {
            return StatusUpdate(model, OrderStatus.Готов);
        }

        public bool DeliveryOrder(OrderBindingModel model)
        {
            return StatusUpdate(model, OrderStatus.Выдан);
        }

        public bool Delete(OrderBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id: {Id}", model.Id);
            if (_orderStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }
    }
}
