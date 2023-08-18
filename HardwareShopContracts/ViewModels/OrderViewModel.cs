using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;
using System.ComponentModel;

namespace HardwareShopContracts.ViewModels
{
    public class OrderViewModel : IOrderModel
    {
        [DisplayName("Номер")]
        public int Id { get; set; }
        public int GoodId { get; set; }
        public int UserId { get; set; }
        [DisplayName("Товар")]
        public string GoodName { get; set; } = string.Empty;
        [DisplayName("Количество")]
        public int Count { get; set; }
        [DisplayName("Сумма")]
        public double Sum { get; set; }
        [DisplayName("Статус")]
        public OrderStatus Status { get; set; }
        [DisplayName("Дата создания")]
        public DateTime DateCreate { get; set; }
        [DisplayName("Дата выполнения")]
        public DateTime? DateImplement { get; set; }
    }
}
