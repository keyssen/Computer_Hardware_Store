using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace HardwareShopBusinessLogic.BusinessLogics
{
    public class UserLogic : IUserLogic
    {
        private readonly ILogger _logger;
        private readonly IUserStorage _userStorage;
        public UserLogic(ILogger<UserLogic> logger, IUserStorage userStorage)
        {
            _logger = logger;
            _userStorage = userStorage;
        }
        public bool Create(UserBindingModel model)
        {
            CheckModel(model);
            if (_userStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Delete(UserBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id: {Id}", model.Id);
            if (_userStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }

        public UserViewModel? ReadElement(UserSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. Login: {Login}. Email: {Email}. Id: {Id}.",
                model.Login, model.Email, model.Id);
            var element = _userStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id: {Id}", element.Id);
            return element;
        }

        public List<UserViewModel>? ReadList(UserSearchModel? model)
        {
            _logger.LogInformation("ReadList. Login: {Login}. Email: {Email}. Id: {Id}.",
                 model?.Login, model?.Email, model?.Id);
            var list = model == null ? _userStorage.GetFullList() :
                _userStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count: {Count}", list.Count);
            return list;
        }

        public bool Update(UserBindingModel model)
        {
            CheckModel(model);
            if (_userStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }

        private void CheckModel(UserBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.Login) || model.Login.Length > 40)
            {
                throw new ArgumentNullException("Нет логина пользователя или длина превышает 40 символов", nameof(model.Login));
            }
            if (string.IsNullOrEmpty(model.Email) || model.Email.Length > 40)
            {
                throw new ArgumentNullException("Нет почты пользователя или длина превышает 40 символов", nameof(model.Email));
            }
            if (!Regex.IsMatch(model.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            {
                throw new ArgumentException("Неправильно введенная почта", nameof(model.Email));
            }
            if (string.IsNullOrEmpty(model.Password) || model.Password.Length > 40 || model.Password.Contains(' '))
            {
                throw new ArgumentNullException("Нет пароля пользователя или пароль содержит пробелы", nameof(model.Password));
            }
            _logger.LogInformation("User. Login: {Login}. Email: {Email}. Id: {Id}",
                model.Login, model.Email, model.Id);
            var elementLogin = _userStorage.GetElement(new UserSearchModel
            {
                Login = model.Login
            });
            var elementEmail = _userStorage.GetElement(new UserSearchModel
            {
                Email = model.Email
            });
            if (elementEmail != null && elementEmail.Id != model.Id)
            {
                throw new InvalidOperationException("Клиент с такой почтой уже есть");
            }
            if (elementLogin != null && elementLogin.Id != model.Id)
            {
                throw new InvalidOperationException("Клиент с таким логином уже есть");
            }
        }
    }
}
