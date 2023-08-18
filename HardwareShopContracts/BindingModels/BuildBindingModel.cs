using HardwareShopDataModels.Models;

namespace HardwareShopContracts.BindingModels
{
    public class BuildBindingModel : IBuildModel
    {
        public int Id { get; set; }

        public double Price { get; set; }

        public string BuildName { get; set; } = string.Empty;

        public int UserId { get; set; }

        public Dictionary<int, (IPurchaseModel, int)> BuildPurchases { get; set; } = new();

        public Dictionary<int, (IComponentModel, int)> BuildComponents { get; set; } = new();

        public Dictionary<int, ICommentModel> BuildComments { get; set; } = new();
    }
}
