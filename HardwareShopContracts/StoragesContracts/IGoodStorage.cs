using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.StoragesContracts
{
    public interface IGoodStorage
    {
        List<GoodViewModel> GetFullList();
        List<GoodViewModel> GetFilteredList(GoodSearchModel model);
        GoodViewModel? GetElement(GoodSearchModel model);
        GoodViewModel? Insert(GoodBindingModel model);
        GoodViewModel? Update(GoodBindingModel model);
        GoodViewModel? Delete(GoodBindingModel model);
    }
}
