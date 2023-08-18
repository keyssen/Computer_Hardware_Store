using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDatabaseImplement.Models.Worker;
using HardwareShopDataModels.Enums;
using Microsoft.EntityFrameworkCore;

namespace HardwareShopDatabaseImplement.Implements.Worker
{
    public class BuildStorage : IBuildStorage
    {
        public List<BuildViewModel> GetFullList()
        {
            using var context = new HardwareShopDatabase();
            return context.Builds
                .Include(x => x.Purchases)
                .ThenInclude(x => x.Purchase)
                .ToList()
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<BuildViewModel> GetFilteredList(BuildSearchModel model)
        {
            if (string.IsNullOrEmpty(model.BuildName) && !model.UserId.HasValue)
            {
                return new();
            }
            using var context = new HardwareShopDatabase();
            if (!string.IsNullOrEmpty(model.BuildName))
            {
                return context.Builds
                    .Include(x => x.Purchases)
                    .ThenInclude(x => x.Purchase)
                    .Where(x => x.BuildName.Contains(model.BuildName))
                    .ToList()
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            return context.Builds
                .Include(x => x.Purchases)
                .ThenInclude(x => x.Purchase)
                .Where(x => x.UserId == model.UserId)
                .ToList()
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public BuildViewModel? GetElement(BuildSearchModel model)
        {
            if (string.IsNullOrEmpty(model.BuildName) && !model.Id.HasValue)
            {
                return null;
            }
            using var context = new HardwareShopDatabase();
            var result = context.Builds
                .Include(x => x.Purchases)
                .ThenInclude(x => x.Purchase)
                .FirstOrDefault(x => (!string.IsNullOrEmpty(model.BuildName) && x.BuildName == model.BuildName) ||
                (model.Id.HasValue && x.Id == model.Id))
                ?.GetViewModel;
            return result;
		}

        public BuildViewModel? Insert(BuildBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var newBuild = Build.Create(model);
            if (newBuild == null)
            {
                return null;
            }
            context.Builds.Add(newBuild);
            context.SaveChanges();
            return context.Builds
                .Include(x => x.Purchases)
                .ThenInclude(x => x.Purchase)
                .FirstOrDefault(x => x.Id == newBuild.Id)
                ?.GetViewModel;
        }

        public BuildViewModel? Update(BuildBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var build = context.Builds
                    .Include(x => x.Purchases)
                    .ThenInclude(x => x.Purchase)
                    .FirstOrDefault(x => x.Id == model.Id && x.UserId == model.UserId);
                if (build == null)
                {
                    return null;
                }
                build.Update(model);
                context.SaveChanges();
                build.UpdatePurchases(context, model);
                transaction.Commit();
                return build?.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public BuildViewModel? Delete(BuildBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var element = context.Builds
                    .Include(x => x.Purchases)
                    .ThenInclude(x => x.Purchase)
                    .FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
				var buildPurchases = context.PurchasesBuilds.Where(x => x.BuildId == element.Id).ToList();
				buildPurchases.ForEach(x =>
				{
					var purchase = context.Purchases.Include(x => x.Builds).FirstOrDefault(rec => rec.Id == x.PurchaseId && rec.PurchaseStatus != PurchaseStatus.Выдан);
					if (purchase != null)
                    {
						purchase.Sum -= element.Price * x.Count;
						purchase.Sum = Math.Round(purchase.Sum, 2);
					}
				});
				context.Builds.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }
    }
}