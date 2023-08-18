using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models;

namespace HardwareShopDatabaseImplement.Implements
{
    public class UserStorage : IUserStorage
    {
        public UserViewModel? Delete(UserBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var element = context.Users
                .FirstOrDefault(rec => rec.Id == model.Id);
            if (element != null)
            {
                context.Users.Remove(element);
                context.SaveChanges();
                return element.GetViewModel;
            }
            return null;
        }

        public UserViewModel? GetElement(UserSearchModel model)
        {
            using var context = new HardwareShopDatabase();
            if (model.Id.HasValue)
                return context.Users
                    .FirstOrDefault(x => x.Id == model.Id)?
                    .GetViewModel;
            if (!string.IsNullOrEmpty(model.Email) && !string.IsNullOrEmpty(model.Password))
                return context.Users
                    .FirstOrDefault(x => x.Email
                    .Equals(model.Email) && x.Password
                    .Equals(model.Password))?
                    .GetViewModel;
            if (!string.IsNullOrEmpty(model.Email))
                return context.Users
                    .FirstOrDefault(x => x.Email.Equals(model.Email))?.GetViewModel;
            if (!string.IsNullOrEmpty(model.Login))
                return context.Users
                    .FirstOrDefault(x => x.Login.Equals(model.Login))?.GetViewModel;
            return null;
        }

        public List<UserViewModel> GetFilteredList(UserSearchModel model)
        {
            if (string.IsNullOrEmpty(model.Login))
            {
                return new();
            }
            using var context = new HardwareShopDatabase();
            return context.Users
                .Where(x => x.Login.Contains(model.Login))
                .Select(x => x.GetViewModel)
                .ToList();
        }

        public List<UserViewModel> GetFullList()
        {
            using var context = new HardwareShopDatabase();
            return context.Users
                    .Select(x => x.GetViewModel)
                    .ToList();
        }

        public UserViewModel? Insert(UserBindingModel model)
        {
            var newUser = User.Create(model);
            if (newUser == null)
            {
                return null;
            }
            using var context = new HardwareShopDatabase();
            context.Users.Add(newUser);
            context.SaveChanges();
            return newUser.GetViewModel;
        }

        public UserViewModel? Update(UserBindingModel model)
        {
            using var context = new HardwareShopDatabase();
            var user = context.Users
                .FirstOrDefault(x => x.Id == model.Id);
            if (user == null)
            {
                return null;
            }
            user.Update(model);
            context.SaveChanges();
            return user.GetViewModel;
        }
    }
}