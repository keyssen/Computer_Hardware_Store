using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.StoragesContracts
{
    public interface ICommentStorage
    {
        List<CommentViewModel> GetFullList();
        List<CommentViewModel> GetFilteredList(CommentSearchModel model);
        CommentViewModel? GetElement(CommentSearchModel model);
        CommentViewModel? Insert(CommentBindingModel model);
        CommentViewModel? Update(CommentBindingModel model);
        CommentViewModel? Delete(CommentBindingModel model);
    }
}