using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDataModels.Models;
using System.ComponentModel.DataAnnotations;

namespace HardwareShopDatabaseImplement.Models.Worker
{
    public class Comment : ICommentModel
    {
        public int Id { get; set; }

        [Required]
        public string Text { get; set; } = string.Empty;

        [Required]
        public int BuildId { get; set; }

        public virtual Build Build { get; set; } = null!;

        [Required]
        public int UserId { get; set; }

        public static Comment? Create(CommentBindingModel? model)
        {
            if (model == null)
            {
                return null;
            }
            return new Comment()
            {
                Id = model.Id,
                Text = model.Text,
                BuildId = model.BuildId,
                UserId = model.UserId,
            };
        }
        public void Update(CommentBindingModel? model)
        {
            if (model == null)
            {
                return;
            }
            Text = model.Text;
        }
        public CommentViewModel GetViewModel => new()
        {
            Id = Id,
            Text = Text,
            BuildId = BuildId,
            BuildName = Build.BuildName,
            UserId = UserId,
        };
    }
}
