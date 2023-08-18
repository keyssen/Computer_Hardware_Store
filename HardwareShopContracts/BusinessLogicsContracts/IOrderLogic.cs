using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.BuisnessLogicsContracts
{
    public interface IOrderLogic
    {
        List<OrderViewModel>? ReadList(OrderSearchModel? model);
        bool CreateOrder(OrderBindingModel model);
        bool TakeOrderInWork(OrderBindingModel model);
        bool FinishOrder(OrderBindingModel model);
        bool DeliveryOrder(OrderBindingModel model);
        bool Delete(OrderBindingModel model);
    }
}
