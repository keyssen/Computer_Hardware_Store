using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;

namespace HardwareShopContracts.BindingModels
{
    public class OrderBindingModel : IOrderModel
    {
        public int Id { get; set; }

        public int GoodId { get; set; }

        public int UserId { get; set; }

        public int Count { get; set; }

        public double Sum { get; set; }

        public OrderStatus Status { get; set; } = OrderStatus.Неизвестен;

        public DateTime DateCreate { get; set; } = DateTime.Now;

        public DateTime? DateImplement { get; set; }
    }
}
