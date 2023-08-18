using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public interface IPurchaseLogic
    {
        List<PurchaseViewModel>? ReadList(PurchaseSearchModel? model);
        List<PurchaseViewModel>? ReadOrderList(PurchaseSearchModel model);
        PurchaseViewModel? ReadElement(PurchaseSearchModel model);
        bool Create(PurchaseBindingModel model);
        bool Update(PurchaseBindingModel model);
        bool Delete(PurchaseBindingModel model);
        bool DeliveryPurchase(PurchaseBindingModel model);
    }
}
