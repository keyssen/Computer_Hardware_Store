using HardwareShopContracts.ViewModels;
using HardwareShopDataModels.Models;

namespace HardwareShopContracts.BindingModels
{
    public class GoodBindingModel : IGoodModel
    {
        public int Id { get; set; }

        public string GoodName { get; set; } = string.Empty;

        public double Price { get; set; }

        public int UserId { get; set; }

        public Dictionary<int, (IComponentModel, int)> GoodComponents
        {
            get;
            set;
        } = new();

        // for dictionary item1
        public List<ComponentViewModel> GoodComponentsComponents
        {
            get;
            set;
        } = new();

        // for dictionary item2
        public List<int> GoodComponentsCounts
        {
            get;
            set;
        } = new();

        // for report list
        public List<GoodViewModel> Goods
        {
            get;
            set;
        } = new();
    }
}
