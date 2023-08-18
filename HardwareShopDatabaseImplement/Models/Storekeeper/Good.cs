using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.ManyToMany;
using HardwareShopDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareShopDatabaseImplement.Models.Storekeeper
{
    public class Good : IGoodModel
    {
        public int Id { get; set; }

        [Required]
        public string GoodName { get; set; } = string.Empty;

        [Required]
        public double Price { get; set; }

        [Required]
        public int UserId { get; set; }

        private Dictionary<int, (IComponentModel, int)>? _goodComponents = null;

        [ForeignKey("GoodId")]
        public virtual List<GoodComponent> Components { get; set; } = new();

        [ForeignKey("GoodId")]
        public virtual List<Order> Orders { get; set; } = new();

        [ForeignKey("GoodId")]
        public virtual List<PurchaseGood> Purchases { get; set; } = new();

        [NotMapped]
        public Dictionary<int, (IComponentModel, int)> GoodComponents
        {
            get
            {
                if (_goodComponents == null)
                {
                    _goodComponents = Components
                            .ToDictionary(recGC => recGC.ComponentId, recGC => (recGC.Component as IComponentModel, recGC.Count));
                }
                return _goodComponents;
            }
        }

        public static Good Create(HardwareShopDatabase context, GoodBindingModel model)
        {
            return new Good()
            {
                Id = model.Id,
                GoodName = model.GoodName,
                Price = model.Price,
                UserId = model.UserId,
                Components = model.GoodComponents.Select(x => new GoodComponent
                {
                    Component = context.Components.First(y => y.Id == x.Key),
                    Count = x.Value.Item2
                }).ToList()
            };
        }

        public void Update(GoodBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            GoodName = model.GoodName;
            Price = model.Price;
        }

        public bool ContainsComponent(int componentId)
        {
            return GoodComponents.ContainsKey(componentId);
        }

        public GoodViewModel GetViewModel => new()
        {
            Id = Id,
            GoodName = GoodName,
            Price = Price,
            UserId = UserId,
            GoodComponents = GoodComponents
        };

        public void UpdateComponents(HardwareShopDatabase context, GoodBindingModel model)
        {
            var goodComponents = context.GoodsComponents
                .Where(rec => rec.GoodId == model.Id).ToList();
            if (goodComponents != null && goodComponents.Count > 0)
            {   // удалили те, которых нет в модели
                context.GoodsComponents
                    .RemoveRange(goodComponents
                    .Where(rec => !model.GoodComponents.ContainsKey(rec.ComponentId)));
                context.SaveChanges();
                goodComponents = context.GoodsComponents
                    .Where(rec => rec.GoodId == model.Id).ToList();
                // обновили количество у существующих записей
                foreach (var updateComponent in goodComponents)
                {
                    updateComponent.Count = model.GoodComponents[updateComponent.ComponentId].Item2;
                    model.GoodComponents.Remove(updateComponent.ComponentId);
                }
                context.SaveChanges();
            }
            var good = context.Goods.First(x => x.Id == Id);
            foreach (var gc in model.GoodComponents)
            {
                context.GoodsComponents.Add(new GoodComponent
                {
                    Good = good,
                    Component = context.Components.First(x => x.Id == gc.Key),
                    Count = gc.Value.Item2
                });
                context.SaveChanges();
            }
            _goodComponents = null;
        }
    }
}
