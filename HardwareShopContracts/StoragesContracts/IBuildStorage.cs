using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.StoragesContracts
{
    public interface IBuildStorage
    {
        List<BuildViewModel> GetFullList();
        List<BuildViewModel> GetFilteredList(BuildSearchModel model);
        BuildViewModel? GetElement(BuildSearchModel model);
        BuildViewModel? Insert(BuildBindingModel model);
        BuildViewModel? Update(BuildBindingModel model);
        BuildViewModel? Delete(BuildBindingModel model);
    }
}