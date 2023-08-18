using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public interface IBuildLogic
    {
        List<BuildViewModel>? ReadList(BuildSearchModel? model);
        BuildViewModel? ReadElement(BuildSearchModel model);
        bool Create(BuildBindingModel model);
        bool Update(BuildBindingModel model);
        bool Delete(BuildBindingModel model);
    }
}
