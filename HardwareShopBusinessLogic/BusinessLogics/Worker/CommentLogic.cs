using HardwareShopContracts.BindingModels;
using HardwareShopContracts.SearchModels;
using HardwareShopContracts.StoragesContracts;
using HardwareShopContracts.ViewModels;
using Microsoft.Extensions.Logging;

namespace HardwareShopContracts.BusinessLogicsContracts
{
    public class CommentLogic : ICommentLogic
    {
        private readonly ILogger _logger;
        private readonly ICommentStorage _commentStorage;
        public CommentLogic(ILogger<CommentLogic> logger, ICommentStorage commentStorage)
        {
            _logger = logger;
            _commentStorage = commentStorage;
        }

        public List<CommentViewModel>? ReadList(CommentSearchModel? model)
        {
            _logger.LogInformation("ReadList. BuildId:{BuildId}. Id:{Id}", model?.BuildId, model?.Id);
            var list = model == null ? _commentStorage.GetFullList() : _commentStorage.GetFilteredList(model);
            if (list == null)
            {
                _logger.LogWarning("ReadList return null list");
                return null;
            }
            _logger.LogInformation("ReadList. Count:{Count}", list.Count);
            return list;
        }

        public CommentViewModel? ReadElement(CommentSearchModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            _logger.LogInformation("ReadElement. BuildId:{BuildId}. Id:{Id}", model.BuildId, model.Id);
            var element = _commentStorage.GetElement(model);
            if (element == null)
            {
                _logger.LogWarning("ReadElement element not found");
                return null;
            }
            _logger.LogInformation("ReadElement find. Id:{Id}", element.Id);
            return element;
        }

        public bool Create(CommentBindingModel model)
        {
            CheckModel(model);
            if (_commentStorage.Insert(model) == null)
            {
                _logger.LogWarning("Insert operation failed");
                return false;
            }
            return true;
        }

        public bool Update(CommentBindingModel model)
        {
            CheckModel(model);
            if (_commentStorage.Update(model) == null)
            {
                _logger.LogWarning("Update operation failed");
                return false;
            }
            return true;
        }
        public bool Delete(CommentBindingModel model)
        {
            CheckModel(model, false);
            _logger.LogInformation("Delete. Id:{Id}", model.Id);
            if (_commentStorage.Delete(model) == null)
            {
                _logger.LogWarning("Delete operation failed");
                return false;
            }
            return true;
        }
        private void CheckModel(CommentBindingModel model, bool withParams = true)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            if (!withParams)
            {
                return;
            }
            if (string.IsNullOrEmpty(model.Text))
            {
                throw new ArgumentNullException("Нет текста у комментария", nameof(model.Id));
            }
            if (model.BuildId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор у сборки", nameof(model.BuildId));
            }
            if (model.Id < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор у комментария", nameof(model.Id));
            }
            if (model.UserId < 0)
            {
                throw new ArgumentNullException("Некорректный идентификатор у клиента", nameof(model.UserId));
            }

            _logger.LogInformation("Comment. UserId:{UserId}. BuildId:{BuildId}. Id:{Id}", model.UserId, model.BuildId, model.Id);
        }
    }
}