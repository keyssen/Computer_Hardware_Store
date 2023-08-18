namespace HardwareShopDataModels.Models
{
    public interface IBuildModel : IId
    {
        double Price { get; }

        string BuildName { get; }

        int UserId { get; }

        Dictionary<int, (IPurchaseModel, int)> BuildPurchases { get; }

        Dictionary<int, (IComponentModel, int)> BuildComponents { get; }

        Dictionary<int, ICommentModel> BuildComments { get; }
    }
}
