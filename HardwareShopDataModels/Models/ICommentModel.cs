namespace HardwareShopDataModels.Models
{
    public interface ICommentModel : IId
    {
        string Text { get; }

        int BuildId { get; }

        int UserId { get; }
    }
}
