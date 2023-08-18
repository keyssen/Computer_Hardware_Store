using HardwareShopDatabaseImplement.Models.Storekeeper;
using System.ComponentModel.DataAnnotations;

namespace HardwareShopDatabaseImplement.Models.ManyToMany
{
    public class GoodComponent
    {
        public int GoodId { get; set; }

        public int ComponentId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Component Component { get; set; } = new();

        public virtual Good Good { get; set; } = new();
    }
}
