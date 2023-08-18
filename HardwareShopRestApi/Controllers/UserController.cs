using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HardwareShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly ILogger _logger;

        private readonly IUserLogic _logic;

        public UserController(IUserLogic logic, ILogger<UserController> logger)
        {
            _logger = logger;
            _logic = logic;
        }

        [HttpGet]
        public UserViewModel? Login(string email, string password)
        {
            try
            {
                return _logic.ReadElement(new UserSearchModel
                {
                    Email = email,
                    Password = password
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка входа в систему");
                throw;
            }
        }

        [HttpPost]
        public void Register(UserBindingModel model)
        {
            try
            {
                _logic.Create(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка регистрации");
                throw;
            }
        }

        [HttpPost]
        public void UpdateData(UserBindingModel model)
        {
            try
            {
                _logic.Update(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка обновления данных");
                throw;
            }
        }
    }
}
