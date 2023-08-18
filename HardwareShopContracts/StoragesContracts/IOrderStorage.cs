using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.StoragesContracts
{
    public interface IOrderStorage
    {
        List<OrderViewModel> GetFullList();
        List<OrderViewModel> GetFilteredList(OrderSearchModel model);
        OrderViewModel? GetElement(OrderSearchModel model);
        OrderViewModel? Insert(OrderBindingModel model);
        OrderViewModel? Update(OrderBindingModel model);
        OrderViewModel? Delete(OrderBindingModel model);
    }
}
