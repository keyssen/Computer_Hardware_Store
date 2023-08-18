using HardwareShopStorekeeperApp.Models;
using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDataModels.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HardwareShopStorekeeperApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public void Register(string login, string email, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Введите логин, email, пароль");
            }
            APIClient.PostRequest("api/user/register", new UserBindingModel
            {
                Login = login,
                Email = email,
                Password = password,
                Role = UserRole.Кладовщик
            });
            Response.Redirect("Enter");
            return;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.User);
        }

        [HttpPost]
        public IActionResult Privacy(string login, string email, string password)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Введите логин, пароль и почту");
            }
            APIClient.PostRequest("api/user/updatedata", new UserBindingModel
            {
                Id = APIClient.User.Id,
                Login = login,
                Email = email,
                Password = password
            });

            APIClient.User.Login = login;
            APIClient.User.Email = email;
            APIClient.User.Password = password;

            return RedirectToAction("MainStorekeeper", "Storekeeper");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Enter()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Enter(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Введите почту и пароль");
            }
            APIClient.User = APIClient.GetRequest<UserViewModel>($"api/user/login?email={email}&password={password}");
            if (APIClient.User == null || APIClient.User.Role != UserRole.Кладовщик)
            {
                throw new Exception("Неверные почта и/или пароль");
            }
            return RedirectToAction("MainStorekeeper", "Storekeeper");
        }
    }
}