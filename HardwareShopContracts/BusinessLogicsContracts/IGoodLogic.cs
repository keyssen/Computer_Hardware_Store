using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public interface IGoodLogic
    {
        List<GoodViewModel>? ReadList(GoodSearchModel? model);
        GoodViewModel? ReadElement(GoodSearchModel model);
        bool Create(GoodBindingModel model);
        bool Update(GoodBindingModel model);
        bool Delete(GoodBindingModel model);
    }
}
