using HardwareShopDataModels.Models;
using System.ComponentModel;
namespace HardwareShopContracts.ViewModels
{
    public class BuildViewModel : IBuildModel
    {
        public int Id { get; set; }

        [DisplayName("Цена")]
        public double Price { get; set; }

        [DisplayName("Название Сборки")]
        public string BuildName { get; set; } = string.Empty;

        public int UserId { get; set; }

        public Dictionary<int, (IPurchaseModel, int)> BuildPurchases { get; set; } = new();

        public Dictionary<int, (IComponentModel, int)> BuildComponents { get; set; } = new();

        public Dictionary<int, ICommentModel> BuildComments { get; set; } = new();
    }
}
