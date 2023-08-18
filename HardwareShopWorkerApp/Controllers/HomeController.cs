using DocumentFormat.OpenXml.Spreadsheet;
using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models;
using HardwareShopDatabaseImplement.Models.ManyToMany;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDatabaseImplement.Models.Worker;
using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;
using HardwareShopWorkerApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO.Pipelines;
using static System.Net.Mime.MediaTypeNames;

namespace HardwareShopWorkerApp.Controllers
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
                Role = UserRole.Работник,
            });
            Response.Redirect("Enter");
            return;
        }

        public IActionResult Index()
        {
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			}
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
        public void Privacy(string login, string email, string password)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как суда попали? Суда вход только авторизованным");
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
            Response.Redirect("MainWorker");
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
        public void Enter(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                throw new Exception("Введите почту и пароль");
            }
            APIClient.User = APIClient.GetRequest<UserViewModel>($"api/user/login?email={email}&password={password}");
            if (APIClient.User == null || APIClient.User.Role != UserRole.Работник)
            {
                throw new Exception("Неверные почта и/или пароль");
            }
            Response.Redirect("MainWorker");
        }

        [HttpPost]
		public void CreateBuild(string name)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как суда попали? Суда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception($"Имя магазина не должно быть пустым");
            }
            APIClient.PostRequest("api/build/create", new BuildBindingModel
            {
				BuildName = name,
				UserId = APIClient.User.Id
			});
            Response.Redirect("Builds");
        }

		[HttpGet]
		public BuildViewModel GetBuild(int buildId)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (buildId <= 0)
			{
				throw new Exception($"Идентификатор сборки не может быть ниже или равен 0");
			}
            var result = APIClient.GetRequest<BuildViewModel>($"api/build/getBuild?buildId={buildId}");
			if (result == null)
			{
				return null;
			}
			return result;
		}

		[HttpPost]
		public void UpdateBuild(string name, int buildId)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (string.IsNullOrEmpty(name))
			{
				throw new Exception($"Имя сборки не должно быть пустым");
			}
			if (buildId <= 0)
			{
				throw new Exception($"Идентификатор сборки не может быть ниже или равен 0");
			}
			APIClient.PostRequest("api/build/update", new BuildBindingModel
			{
                Id = buildId,
				BuildName = name
			});
			Response.Redirect("Builds");
		}

		[HttpGet]
		public void LinkBuildPurchase(int buildId, int purchaseId, int count)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (buildId <= 0)
			{
				throw new Exception($"Идентификтаор сборки не может быть ниже или равен 0");
			}
			if (purchaseId <= 0)
			{
				throw new Exception($"Идентификтаор покупки не может быть ниже или равен 0");
			}
			if (count <= 0)
			{
				throw new Exception($"Количество сборок в покупке не может быть ниже или равен 0");
			}
			APIClient.GetRequest<bool>($"api/build/UpdateLinkPurchase?buildId={buildId}&purchaseId={purchaseId}&count={count}&userId={APIClient.User.Id}");
			Response.Redirect($"LinkPurchase?buildId={buildId}");
		}

		[HttpPost]
		public void DeleteBuild(int deleteBuildId)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (deleteBuildId <= 0)
			{
				throw new Exception($"Идентификатор сборки не может быть ниже или равен 0");
			}
			APIClient.PostRequest("api/build/DeleteBuild", new BuildBindingModel
			{
				Id = deleteBuildId
			});
			Response.Redirect("Builds");
		}

		public IActionResult MainWorker()
        {
            return View();
        }

        public IActionResult Comments()
        {			
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			}
			ViewBag.Builds = APIClient.GetRequest<List<BuildViewModel>>($"api/build/getbuilds?userId={APIClient.User.Id}");
			return View(APIClient.GetRequest<List<CommentViewModel>>($"api/comment/getcomments?userId={APIClient.User.Id}"));
        }

		[HttpPost]
		public void CreateComment(int buildId, string text)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new Exception($"Текст не должен быть пустым");
			}
			if (buildId <= 0)
			{
				throw new Exception($"Идентификатор сборки должен быть больше 0");
			}
			APIClient.PostRequest("api/comment/create", new CommentBindingModel
			{
				BuildId = buildId,
				UserId = APIClient.User.Id,
				Text = text
			});
			Response.Redirect("Comments");
		}

		[HttpGet]
		public CommentViewModel GetComment(int commentId)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (commentId <= 0)
			{
				throw new Exception($"Идентификатор комментария не может быть ниже или равен 0");
			}
			var result = APIClient.GetRequest<CommentViewModel>($"api/comment/getcomment?commentId={commentId}");
			if (result == null)
			{
				return null;
			}
			return result;
		}

		[HttpPost]
		public void UpdateComment(string text, int commentId)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (string.IsNullOrEmpty(text))
			{
				throw new Exception($"Текст комментария не должно быть пустым");
			}
			if (commentId <= 0)
			{
				throw new Exception($"Идентификатор комментария не может быть ниже или равен 0");
			}
			APIClient.PostRequest("api/comment/update", new CommentBindingModel
			{
				Id = commentId,
				Text = text
			});
			Response.Redirect("Comments");
		}


		[HttpPost]
		public void DeleteComment(int deleteCommentId)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (deleteCommentId <= 0)
			{
				throw new Exception($"Идентификатор комментария не может быть ниже или равен 0");
			}
			APIClient.PostRequest("api/comment/delete", new CommentBindingModel
			{
				Id = deleteCommentId,
			});
			Response.Redirect("Comments");
		}

        [HttpGet]
        public IActionResult Purchases()
        {
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			}
			return View(APIClient.PostRequestWithResult<PurchaseSearchModel, List<PurchaseViewModel>>($"api/purchase/getpurchases", new() { UserId = APIClient.User.Id }));
		}

        [HttpPost]
        public void Purchases(int id)
        {
            Response.Redirect("CreatePurchase");
        }

		[HttpGet]
		public GoodViewModel? GetGood(int Id)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
			}
			var result = APIClient.GetRequest<GoodViewModel?>($"api/good/getgood?id={Id}");
			if (result == null)
			{
				return default;
			}
			return result;
		}

		[HttpGet]
		public IActionResult ListComponents()
		{
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			}
			
			ViewBag.Purchases = APIClient.PostRequestWithResult<PurchaseSearchModel, List<PurchaseViewModel>> ($"api/purchase/getpurchases", new() {UserId = APIClient.User.Id });
			return View();
		}

		[HttpPost]
		public int[]? ListComponents([FromBody] PurchaseBindingModel purchaseModel, [FromQuery] string format)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
			}
			if (string.IsNullOrEmpty(format))
			{
				throw new FormatException($"Неправильный формат файла: {format}");
			}
			byte[]? file = APIClient.PostRequestWithResult<PurchaseBindingModel, byte[]>($"api/report/buildpurchasereport?format={format}", purchaseModel);
			var array = file!.Select(b => (int)b).ToArray();
			return array;
		}


		[HttpGet]
		public GoodViewModel? GetPurchase(int Id)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
			}
			var result = APIClient.GetRequest<GoodViewModel?>($"api/purchase/getpurchase?purchaseId={Id}");
			if (result == null)
			{
				return default;
			}
			return result;
		}

		[HttpGet]
        public IActionResult CreatePurchase()
        {
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			}
			ViewBag.Goods = APIClient.GetRequest<List<GoodViewModel>>($"api/good/GetAllGoods");
			return View();
		}

		[HttpPost]
		public void CreatePurchase([FromBody] PurchaseBindingModel purchaseModel)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
			}
			if (purchaseModel.Sum <= 0)
			{
				throw new Exception("Цена должна быть больше 0");
			}
			purchaseModel.UserId = APIClient.User.Id;
			APIClient.PostRequest("api/purchase/createpurchase", purchaseModel);
		}

		[HttpPost]
		public void UpdateStatusPurchase(int id, int status)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
			}
			if (id <= 0)
			{
				throw new Exception("Некорректный идентификатор");
			}
			if (status <= 0)
			{
				throw new Exception("Некорректный статус");
			}
			APIClient.PostRequest("api/purchase/UpdateStatusPurchase", new PurchaseBindingModel
			{
				Id = id,
				PurchaseStatus = (PurchaseStatus)status,
				UserId = APIClient.User.Id
			});
		}

		[HttpPost]
		public void DeletePurchase(int purchaseId)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
			}
			if (purchaseId <= 0)
			{
				throw new Exception("Некорректный идентификатор");
			}
			var purchase = APIClient.GetRequest<PurchaseViewModel>($"api/purchase/getpurchase?purchaseId={purchaseId}");
			APIClient.PostRequest("api/purchase/DeletePurchase", new PurchaseBindingModel
			{
				Id = purchaseId,
				PurchaseStatus = purchase.PurchaseStatus
			});
		}

		[HttpGet]
		public IActionResult UpdatePurchase(int purchaseId)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
			}
			ViewBag.Goods = APIClient.GetRequest<List<GoodViewModel>>($"api/good/GetAllGoods");
			return View(purchaseId);
		}

		[HttpPost]
		public void UpdatePurchase([FromBody] PurchaseBindingModel purchaseModel)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
			}
			purchaseModel.UserId = APIClient.User.Id;
			APIClient.PostRequest("api/purchase/update", purchaseModel);
		}

		[HttpGet]
		public Tuple<PurchaseViewModel, List<Tuple<GoodViewModel?, int>>>? GetPurchaseUpdate(int purchaseId)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
			}
			var result = APIClient.GetRequest<Tuple<PurchaseViewModel, List<Tuple<GoodViewModel?, int>>>?>($"api/purchase/getpurchaseupdate?purchaseId={purchaseId}&userId={APIClient.User.Id}");
			return result;
		}

        [HttpGet]
        public IActionResult Builds()
        {
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			} 
            return View(APIClient.GetRequest<List<BuildViewModel>>($"api/build/getbuilds?userId={APIClient.User.Id}"));
		}

        [HttpPost]
        public void Builds(int id)
        {
            Response.Redirect("LinkPurchase");
        }

        [HttpGet]
        public IActionResult LinkPurchase(int buildId)
        {
			if (APIClient.User == null)
			{
				return Redirect("~/Home/Enter");
			}
			if (buildId <= 0)
			{
				throw new Exception($"Идентификтаор сборки не может быть ниже или равен 0");
			}		
			ViewBag.Purchases = APIClient.PostRequestWithResult<PurchaseSearchModel, List<PurchaseViewModel>>($"api/purchase/getpurchases", new() { UserId = APIClient.User.Id, PurchaseStatus = PurchaseStatus.Выполняется });
			return View(APIClient.GetRequest<List<Tuple<PurchaseViewModel, int>>>($"api/build/GetBuildPurchase?buildId={buildId}"));
		}

		
		[HttpGet]
		public void DeleteLinkPurchase(int deleteBuildId, int deletePurchaseId)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (deleteBuildId <= 0)
			{
				throw new Exception($"Идентификтаор сборки не может быть ниже или равен 0");
			}
			if (deletePurchaseId <= 0)
			{
				throw new Exception($"Идентификтаор покупки не может быть ниже или равен 0");
			}
			APIClient.GetRequest<bool>($"api/build/deleteLinkPurchase?deleteBuildId={deleteBuildId}&deletePurchaseId={deletePurchaseId}&userId={APIClient.User.Id}");
			Response.Redirect($"LinkPurchase?buildId={deleteBuildId}");
		}

		[HttpGet]
		public void UpdateLinkPurchase(int updateBuildId, int updatePurchaseId, int count)
		{
			if (APIClient.User == null)
			{
				throw new Exception("Вы как суда попали? Суда вход только авторизованным");
			}
			if (updateBuildId <= 0)
			{
				throw new Exception($"Идентификтаор сборки не может быть ниже или равен 0");
			}
			if (updatePurchaseId <= 0)
			{
				throw new Exception($"Идентификтаор покупки не может быть ниже или равен 0");
			}
			APIClient.GetRequest<bool>($"api/build/UpdateLinkPurchase?updateBuildId={updateBuildId}&updatePurchaseId={updatePurchaseId}&count={count}&userId={APIClient.User.Id}");
			Response.Redirect($"LinkPurchase?buildId={updateBuildId}");
		}

        public IActionResult WorkerReport()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public List<ReportPurchaseViewModel>? WorkerReport([FromBody] ReportBindingModel reportModel)
        {
            if (APIClient.User == null)
            {
                return new();
            }
            reportModel.UserId = APIClient.User.Id;
            List<ReportPurchaseViewModel>? list = APIClient.PostRequestWithResult<ReportBindingModel, List<ReportPurchaseViewModel>>("api/report/getpurchasereportdata", reportModel);
            return list;
        }

        [HttpPost]
        public void SendByMailPurchaseReport([FromBody] ReportBindingModel reportModel)
        {
            if (APIClient.User == null)
            {
                return;
            }
            reportModel.UserId = APIClient.User.Id;
            reportModel.UserEmail = APIClient.User.Email;
            APIClient.PostRequest("api/report/SendByMailPurchaseReport", reportModel);
        }
    }
}