using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BuisnessLogicsContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace HardwareShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly ILogger _logger;

        private readonly IOrderLogic _order;

        public OrderController(ILogger<OrderController> logger, IOrderLogic order)
        {
            _logger = logger;
            _order = order;
        }

        [HttpGet]
        public List<OrderViewModel>? GetOrders(int userId)
        {
            try
            {
                return _order.ReadList(new() { UserId = userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка заказов");
                throw;
            }
        }

        [HttpPost]
        public void CreateOrder(OrderBindingModel model)
        {
            try
            {
                _order.CreateOrder(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания заказа");
                throw;
            }
        }

        [HttpPost]
        public void UpdateData(OrderBindingModel model)
        {
            try
            {
                switch (model.Status)
                {
                    case OrderStatus.Неизвестен:
                        break;
                    case OrderStatus.Принят:
                        break;
                    case OrderStatus.Выполняется:
                        _order.TakeOrderInWork(model);
                        break;
                    case OrderStatus.Готов:
                        _order.FinishOrder(model);
                        break;
                    case OrderStatus.Выдан:
                        _order.DeliveryOrder(model);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных заказа");
                throw;
            }
        }

        [HttpPost]
        public void DeleteOrder(OrderBindingModel model)
        {
            try
            {
                _order.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления заказа");
                throw;
            }
        }
    }
}
