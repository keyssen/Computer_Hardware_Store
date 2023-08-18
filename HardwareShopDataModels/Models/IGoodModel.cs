namespace HardwareShopDataModels.Models
{
    public interface IGoodModel : IId
    {
        string GoodName { get; }
        double Price { get; }
        int UserId { get; }
        Dictionary<int, (IComponentModel, int)> GoodComponents { get; }
    }
}
