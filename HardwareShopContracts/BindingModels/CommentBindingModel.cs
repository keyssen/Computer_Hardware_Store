using HardwareShopDataModels.Models;

namespace HardwareShopContracts.BindingModels
{
    public class CommentBindingModel : ICommentModel
    {
        public int Id { get; set; }

        public string Text { get; set; } = string.Empty;

        public int BuildId { get; set; }

        public int UserId { get; set; }
    }
}
