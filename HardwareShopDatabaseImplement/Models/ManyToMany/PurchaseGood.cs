using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDatabaseImplement.Models.Worker;
using System.ComponentModel.DataAnnotations;

namespace HardwareShopDatabaseImplement.Models.ManyToMany
{
    public class PurchaseGood
    {

        public int PurchaseId { get; set; }

        public int GoodId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Purchase Purchase { get; set; } = new();

        public virtual Good Good { get; set; } = new();
    }
}
