using HardwareShopDataModels.Models;
using System.ComponentModel;

namespace HardwareShopContracts.ViewModels
{
    public class ComponentViewModel : IComponentModel
    {
        public int Id { get; set; }
        [DisplayName("Компонент")]
        public string ComponentName { get; set; } = string.Empty;
        [DisplayName("Стоимость")]
        public double Cost { get; set; }
        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; } = DateTime.Now;
        public int UserId { get; set; }
        public Dictionary<int, (IBuildModel, int)> ComponentBuilds
        {
            get;
            set;
        } = new();
    }
}
