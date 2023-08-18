using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using Microsoft.EntityFrameworkCore;

namespace HardwareShopDatabaseImplement.Implements.Storekeeper
{
    public class OrderStorage : IOrderStorage
    {
        public OrderViewModel? Delete(OrderBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var element = context.Orders
                .Include(x => x.Good)
                .FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Orders.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }

        public OrderViewModel? GetElement(OrderSearchModel model)
        {
            if (!model.Id.HasValue)
            {
                return null;
            }
            using var context = new HardwareShopDatabase();
            return context.Orders
                .Include(x => x.Good)
                .FirstOrDefault(x => model.Id.HasValue && x.Id == model.Id)
                ?.GetViewModel;
        }

        public List<OrderViewModel> GetFilteredList(OrderSearchModel model)
        {
            using var context = new HardwareShopDatabase();
            if (model.UserId.HasValue)
            {
                return context.Orders
                    .Include(x => x.Good)
                    .Where(x => x.UserId == model.UserId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            return new();
        }

        public List<OrderViewModel> GetFullList()
        {
            using var context = new HardwareShopDatabase();
            return context.Orders
                .Include(x => x.Good)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public OrderViewModel? Insert(OrderBindingModel model)
        {
            var newOrder = Order.Create(model);
            if (newOrder == null)
            {
                return null;
            }
            using var context = new HardwareShopDatabase();
            context.Orders.Add(newOrder);
            context.SaveChanges();
            return context.Orders
                          .Include(x => x.Good)
                          .FirstOrDefault(x => x.Id == newOrder.Id)
                          ?.GetViewModel;
        }

        public OrderViewModel? Update(OrderBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var order = context.Orders
                .Include(x => x.Good)
                .FirstOrDefault(x => x.Id == model.Id);
            if (order == null)
            {
                return null;
            }
            order.Update(model);
            context.SaveChanges();
            return order.GetViewModel;
        }
    }
}
