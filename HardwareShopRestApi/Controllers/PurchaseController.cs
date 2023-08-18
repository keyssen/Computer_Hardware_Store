using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;
using HardwareShopRestApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Xml.Linq;

namespace HardwareShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class PurchaseController : Controller
    {
        private readonly ILogger _logger;

        private readonly IPurchaseLogic _purchaseLogic;

        public PurchaseController(IPurchaseLogic purchaseLogic, ILogger<UserController> logger)
        {
            _logger = logger;
            _purchaseLogic = purchaseLogic;
		}

		[HttpGet]
		public Tuple<PurchaseViewModel, List<Tuple<GoodViewModel, int>>>? GetPurchaseUpdate(int purchaseId, int userId)
		{
			try
			{
				var purchase = _purchaseLogic.ReadElement(new() { Id = purchaseId });
				if (purchase == null || purchase.UserId != userId)
					return null;
				var tuple = Tuple.Create(purchase,
					purchase.PurchaseGoods.Select(x => Tuple.Create(new GoodViewModel
					{
						Id = x.Value.Item1.Id,
						GoodName = x.Value.Item1.GoodName,
						Price = x.Value.Item1.Price
					}, x.Value.Item2)).ToList());
				return tuple;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка получения покупки");
				throw;
			}
		}

		[HttpPost]
		public List<PurchaseViewModel>? GetPurchases(PurchaseSearchModel model)
		{
			try
			{
				return _purchaseLogic.ReadList(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка получения списка товаров");
				throw;
			}
		}

        [HttpGet]
        public List<PurchaseViewModel>? GetPurchasesNotDelivery(int userId)
        {
            try
            {
                return _purchaseLogic.ReadList(new() { UserId = userId, PurchaseStatus = PurchaseStatus.Выполняется });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка товаров");
                throw;
            }
        }

        [HttpGet]
		public PurchaseViewModel? GetPurchase(int purchaseId)
		{
			try
			{
				return _purchaseLogic.ReadElement(new() { Id = purchaseId });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка получения товара");
				throw;
			}
		}

		[HttpPost]
		public void CreatePurchase(PurchaseBindingModel model)
		{
			try
			{
				for (int i = 0; i < model.PurchaseGoodsCounts.Count; i++)
				{
					model.PurchaseGoods.Add(model.ListPurchaseGoods[i].Id,(model.ListPurchaseGoods[i] as IGoodModel, model.PurchaseGoodsCounts[i]));
				}
				_purchaseLogic.Create(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка создания товара");
				throw;
			}
		}

		[HttpPost]
		public void UpdateStatusPurchase(PurchaseBindingModel model)
		{
			try
			{
				var oldModel = _purchaseLogic.ReadElement(new() {
					Id = model.Id,
				});
				model.Sum = oldModel.Sum;
				_purchaseLogic.DeliveryPurchase(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка обновления данных товара");
				throw;
			}
		}

		[HttpPost]
		public void Update(PurchaseBindingModel model)
		{
			try
			{
				for (int i = 0; i < model.PurchaseGoodsCounts.Count; i++)
				{
					model.PurchaseGoods.Add(model.ListPurchaseGoods[i].Id, (model.ListPurchaseGoods[i] as IGoodModel, model.PurchaseGoodsCounts[i]));
				}			
				model.PurchaseBuilds = _purchaseLogic.ReadElement(new() { Id = model.Id }).PurchaseBuilds;
				model.Sum = Calc(model);
				_purchaseLogic.Update(model);				
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка обновления данных товара");
				throw;
			}
		}

		private double Calc(PurchaseBindingModel purchase)
		{
			double price = 0;
			foreach (var elem in purchase.PurchaseBuilds)
			{
				price += ((elem.Value.Item1?.Price ?? 0) * elem.Value.Item2);
			}
			foreach (var elem in purchase.PurchaseGoods)
			{
				price += ((elem.Value.Item1?.Price ?? 0) * elem.Value.Item2);
			}
			return Math.Round(price * 1.1, 2);
		}

		[HttpPost]
		public void DeletePurchase(PurchaseBindingModel model)
		{
			try
			{
				_purchaseLogic.Delete(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка удаления товара");
				throw;
			}
        }
    }
}


