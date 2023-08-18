using HardwareShopContracts.ViewModels;
using HardwareShopDataModels.Models;

namespace HardwareShopContracts.BindingModels
{
    public class ComponentBindingModel : IComponentModel
    {
        public int Id { get; set; }

        public string ComponentName { get; set; } = string.Empty;

        public double Cost { get; set; }

        public int UserId { get; set; }

        public DateTime DateCreate { get; set; } = DateTime.Now;

        public Dictionary<int, (IBuildModel, int)> ComponentBuilds
        {
            get;
            set;
        } = new();

        public List<BuildViewModel> ComponentBuildsBuilds
        {
            get;
            set;
        } = new();

        public List<int> ComponentBuildsCounts
        {
            get;
            set;
        } = new();
    }
}
