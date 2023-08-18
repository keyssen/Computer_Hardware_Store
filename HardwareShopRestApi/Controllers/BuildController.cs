using HardwareShopBusinessLogic.BusinessLogics.Storekeeper;
using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.ManyToMany;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDatabaseImplement.Models.Worker;
using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace HardwareShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BuildController : Controller
    {
        private readonly ILogger _logger;
        private readonly IBuildLogic _buildLogic;
		private readonly IPurchaseLogic _purchaseLogic;

		public BuildController(IBuildLogic buildLogic, IPurchaseLogic purchaseLogic, ILogger<UserController> logger)
        {
            _logger = logger;
            _buildLogic = buildLogic;
            _purchaseLogic = purchaseLogic;

		}

		[HttpGet]
        public List<BuildViewModel>? GetBuilds(int userId = 0)
        {
            try
            {
                if (userId == 0)
                    return _buildLogic.ReadList(null);
                return _buildLogic.ReadList(new BuildSearchModel
                {
                    UserId = userId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка сборок пользоватля");
                throw;
            }
        }

        [HttpGet]
        public BuildViewModel? GetBuild(int buildId)
        {
            try
            {       
                return _buildLogic.ReadElement(new() { Id = buildId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка сборки");
                throw;
            }
        }

		[HttpGet]
		public List<Tuple<PurchaseViewModel, int>>? GetBuildPurchase(int buildId)
		{
			try
			{
				var result = _buildLogic.ReadElement(new() { Id = buildId });
				List<Tuple<PurchaseViewModel, int>> listPurchase = new List<Tuple<PurchaseViewModel, int>>();
				foreach (var item in result.BuildPurchases)
				{
					listPurchase.Add(Tuple.Create(new PurchaseViewModel
                    {
                        Id = item.Value.Item1.Id,
						Sum = item.Value.Item1.Sum,
						PurchaseStatus = item.Value.Item1.PurchaseStatus,
					}, item.Value.Item2));
				}
				return listPurchase;

			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка сборки");
				throw;
			}
		}

		[HttpPost]
        public void Create(BuildBindingModel model)
        {
            try
            {
                _buildLogic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка сборки");
                throw;
            }
        }


		[HttpPost]
        public void Update(BuildBindingModel model)
        {
            try
            {
				var build = _buildLogic.ReadElement(new() { Id = model.Id });
				model.Price = build.Price;
                _buildLogic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }

		private double Calc(int purchaseId)
		{
			var purchase = _purchaseLogic.ReadElement(new() { Id = purchaseId });
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

		[HttpGet]
		public bool DeleteLinkPurchase(int deleteBuildId, int deletePurchaseId, int userId)
		{
			try
			{
				var build = _buildLogic.ReadElement(new() { Id = deleteBuildId });
				var purchase = _purchaseLogic.ReadElement(new() { Id = deletePurchaseId });
                if (purchase == null || build == null || build.UserId != userId || purchase.UserId != userId
                    || purchase.PurchaseStatus == PurchaseStatus.Выдан) return false;
                build.BuildPurchases.Remove(deletePurchaseId);
				
				_buildLogic.Update(new BuildBindingModel
				{
					Id = deleteBuildId,
					Price = build.Price,
					BuildName = build.BuildName,
					BuildPurchases = build.BuildPurchases
				});
				purchase.Sum = Calc(purchase.Id);
				_purchaseLogic.Update(new PurchaseBindingModel
				{
					Id = purchase.Id,
					Sum = purchase.Sum,
					PurchaseStatus = purchase.PurchaseStatus,
					DatePurchase = purchase.DatePurchase,
					PurchaseGoods = purchase.PurchaseGoods,
				});
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка обновления данных");
				throw;
			}
		}

		[HttpGet]
		public bool UpdateLinkPurchase(int buildId, int purchaseId, int count, int userId)
		{
			try
			{
                var build = _buildLogic.ReadElement(new() { Id = buildId });
                var purchase = _purchaseLogic.ReadElement(new() { Id = purchaseId });
                if (purchase == null || build == null || build.UserId != userId || purchase.UserId != userId
                    || purchase.PurchaseStatus == PurchaseStatus.Выдан) return false;
                if (build.BuildPurchases.ContainsKey(purchaseId))
				{
					build.BuildPurchases[purchaseId] = (build.BuildPurchases[purchaseId].Item1, count);
				}
				else
				{
					build.BuildPurchases.Add(purchaseId, (purchase, count));
				}
                _buildLogic.Update(new BuildBindingModel
				{
					Id = buildId,
					Price = build.Price,
					BuildName = build.BuildName,
					BuildPurchases = build.BuildPurchases
				});
				purchase.Sum = Calc(purchase.Id);
				_purchaseLogic.Update(new PurchaseBindingModel
				{
					Id = purchase.Id,
					Sum = purchase.Sum,
					PurchaseStatus = purchase.PurchaseStatus,
					DatePurchase = purchase.DatePurchase,
					PurchaseGoods = purchase.PurchaseGoods,
					UserId = purchase.UserId
				});
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка обновления данных");
				throw;
			}
		}

		[HttpPost]
        public void DeleteBuild(BuildBindingModel model)
        {
            try
            {
                _buildLogic.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления сборки");
                throw;
            }
        }
    }
}
