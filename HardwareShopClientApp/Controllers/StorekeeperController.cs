using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDataModels.Enums;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace HardwareShopStorekeeperApp.Controllers
{
	public class StorekeeperController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public StorekeeperController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult CreateOrder()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Goods = APIClient.GetRequest<List<GoodViewModel>>($"api/good/getgoods?userId={APIClient.User.Id}");
            return View();
        }

        [HttpPost]
        public void CreateOrder(int good, int count, string sum)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (good <= 0)
            {
                throw new Exception("Некорректный идентификатор товара");
            }
            if (count <= 0)
            {
                throw new Exception("Количество должно быть больше 0");
            }
            if (Convert.ToDouble(sum.Replace('.', ',')) <= 0)
            {
                throw new Exception("Цена должна быть больше 0");
            }
            APIClient.PostRequest("api/order/createorder", new OrderBindingModel
            {
                UserId = APIClient.User.Id,
                GoodId = good,
                Count = count,
                Sum = Convert.ToDouble(sum.Replace('.', ','))
            });
            Response.Redirect("Orders");
        }

        [HttpPost]
        public void DeleteOrder(int Id)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (Id <= 0)
            {
                throw new Exception("Некорректный идентификатор");
            }
            APIClient.PostRequest("api/order/deleteorder", new OrderBindingModel
            {
                Id = Id
            });
            Response.Redirect("Orders");
        }

        [HttpPost]
        public void UpdateOrder(int id, int status)
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
            APIClient.PostRequest("api/order/updatedata", new OrderBindingModel
            {
                Id = id,
                Status = (OrderStatus)status
            });
            Response.Redirect("Orders");
        }

        [HttpPost]
        public double Calc(int count, int good)
        {
            var prod = APIClient.GetRequest<GoodViewModel>($"api/good/getgood?id={good}");
            double result = Math.Round(count * (prod?.Price ?? 1), 2);
            return result;
        }

        public IActionResult CreateGood()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Components = APIClient.GetRequest<List<ComponentViewModel>>($"api/component/getcomponents?userId={APIClient.User.Id}");
            return View();
        }
        
        [HttpPost]
        public void CreateGood([FromBody] GoodBindingModel goodModel)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(goodModel.GoodName))
            {
                throw new Exception("Название не должно быть пустым");
            }
            if (goodModel.Price <= 0)
            {
                throw new Exception("Цена должна быть больше 0");
            }
            goodModel.UserId = APIClient.User.Id;
            APIClient.PostRequest("api/good/creategood", goodModel);
        }

        public IActionResult UpdateGood(int goodid)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            ViewBag.Components = APIClient.GetRequest<List<ComponentViewModel>>($"api/component/getcomponents?userId={APIClient.User.Id}");
            return View(goodid);
        }

        [HttpPost]
        public void UpdateGood([FromBody] GoodBindingModel goodModel)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(goodModel.GoodName))
            {
                throw new Exception("Название не должно быть пустым");
            }
            if (goodModel.Price <= 0)
            {
                throw new Exception("Цена должна быть больше 0");
            }
            goodModel.UserId = APIClient.User.Id;
            APIClient.PostRequest("api/good/updatedata", goodModel);
        }

        [HttpGet]
        public GoodViewModel? GetGood(int Id)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (Id <= 0)
            {
                throw new Exception($"Идентификатор товара не может быть меньше или равен 0");
            }
            var result = APIClient.GetRequest<GoodViewModel>($"api/good/getgood?id={Id}");
            return result;
        }

        [HttpGet]
        public Tuple<GoodViewModel, List<Tuple<ComponentViewModel?, int>>>? GetGoodUpdate(int goodid)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            var result = APIClient.GetRequest<Tuple<GoodViewModel,
                List<Tuple<ComponentViewModel?, int>>>?>($"api/good/getgoodupdate?id={goodid}&userId={APIClient.User.Id}");
            return result;
        }

        [HttpPost]
        public void DeleteGood(int good)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (good <= 0)
            {
                throw new Exception($"Идентификатор товара не может быть меньше или равен 0");
            }
            APIClient.PostRequest("api/good/deletegood", new GoodBindingModel
            {
                Id = good
            });
        }

        public IActionResult LinkBuilds(int componentid)
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Builds = APIClient.GetRequest<List<BuildViewModel>>($"api/build/getbuilds");
            return View(componentid);
        }

        [HttpPost]
        public void LinkBuilds([FromBody] ComponentBindingModel componentModel)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            componentModel.UserId = APIClient.User.Id;
            APIClient.PostRequest($"api/component/updatedata", componentModel);
        }

        [HttpGet]
        public BuildViewModel? GetBuild(int buildId)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (buildId <= 0)
            {
                throw new Exception($"Идентификатор сборки не может быть меньше или равен 0");
            }
            var result = APIClient.GetRequest<BuildViewModel>($"api/build/getbuild?buildId={buildId}");
            return result;
        }

        [HttpGet]
        public List<Tuple<BuildViewModel, int>>? GetComponentBuilds(int componentid)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            var result = APIClient.GetRequest<List<Tuple<BuildViewModel, int>>?>($"api/component/getcomponentbuilds?id={componentid}");
            return result;
        }

        public IActionResult CreateComponent()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public void CreateComponent(string name, string cost)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception("Название не должно быть пустым");
            }
            if (string.IsNullOrEmpty(cost) || Convert.ToDouble(cost.Replace('.', ',')) <= 0)
            {
                throw new Exception("Цена должна быть больше 0");
            }
            APIClient.PostRequest("api/component/createcomponent", new ComponentBindingModel
            {
                UserId = APIClient.User.Id,
                ComponentName = name,
                Cost = Convert.ToDouble(cost.Replace('.', ','))
            });
            Response.Redirect("Components");
        }

        [HttpGet]
        public ComponentViewModel? GetComponent(int Id)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            var result = APIClient.GetRequest<ComponentViewModel?>($"api/component/getcomponent?id={Id}");
            if (result == null)
            {
                return default;
            }
            return result;
        }

        [HttpPost]
        public void UpdateComponent(string name, string cost, DateTime date, int component)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (component <= 0)
            {
                throw new Exception($"Идентификатор комплектующего не может быть меньше или равен 0");
            }
            if (string.IsNullOrEmpty(name))
            {
                throw new Exception($"Имя комплектующего не должно быть пустым");
            }
            if (Convert.ToDouble(cost.Replace('.', ',')) <= 0)
            {
                throw new Exception($"Цена комплектующего не может быть меньше или равна 0");
            }
            APIClient.PostRequest("api/component/updatecomponent", new ComponentBindingModel
            {
                Id = component,
                ComponentName = name,
                Cost = Convert.ToDouble(cost.Replace('.', ',')),
                UserId = APIClient.User.Id,
            });
            Response.Redirect("Components");
        }

        [HttpPost]
        public void DeleteComponent(int component)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (component <= 0)
            {
                throw new Exception($"Идентификатор комплектующего не может быть меньше или равен 0");
            }
            APIClient.PostRequest("api/component/deletecomponent", new ComponentBindingModel
            {
                Id = component
            });
        }

        public IActionResult MainStorekeeper()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        public IActionResult Components()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<ComponentViewModel>>($"api/component/getcomponents?userId={APIClient.User.Id}"));
        }

        public IActionResult Goods()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<GoodViewModel>>($"api/good/getgoods?userId={APIClient.User.Id}"));
        }

        public IActionResult Orders()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View(APIClient.GetRequest<List<OrderViewModel>>($"api/order/getorders?userId={APIClient.User.Id}"));
        }

        public IActionResult ListBuilds()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            ViewBag.Goods = APIClient.GetRequest<List<GoodViewModel>>($"api/good/getgoods?userId={APIClient.User.Id}");
            return View();
        }

        [HttpPost]
        public int[]? ListBuilds([FromBody] GoodBindingModel goodModel, [FromQuery] string format)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            if (string.IsNullOrEmpty(format))
            {
                throw new FormatException("Неправильный формат файла");
            }
            byte[]? file = APIClient.PostRequestWithResult<GoodBindingModel, byte[]>($"api/report/buildgoodreport?format={format}", goodModel);
            return file!.Select(b => (int)b).ToArray();
        }

        public IActionResult Report()
        {
            if (APIClient.User == null)
            {
                return Redirect("~/Home/Enter");
            }
            return View();
        }

        [HttpPost]
        public List<ReportComponentsViewModel> Report([FromBody] ReportBindingModel reportModel)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            reportModel.UserId = APIClient.User.Id;
            List<ReportComponentsViewModel>? list = APIClient.PostRequestWithResult
                <ReportBindingModel, List<ReportComponentsViewModel>>("api/report/componentsreport", reportModel);
            return list!;
        }

        [HttpPost]
        public void ReportSendOnMail([FromBody] ReportBindingModel reportModel)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            reportModel.UserId = APIClient.User.Id;
            reportModel.UserEmail = APIClient.User.Email;
            APIClient.PostRequest("api/report/componentsreportsendonmail", reportModel);
        }

        [HttpGet]
        public List<CommentViewModel> GetCommentsOnBuild(int buildId)
        {
            if (APIClient.User == null)
            {
                throw new Exception("Вы как сюда попали? Сюда вход только авторизованным");
            }
            return APIClient.GetRequest<List<CommentViewModel>>($"api/comment/GetCommentsOnBuild?buildId={buildId}")!;
        }
    }
}
