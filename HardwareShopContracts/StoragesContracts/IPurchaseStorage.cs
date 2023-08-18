using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.StoragesContracts
{
    public interface IPurchaseStorage
    {
        List<PurchaseViewModel> GetFullList();
        List<PurchaseViewModel> GetFilteredList(PurchaseSearchModel model);
        List<PurchaseViewModel> GetReportFilteredList(PurchaseSearchModel model);
        PurchaseViewModel? GetElement(PurchaseSearchModel model);
        PurchaseViewModel? Insert(PurchaseBindingModel model);
        PurchaseViewModel? Update(PurchaseBindingModel model, bool withParams = true);
        PurchaseViewModel? Delete(PurchaseBindingModel model);
    }
}
