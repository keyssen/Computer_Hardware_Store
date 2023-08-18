using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDataModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace HardwareShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class GoodController : Controller
    {
        private readonly ILogger _logger;

        private readonly IGoodLogic _good;

        public GoodController(ILogger<GoodController> logger, IGoodLogic good)
        {
            _logger = logger;
            _good = good;
        }

		[HttpGet]
		public List<GoodViewModel>? GetAllGoods()
		{
			try
			{
				return _good.ReadList(null);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка получения списка всех товаров");
				throw;
			}
		}

		[HttpGet]
        public List<GoodViewModel>? GetGoods(int userId)
        {
            try
            {
                return _good.ReadList(new() { UserId = userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка товаров");
                throw;
            }
        }

        [HttpGet]
        public GoodViewModel? GetGood(int id)
        {
            try
            {
                return _good.ReadElement(new() { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения товара");
                throw;
            }
        }

        [HttpGet]
        public Tuple<GoodViewModel, List<Tuple<ComponentViewModel, int>>>? GetGoodUpdate(int id, int userId)
        {
            try
            {
                var good = _good.ReadElement(new() { Id = id, UserId = userId });
                if (good == null)
                    return null;
                var tuple = Tuple.Create(good,
                    good.GoodComponents.Select(x => Tuple.Create(new ComponentViewModel 
                    { 
                        Id = x.Value.Item1.Id,
                        ComponentName = x.Value.Item1.ComponentName,
                        Cost = x.Value.Item1.Cost
                    }, x.Value.Item2)).ToList());
                return tuple;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения товара");
                throw;
            }
        }

        [HttpPost]
        public void CreateGood(GoodBindingModel model)
        {
            try
            {
                for (int i = 0; i < model.GoodComponentsCounts.Count; i++)
                {
                    model.GoodComponents.Add(model.GoodComponentsComponents[i].Id,
                        (model.GoodComponentsComponents[i] as IComponentModel, model.GoodComponentsCounts[i]));
                }
                _good.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания товара");
                throw;
            }
        }

        [HttpPost]
        public void UpdateData(GoodBindingModel model)
        {
            try
            {
                for (int i = 0; i < model.GoodComponentsCounts.Count; i++)
                {
                    model.GoodComponents.Add(model.GoodComponentsComponents[i].Id,
                        (model.GoodComponentsComponents[i] as IComponentModel, model.GoodComponentsCounts[i]));
                }
                _good.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных товара");
                throw;
            }
        }

        [HttpPost]
        public void DeleteGood(GoodBindingModel model)
        {
            try
            {
                _good.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления товара");
                throw;
            }
        }
    }
}
