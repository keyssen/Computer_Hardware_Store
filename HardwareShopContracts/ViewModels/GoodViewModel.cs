using HardwareShopDataModels.Models;
using System.ComponentModel;

namespace HardwareShopContracts.ViewModels
{
    public class GoodViewModel : IGoodModel
    {
        public int Id { get; set; }
        [DisplayName("Товар")]
        public string GoodName { get; set; } = string.Empty;
        [DisplayName("Цена")]
        public double Price { get; set; }
        public int UserId { get; set; }
        public Dictionary<int, (IComponentModel, int)> GoodComponents
        {
            get;
            set;
        } = new();
    }
}
