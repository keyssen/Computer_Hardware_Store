using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public interface ICommentLogic
    {
        List<CommentViewModel>? ReadList(CommentSearchModel? model);
        CommentViewModel? ReadElement(CommentSearchModel model);
        bool Create(CommentBindingModel model);
        bool Update(CommentBindingModel model);
        bool Delete(CommentBindingModel model);
    }
}