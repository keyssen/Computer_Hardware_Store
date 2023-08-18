using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.ManyToMany;
using HardwareShopDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareShopDatabaseImplement.Models.Storekeeper
{
    public class Component : IComponentModel
    {
        public int Id { get; set; }

        [Required]
        public string ComponentName { get; set; } = string.Empty;

        [Required]
        public double Cost { get; set; }

        [Required]
        public int UserId { get; set; }
        [Required]
        public DateTime DateCreate { get; set; }

        [ForeignKey("ComponentId")]
        public virtual List<GoodComponent> Goods { get; set; } = new();

        [ForeignKey("ComponentId")]
        public virtual List<ComponentBuild> Builds { get; set; } = new();

        private Dictionary<int, (IBuildModel, int)>? _componentBuilds = null;

        [NotMapped]
        public Dictionary<int, (IBuildModel, int)> ComponentBuilds
        {
            get
            {
                if (_componentBuilds == null)
                {
                    _componentBuilds = Builds
                            .ToDictionary(recCB => recCB.BuildId, recCB => (recCB.Build as IBuildModel, recCB.Count));
                }
                return _componentBuilds;
            }
        }

        public static Component? Create(ComponentBindingModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Component()
            {
                Id = model.Id,
                ComponentName = model.ComponentName,
                Cost = model.Cost,
                UserId = model.UserId,
                DateCreate = model.DateCreate
            };
        }

        public void Update(ComponentBindingModel model)
        {
            if (model == null)
            {
                return;
            }
            ComponentName = model.ComponentName;
            Cost = model.Cost;
        }

        public ComponentViewModel GetViewModel => new()
        {
            Id = Id,
            ComponentName = ComponentName,
            Cost = Cost,
            UserId = UserId,
            ComponentBuilds = ComponentBuilds,
            DateCreate = DateCreate
        };

        public void UpdateBuilds(HardwareShopDatabase context, ComponentBindingModel model)
        {
            var componentBuilds = context.ComponentsBuilds.Where(rec => rec.ComponentId == model.Id).ToList();
            if (componentBuilds != null && componentBuilds.Count > 0)
            {   // удалили те, которых нет в модели
                context.ComponentsBuilds.RemoveRange(componentBuilds.Where(rec =>
                {
                    bool flag = !model.ComponentBuilds.ContainsKey(rec.BuildId);
                    if (flag)
                    {
                        var build = context.Builds.First(x => x.Id == rec.Build.Id);
                        if (build != null)
                        {
                            build.Price -= rec.Component.Cost * rec.Count;
                        }
                        context.SaveChanges();
                    }
                    return flag;
                })); 
                context.SaveChanges();
                componentBuilds = context.ComponentsBuilds.Where(rec => rec.ComponentId == model.Id).ToList();
                // обновили количество у существующих записей
                foreach (var updateBuild in componentBuilds)
                { 
                    var build = context.Builds.First(x => x.Id == updateBuild.Build.Id);
                    if (updateBuild.Count > model.ComponentBuilds[updateBuild.BuildId].Item2)
                    {
                        build.Price -= (updateBuild.Count - model.ComponentBuilds[updateBuild.BuildId].Item2) * updateBuild.Component.Cost;
                    }
                    else if (updateBuild.Count < model.ComponentBuilds[updateBuild.BuildId].Item2)
                    {
                        build.Price += (model.ComponentBuilds[updateBuild.BuildId].Item2 - updateBuild.Count) * updateBuild.Component.Cost;
                    }
                    updateBuild.Count = model.ComponentBuilds[updateBuild.BuildId].Item2;
                    model.ComponentBuilds.Remove(updateBuild.BuildId);
                }
                context.SaveChanges();
            }
            var component = context.Components.First(x => x.Id == Id);
            foreach (var cb in model.ComponentBuilds)
            {
                context.ComponentsBuilds.Add(new ComponentBuild
                {
                    Component = component,
                    Build = context.Builds.First(x => x.Id == cb.Key),
                    Count = cb.Value.Item2
                });

                var build = context.Builds.First(x => x.Id == cb.Key);
                if (build != null)
                {
                    build.Price += cb.Value.Item2 * component.Cost;
                }
                context.SaveChanges();
            }
            _componentBuilds = null;
        }
    }
}
