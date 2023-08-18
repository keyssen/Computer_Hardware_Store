using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.Worker;
using Microsoft.EntityFrameworkCore;

namespace HardwareShopDatabaseImplement.Implements.Worker
{
    public class PurchaseStorage : IPurchaseStorage
    {
        public List<PurchaseViewModel> GetFullList()
        {
            using var context = new HardwareShopDatabase();
            return context.Purchases
                    .Include(x => x.Goods)
                    .ThenInclude(x => x.Good)
                    .Select(x => x.GetViewModel)
                    .ToList();
        }

        public List<PurchaseViewModel> GetFilteredList(PurchaseSearchModel model)
        {
            using var context = new HardwareShopDatabase();
            if (model.UserId.HasValue && model.DateFrom.HasValue && model.DateTo.HasValue)
            {
                return context.Purchases
                    .Include(x => x.Builds)
                    .ThenInclude(x => x.Build)
                    .ThenInclude(x => x.Components)
                    .ThenInclude(x => x.Component)
                    .Include(x => x.Builds)
                    .ThenInclude(x => x.Build)
                    .ThenInclude(x => x.Comments)
                    .Where(x => x.UserId == model.UserId && x.DatePurchase >= model.DateFrom && x.DatePurchase <= model.DateTo)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            if (model.UserId.HasValue && model.PurchaseStatus.HasValue)
            {
                return context.Purchases
                    .Include(x => x.Goods)
                    .ThenInclude(x => x.Good)
                    .Where(x => x.PurchaseStatus == model.PurchaseStatus && x.UserId == model.UserId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            if (model.UserId.HasValue)
                return context.Purchases
                    .Include(x => x.Goods)
                    .ThenInclude(x => x.Good)
                    .Where(x => x.UserId == model.UserId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            return context.Purchases
                   .Include(x => x.Goods)
                   .ThenInclude(x => x.Good)
                   .Where(x => x.PurchaseStatus == model.PurchaseStatus)
                   .Select(x => x.GetViewModel)
                   .ToList();
        }

        public List<PurchaseViewModel> GetReportFilteredList(PurchaseSearchModel model)
        {
            using var context = new HardwareShopDatabase();
            if (!model.UserId.HasValue)
            {
                return new();
            }
            return context.Purchases
                .Include(x => x.Builds)
                .ThenInclude(x => x.Build)
                .ThenInclude(x => x.Components)
                .ThenInclude(x => x.Component)
                .Where(x => x.UserId == model.UserId)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public PurchaseViewModel? GetElement(PurchaseSearchModel model)
        {
            if (!model.Id.HasValue)
            {
                return null;
            }
            using var context = new HardwareShopDatabase();
            return context.Purchases
                .Include(x => x.Goods)
                .ThenInclude(x => x.Good)
				.Include(x => x.Builds)
				.ThenInclude(x => x.Build)
				.ThenInclude(x => x.Components)
				.ThenInclude(x => x.Component)
                .Include(x => x.Builds)
                .ThenInclude(x => x.Build)
                .ThenInclude(x => x.Comments)
                .FirstOrDefault(x => model.Id.HasValue && x.Id == model.Id)
                ?.GetViewModel;
        }

        public PurchaseViewModel? Insert(PurchaseBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var newPurchase = Purchase.Create(context, model);
            if (newPurchase == null)
            {
                return null;
            }
            context.Purchases.Add(newPurchase);
            context.SaveChanges();
            return context.Purchases
                .Include(x => x.Goods)
                .ThenInclude(x => x.Good)
                .FirstOrDefault(x => x.Id == newPurchase.Id)
                 ?.GetViewModel;
        }

        public PurchaseViewModel? Update(PurchaseBindingModel model, bool withParams = true)
        {
            using var context = new HardwareShopDatabase();
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var purchase = context.Purchases
                    .Include(x => x.Goods)
                    .ThenInclude(x => x.Good)
                    .FirstOrDefault(x => x.Id == model.Id && x.UserId == model.UserId);
                if (purchase == null)
                {
                    return null;
                }
                purchase.Update(model);
                context.SaveChanges();
                if (!withParams) {
					transaction.Commit();
					return purchase.GetViewModel;
				} 
				purchase.UpdateGoods(context, model);
                transaction.Commit();
                return purchase.GetViewModel;
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        public PurchaseViewModel? Delete(PurchaseBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var element = context.Purchases
                .Include(x => x.Goods)
                .ThenInclude(x => x.Good)
                .FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Purchases.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }
    }
}
