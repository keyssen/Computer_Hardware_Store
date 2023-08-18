using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public interface IComponentLogic
    {
        List<ComponentViewModel>? ReadList(ComponentSearchModel? model);
        ComponentViewModel? ReadElement(ComponentSearchModel model);
        bool Create(ComponentBindingModel model);
        bool Update(ComponentBindingModel model);
        bool Delete(ComponentBindingModel model);
    }
}
