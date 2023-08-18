using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.Worker;
using Microsoft.EntityFrameworkCore;

namespace HardwareShopDatabaseImplement.Implements.Worker
{
    public class CommentStorage : ICommentStorage
    {
        public List<CommentViewModel> GetFullList()
        {
            using var context = new HardwareShopDatabase();
            return context.Comments
                    .Include(x => x.Build)
                    .Select(x => x.GetViewModel)
                    .ToList();
        }

        public List<CommentViewModel> GetFilteredList(CommentSearchModel model)
        {
            if (!model.BuildId.HasValue && !model.UserId.HasValue)
            {
                return new();
            }
            using var context = new HardwareShopDatabase();
            if (model.UserId.HasValue)
            {
                return context.Comments
                    .Include(x => x.Build)
                    .Where(x => x.UserId == model.UserId)
                    .Select(x => x.GetViewModel)
                    .ToList();
            }
            return context.Comments
                .Include(x => x.Build)
                .Where(x => x.BuildId == model.BuildId)
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public CommentViewModel? GetElement(CommentSearchModel model)
        {
            if (!model.Id.HasValue)
            {
                return null;
            }
            using var context = new HardwareShopDatabase();
            return context.Comments
                .Include(x => x.Build)
                .FirstOrDefault(x => model.Id.HasValue && x.Id == model.Id)
                ?.GetViewModel;
        }

        public CommentViewModel? Insert(CommentBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var newComment = Comment.Create(model);
            if (newComment == null)
            {
                return null;
            }
            context.Comments.Add(newComment);
            context.SaveChanges();
            return context.Comments
                .Include(x => x.Build)
                .FirstOrDefault(x => x.Id == newComment.Id)
                ?.GetViewModel;
        }

        public CommentViewModel? Update(CommentBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var comment = context.Comments
                .Include(x => x.Build)
                .FirstOrDefault(x => x.Id == model.Id);
            if (comment == null)
            {
                return null;
            }
            comment.Update(model);
            context.SaveChanges();
            return comment.GetViewModel;
        }

        public CommentViewModel? Delete(CommentBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var element = context.Comments
                .Include(x => x.Build)
                .FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Comments.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }
    }
}