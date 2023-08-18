using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.StoragesContracts
{
    public interface IUserStorage
    {
        List<UserViewModel> GetFullList();
        List<UserViewModel> GetFilteredList(UserSearchModel model);
        UserViewModel? GetElement(UserSearchModel model);
        UserViewModel? Insert(UserBindingModel model);
        UserViewModel? Update(UserBindingModel model);
        UserViewModel? Delete(UserBindingModel model);
    }
}