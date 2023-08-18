using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using Microsoft.EntityFrameworkCore;

namespace HardwareShopDatabaseImplement.Implements.Storekeeper
{
    public class ComponentStorage : IComponentStorage
    {
        public ComponentViewModel? Delete(ComponentBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var element = context.Components
                .Include(x => x.Builds)
                .ThenInclude(x => x.Build)
                .FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                var componentGoods = context.GoodsComponents.Where(x => x.ComponentId == element.Id).ToList();
                var componentBuilds = context.ComponentsBuilds.Where(x => x.ComponentId == element.Id).ToList();
                componentGoods.ForEach(x =>
                {
                    var good = context.Goods.Include(x => x.Components).FirstOrDefault(rec => rec.Id == x.GoodId);
                    if (good != null)
                        if (good.GoodComponents.Count == 1)
                            context.Goods.Remove(good);
                        else
                        {
                            good.Price -= element.Cost * x.Count;
                            good.Price = Math.Round(good.Price, 2);
                        }
                });
                componentBuilds.ForEach(x =>
                {
                    var build = context.Builds.Include(x => x.Components).FirstOrDefault(rec => rec.Id == x.BuildId);
                    if (build != null)
                    {
                        build.Price -= element.Cost * x.Count;
                        build.Price = Math.Round(build.Price, 2);
                    }
                });
                context.Components.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }

        public ComponentViewModel? GetElement(ComponentSearchModel model)
        {
            using var context = new HardwareShopDatabase();
            if (!string.IsNullOrEmpty(model.ComponentName) || model.Id.HasValue)
                return context.Components
                    .Include(x => x.Builds)
                    .ThenInclude(x => x.Build)
                    .FirstOrDefault(x => (!string.IsNullOrEmpty(model.ComponentName)
                    && x.ComponentName == model.ComponentName) || (model.Id.HasValue && x.Id == model.Id))?.GetViewModel;
            if (model.UserId.HasValue)
                return context.Components
                   .Include(x => x.Builds)
                   .ThenInclude(x => x.Build)
                   .FirstOrDefault(x => x.UserId == model.UserId)?.GetViewModel;
            return null;
        }

        public List<ComponentViewModel> GetFilteredList(ComponentSearchModel model)
        {
            using var context = new HardwareShopDatabase();
            if (model.UserId.HasValue && model.DateFrom.HasValue && model.DateTo.HasValue)
            {
                return context.Components
                    .Include(x => x.Builds)
                    .ThenInclude(x => x.Build)
                    .Where(x => x.UserId == model.UserId && x.DateCreate >= model.DateFrom
                        && x.DateCreate <= model.DateTo)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            if (model.UserId.HasValue)
            {
                return context.Components
                    .Include(x => x.Builds)
                    .ThenInclude(x => x.Build)
                    .Where(x => x.UserId == model.UserId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            if (!string.IsNullOrEmpty(model.ComponentName))
            {
                return context.Components
                    .Include(x => x.Builds)
                    .ThenInclude(x => x.Build)
                    .Where(x => x.ComponentName.Contains(model.ComponentName))
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            return new();
        }

        public List<ComponentViewModel> GetFullList()
        {
            using var context = new HardwareShopDatabase();
            return context.Components
                .Include(x => x.Builds)
                .ThenInclude(x => x.Build)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public ComponentViewModel? Insert(ComponentBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var newComponent = Component.Create(model);
            if (newComponent == null)
            {
                return null;
            }
            context.Components.Add(newComponent);
            context.SaveChanges();
            return context.Components
                .Include(x => x.Builds)
                .ThenInclude(x => x.Build)
                .FirstOrDefault(x => x.Id == newComponent.Id)?.GetViewModel;
        }

        public ComponentViewModel? Update(ComponentBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var component = context.Components
                    .Include(x => x.Builds)
                    .ThenInclude(x => x.Build)
                    .FirstOrDefault(rec => rec.Id == model.Id);
                if (component == null || component.UserId != model.UserId)
                {
                    return null;
                }
                component.Update(model);
                context.SaveChanges();
                if (model.ComponentBuilds == null)
                {
                    context.Builds
                        .Include(x => x.Components)
                        .ThenInclude(x => x.Component)
                        .ToList()
                        .Where(x => x.ContainsComponent(model.Id))
                        .ToList().ForEach(x => { x.Price = x.Components.Sum(x => x.Component.Cost * x.Count); });
                }
                else
                    component.UpdateBuilds(context, model);
                context.Goods
                    .Include(x => x.Components)
                    .ThenInclude(x => x.Component)
                    .ToList()
                    .Where(x => x.ContainsComponent(model.Id))
                    .ToList().ForEach(x => { x.Price = x.Components.Sum(x => x.Component.Cost * x.Count); });
                context.SaveChanges();
                transaction.Commit();
                return component.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public List<Tuple<string, int>> GetComponentBuilds(ComponentSearchModel model)
        {
            if (model == null)
            {
                return new();
            }
            using var context = new HardwareShopDatabase();
            var builds = context.ComponentsBuilds
                .Where(x => x.ComponentId == model.Id)
                .Select(x => Tuple.Create(x.Build.BuildName, x.Count))
                .ToList();
            return builds;
        }

        public List<Tuple<string, int>> GetComponentGoods(ComponentSearchModel model)
        {
            if (model == null)
            {
                return new();
            }
            using var context = new HardwareShopDatabase();
            var goods = context.GoodsComponents
                .Where(x => x.ComponentId == model.Id)
                .Select(x => Tuple.Create(x.Good.GoodName, x.Count))
                .ToList();
            return goods;
        }
    }
}
