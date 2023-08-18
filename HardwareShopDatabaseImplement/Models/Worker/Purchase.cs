using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.ManyToMany;
using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareShopDatabaseImplement.Models.Worker
{
    public class Purchase : IPurchaseModel
    {
        public int Id { get; set; }

        [Required]
        public double Sum { get; set; }

        [Required]
        public PurchaseStatus PurchaseStatus { get; set; } = PurchaseStatus.Неизвестен;

        public DateTime? DatePurchase { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("PurchaseId")]
        public virtual List<PurchaseBuild> Builds { get; set; } = new();

        [ForeignKey("PurchaseId")]
        public virtual List<PurchaseGood> Goods { get; set; } = new();

        private Dictionary<int, (IGoodModel, int)> _purchaseGoods = null!;

        [NotMapped]
        public Dictionary<int, (IGoodModel, int)> PurchaseGoods
        {
            get
            {
                if (_purchaseGoods == null)
                {
                    _purchaseGoods = Goods.ToDictionary(recPG => recPG.GoodId, recPG => (recPG.Good as IGoodModel, recPG.Count));
                }
                return _purchaseGoods;
            }
        }

        private Dictionary<int, (IBuildModel, int)> _purchaseBuilds = null!;

        [NotMapped]
        public Dictionary<int, (IBuildModel, int)> PurchaseBuilds
        {
            get
            {
                if (_purchaseBuilds == null)
                {
                    _purchaseBuilds = Builds.ToDictionary(recPG => recPG.BuildId, recPG => (recPG.Build as IBuildModel, recPG.Count));
                }
                return _purchaseBuilds;
            }
        }

        public static Purchase Create(HardwareShopDatabase context, PurchaseBindingModel model)
        {
            return new Purchase()
            {
                Id = model.Id,
                Sum = model.Sum,
                PurchaseStatus = model.PurchaseStatus,
                DatePurchase = model.DatePurchase,
                UserId = model.UserId,
                Goods = model.PurchaseGoods.Select(x => new PurchaseGood
                {
                    Good = context.Goods.First(y => y.Id == x.Key),
                    Count = x.Value.Item2
                }).ToList(),
            };
        }

        public void Update(PurchaseBindingModel model)
        {
            PurchaseStatus = model.PurchaseStatus;
            DatePurchase = model.DatePurchase;
            Sum = model.Sum;
        }

        public PurchaseViewModel GetViewModel => new()
        {
            Id = Id,
            Sum = Sum,
            PurchaseStatus = PurchaseStatus,
            DatePurchase = DatePurchase,
            UserId = UserId,
            PurchaseGoods = PurchaseGoods,
            PurchaseBuilds = PurchaseBuilds
        };

        public void UpdateGoods(HardwareShopDatabase context, PurchaseBindingModel model)
        {
            var purchaseGoods = context.PurchasesGoods.Where(rec => rec.PurchaseId == model.Id).ToList();
            if (purchaseGoods != null && purchaseGoods.Count > 0)
            {   // удалили те в бд, которых нет в модели
                context.PurchasesGoods.RemoveRange(purchaseGoods.Where(rec => !model.PurchaseGoods.ContainsKey(rec.GoodId)));
                context.SaveChanges();
                purchaseGoods = context.PurchasesGoods.Where(rec => rec.PurchaseId == model.Id).ToList();
                // обновили количество у существующих записей
                foreach (var updateGood in purchaseGoods)
                {
                    updateGood.Count = model.PurchaseGoods[updateGood.GoodId].Item2;
                    model.PurchaseGoods.Remove(updateGood.GoodId);
                }
                context.SaveChanges();
            }
            var purchase = context.Purchases.First(x => x.Id == Id);
            //добавляем в бд товары которые есть в моделе, но ещё нет в бд
            foreach (var dc in model.PurchaseGoods)
            {
                context.PurchasesGoods.Add(new PurchaseGood
                {
                    Purchase = purchase,
                    Good = context.Goods.First(x => x.Id == dc.Key),
                    Count = dc.Value.Item2
                });
                context.SaveChanges();
            }
            _purchaseGoods = null;
        }
    }
}
