using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDataModels.Models;
using Microsoft.AspNetCore.Mvc;

namespace HardwareShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ComponentController : Controller
    {
        private readonly ILogger _logger;

        private readonly IComponentLogic _component;

        public ComponentController(ILogger<ComponentController> logger, IComponentLogic component)
        {
            _logger = logger;
            _component = component;
        }

        [HttpGet]
        public List<ComponentViewModel>? GetComponents(int userId)
        {
            try
            {
                return _component.ReadList(new() { UserId = userId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка комплектующих");
                throw;
            }
        }

        [HttpGet]
        public ComponentViewModel? GetComponent(int id)
        {
            try
            {
                return _component.ReadElement(new() { Id = id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения комплектующего");
                throw;
            }
        }

        [HttpGet]
        public List<Tuple<BuildViewModel, int>>? GetComponentBuilds(int id)
        {
            try
            {
                var component = _component.ReadElement(new() { Id = id });
                if (component == null)
                    return null;
                var tuple = component.ComponentBuilds.Select(x => Tuple.Create(new BuildViewModel
                {
                    Id = x.Value.Item1.Id,
                    BuildName = x.Value.Item1.BuildName,
                }, x.Value.Item2)).ToList();
                return tuple;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения сборок комплектующего");
                throw;
            }
        }

        [HttpPost]
        public void CreateComponent(ComponentBindingModel model)
        {
            try
            {
                _component.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка создания комплектующего");
                throw;
            }
        }

        [HttpPost]
        public void UpdateData(ComponentBindingModel model)
        {
            try
            {
                for (int i = 0; i < model.ComponentBuildsCounts.Count; i++)
                {
                    model.ComponentBuilds.Add(model.ComponentBuildsBuilds[i].Id,
                        (model.ComponentBuildsBuilds[i] as IBuildModel, model.ComponentBuildsCounts[i]));
                }
                _component.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных комплектующего");
                throw;
            }
        }

        [HttpPost]
        public void UpdateComponent(ComponentBindingModel model)
        {
            try
            {
                model.ComponentBuilds = null!;
                _component.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных комплектующего");
                throw;
            }
        }

        [HttpPost]
        public void DeleteComponent(ComponentBindingModel model)
        {
            try
            {
                _component.Delete(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка удаления комплектующего");
                throw;
            }
        }
    }
}
