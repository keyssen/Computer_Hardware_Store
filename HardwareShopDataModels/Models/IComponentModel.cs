namespace HardwareShopDataModels.Models
{
    public interface IComponentModel : IId
    {
        string ComponentName { get; }
        double Cost { get; }
        int UserId { get; }
        DateTime DateCreate { get; }
        Dictionary<int, (IBuildModel, int)> ComponentBuilds { get; }
    }
}
