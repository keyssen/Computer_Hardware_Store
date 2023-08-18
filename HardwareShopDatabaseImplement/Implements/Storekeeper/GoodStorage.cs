using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using Microsoft.EntityFrameworkCore;

namespace HardwareShopDatabaseImplement.Implements.Storekeeper
{
    public class GoodStorage : IGoodStorage
    {
        public GoodViewModel? Delete(GoodBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var element = context.Goods
                .Include(x => x.Components)
                .ThenInclude(x => x.Component)
                .FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Goods.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }

        public GoodViewModel? GetElement(GoodSearchModel model)
        {
            using var context = new HardwareShopDatabase();
            if (model.UserId.HasValue && model.Id.HasValue)
                return context.Goods
                .Include(x => x.Components)
                .ThenInclude(x => x.Component)
                .FirstOrDefault(x => x.UserId == model.UserId && model.Id == x.Id)
                ?.GetViewModel;
            if (!string.IsNullOrEmpty(model.GoodName) || model.Id.HasValue)
                return context.Goods
                    .Include(x => x.Components)
                    .ThenInclude(x => x.Component)
                    .FirstOrDefault(x => (!string.IsNullOrEmpty(model.GoodName) && x.GoodName == model.GoodName) ||
                                    (model.Id.HasValue && x.Id == model.Id))
                    ?.GetViewModel;
            return null;
        }

        public List<GoodViewModel> GetFilteredList(GoodSearchModel model)
        {
            using var context = new HardwareShopDatabase();
            if (model.UserId.HasValue)
                return context.Goods
                    .Include(x => x.Components)
                    .ThenInclude(x => x.Component)
                    .Where(x => x.UserId == model.UserId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            if (!string.IsNullOrEmpty(model.GoodName))
                return context.Goods
                    .Include(x => x.Components)
                    .ThenInclude(x => x.Component)
                    .Where(x => x.GoodName.Contains(model.GoodName))
                    .Select(x => x.GetViewModel)
                    .ToList();
            return new();
        }

        public List<GoodViewModel> GetFullList()
        {
            using var context = new HardwareShopDatabase();
            return context.Goods
                .Include(x => x.Components)
                .ThenInclude(x => x.Component)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public GoodViewModel? Insert(GoodBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var newGood = Good.Create(context, model);
            if (newGood == null)
            {
                return null;
            }
            context.Goods.Add(newGood);
            context.SaveChanges();
            return context.Goods
                .Include(x => x.Components)
                .ThenInclude(x => x.Component)
                .FirstOrDefault(x => x.Id == newGood.Id)?.GetViewModel;
        }

        public GoodViewModel? Update(GoodBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var good = context.Goods
                    .Include(x => x.Components)
                    .ThenInclude(x => x.Component)
                    .FirstOrDefault(rec => rec.Id == model.Id);
                if (good == null)
                {
                    return null;
                }
                good.Update(model);
                context.SaveChanges();
                good.UpdateComponents(context, model);
                transaction.Commit();
                return good.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
