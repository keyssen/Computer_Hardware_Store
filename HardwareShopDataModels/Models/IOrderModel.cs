using HardwareShopDataModels.Enums;

namespace HardwareShopDataModels.Models
{
    public interface IOrderModel : IId
    {
        int GoodId { get; }
        int UserId { get; }
        int Count { get; }
        double Sum { get; }
        OrderStatus Status { get; }
        DateTime DateCreate { get; }
        DateTime? DateImplement { get; }
    }
}
