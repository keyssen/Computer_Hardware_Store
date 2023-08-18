using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.ManyToMany;
using HardwareShopDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareShopDatabaseImplement.Models.Worker
{
    public class Build : IBuildModel
    {
        public int Id { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string BuildName { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [ForeignKey("BuildId")]
        public virtual List<Comment> Comments { get; set; } = new();

        [ForeignKey("BuildId")]
        public virtual List<ComponentBuild> Components { get; set; } = new();

        [ForeignKey("BuildId")]
        public virtual List<PurchaseBuild> Purchases { get; set; } = new();

        private Dictionary<int, ICommentModel> _buildComments = null!;

        [NotMapped]
        public Dictionary<int, ICommentModel> BuildComments
        {
            get
            {
                if (_buildComments == null)
                {
                    _buildComments = Comments.ToDictionary(recBC => recBC.Id, recBC => recBC as ICommentModel);
                }
                return _buildComments;
            }
        }

        private Dictionary<int, (IComponentModel, int)> _buildComponents = null!;

        [NotMapped]
        public Dictionary<int, (IComponentModel, int)> BuildComponents
        {
            get
            {
                if (_buildComponents == null)
                {
                    _buildComponents = Components.ToDictionary(recBC => recBC.ComponentId, recBC => (recBC.Component as IComponentModel, recBC.Count));
                }
                return _buildComponents;
            }
        }

        private Dictionary<int, (IPurchaseModel, int)> _buildPurchases = null!;

        [NotMapped]
        public Dictionary<int, (IPurchaseModel, int)> BuildPurchases
        {
            get
            {
                if (_buildPurchases == null)
                {
                    _buildPurchases = Purchases.ToDictionary(recBP => recBP.PurchaseId, recBP => (recBP.Purchase as IPurchaseModel, recBP.Count));
                }
                return _buildPurchases;
            }
        }

        public static Build Create(BuildBindingModel model)
        {
            return new Build()
            {
                Id = model.Id,
                BuildName = model.BuildName,
                UserId = model.UserId,
                Price = model.Price
            };
        }

        public void Update(BuildBindingModel model)
        {
            BuildName = model.BuildName;
            Price = model.Price;
        }

        public bool ContainsComponent(int componentId)
        {
            return BuildComponents.ContainsKey(componentId);
        }

        public BuildViewModel GetViewModel => new()
        {
            Id = Id,
            BuildName = BuildName,
            Price = Price,
            UserId = UserId,
            BuildPurchases = BuildPurchases,
            BuildComponents = BuildComponents,
            BuildComments = BuildComments
        };

        public void UpdatePurchases(HardwareShopDatabase context, BuildBindingModel model)
        {
            var buildPurchases = context.PurchasesBuilds.Where(rec => rec.BuildId == model.Id).ToList();
            if (buildPurchases != null && buildPurchases.Count > 0)
            {   // удалили те в бд, которых нет в модели
                context.PurchasesBuilds.RemoveRange(buildPurchases.Where(rec => !model.BuildPurchases.ContainsKey(rec.PurchaseId)));
                context.SaveChanges();
                buildPurchases = context.PurchasesBuilds.Where(rec => rec.BuildId == model.Id).ToList();
                // обновили количество у существующих записей
                foreach (var updateComponent in buildPurchases)
                {
                    updateComponent.Count = model.BuildPurchases[updateComponent.PurchaseId].Item2;
                    model.BuildPurchases.Remove(updateComponent.PurchaseId);
                }
                context.SaveChanges();
            }
            var build = context.Builds.First(x => x.Id == Id);
            //добавляем в бд сборки которые есть в модели, но ещё нет в бд
            foreach (var bp in model.BuildPurchases)
            {
                context.PurchasesBuilds.Add(new PurchaseBuild
                {
                    Build = build,
                    Purchase = context.Purchases.First(x => x.Id == bp.Key),
                    Count = bp.Value.Item2
                });
                var purchase = context.Purchases.First(x => x.Id == bp.Key);
                if (purchase != null)
                {
                    purchase.Sum += bp.Value.Item2 * build.Price;
                }
                context.SaveChanges();

            }
            _buildPurchases = null;
        }


        public void UpdateSumPurchase(HardwareShopDatabase context, BuildBindingModel model)
        {
            var buildPurchases = context.PurchasesBuilds.Where(rec => rec.BuildId == model.Id).ToList();

        }
    }
}
