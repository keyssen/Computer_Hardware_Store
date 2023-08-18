using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public interface IUserLogic
    {
        List<UserViewModel>? ReadList(UserSearchModel? model);
        UserViewModel? ReadElement(UserSearchModel model);
        bool Create(UserBindingModel model);
        bool Update(UserBindingModel model);
        bool Delete(UserBindingModel model);
    }
}