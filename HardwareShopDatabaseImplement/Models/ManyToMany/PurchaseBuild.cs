using HardwareShopDatabaseImplement.Models.Worker;
using System.ComponentModel.DataAnnotations;

namespace HardwareShopDatabaseImplement.Models.ManyToMany
{
    public class PurchaseBuild
    {

        public int BuildId { get; set; }

        public int PurchaseId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Build Build { get; set; } = new();

        public virtual Purchase Purchase { get; set; } = new();
    }
}
