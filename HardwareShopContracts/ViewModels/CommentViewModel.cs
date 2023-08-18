using HardwareShopDataModels.Models;
using System.ComponentModel;

namespace HardwareShopContracts.ViewModels
{
    public class CommentViewModel : ICommentModel
    {
        public int Id { get; set; }

        [DisplayName("Комментарий")]
        public string Text { get; set; } = string.Empty;

        [DisplayName("Название сборки")]
        public string BuildName { get; set; } = string.Empty;

        public int BuildId { get; set; }

        public int UserId { get; set; }
    }
}
