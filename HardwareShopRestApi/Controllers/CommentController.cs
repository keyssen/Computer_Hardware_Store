using HardwareShopContracts.BindingModels;
using HardwareShopContracts.BusinessLogicsContracts;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HardwareShopRestApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ILogger _logger;

        private readonly ICommentLogic _commentLogic;

        public CommentController(ICommentLogic commentLogic, ILogger<UserController> logger)
        {
            _logger = logger;
            _commentLogic = commentLogic;
        }

		[HttpGet]
		public List<CommentViewModel>? GetComments(int userId)
		{
			try
			{
				return _commentLogic.ReadList(new CommentSearchModel
				{
					UserId = userId
				});
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка получения списка комментариев пользоватля");
				throw;
			}
		}

        [HttpGet]
        public List<CommentViewModel>? GetCommentsOnBuild(int buildId)
        {
            try
            {
                return _commentLogic.ReadList(new CommentSearchModel
                {
                    BuildId = buildId
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка получения списка комментариев пользоватля");
                throw;
            }
        }

        [HttpGet]
		public CommentViewModel? GetComment(int commentId)
		{
			try
			{
				return _commentLogic.ReadElement(new() { Id = commentId });
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка чтения комментария");
				throw;
			}
		}

		[HttpPost]
		public void Create(CommentBindingModel model)
		{
			try
			{
				_commentLogic.Create(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка создания комментария");
				throw;
			}
		}

		[HttpPost]
		public void Update(CommentBindingModel model)
		{
			try
			{
				_commentLogic.Update(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка обновления комментария");
				throw;
			}
		}

		[HttpPost]
		public void Delete(CommentBindingModel model)
		{
			try
			{
				_commentLogic.Delete(model);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка удаления комментария");
				throw;
			}
		}
	}
}
