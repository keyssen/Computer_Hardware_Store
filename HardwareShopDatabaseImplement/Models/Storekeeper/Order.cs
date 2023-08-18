using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;
using System.ComponentModel.DataAnnotations;

namespace HardwareShopDatabaseImplement.Models.Storekeeper
{
    public class Order : IOrderModel
    {
        public int Id { get; set; }

        [Required]
        public int GoodId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Count { get; set; }

        [Required]
        public double Sum { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public DateTime DateCreate { get; set; }

        public DateTime? DateImplement { get; set; }

        public virtual Good Good { get; set; } = null!;

        public static Order? Create(OrderBindingModel? model)
        {
            if (model == null)
            {
                return null;
            }
            return new Order()
            {
                Id = model.Id,
                GoodId = model.GoodId,
                UserId = model.UserId,
                Count = model.Count,
                Sum = model.Sum,
                Status = model.Status,
                DateCreate = model.DateCreate,
                DateImplement = model.DateImplement,
            };
        }

        public void Update(OrderBindingModel? model)
        {
            if (model == null)
            {
                return;
            }
            Status = model.Status;
            DateImplement = model.DateImplement;
        }

        public OrderViewModel GetViewModel => new()
        {
            Id = Id,
            GoodId = GoodId,
            UserId = UserId,
            Count = Count,
            Sum = Sum,
            Status = Status,
            DateCreate = DateCreate,
            DateImplement = DateImplement,
            GoodName = Good.GoodName
        };
    }
}
