using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDatabaseImplement.Models.Worker;
using System.ComponentModel.DataAnnotations;

namespace HardwareShopDatabaseImplement.Models.ManyToMany
{
    public class ComponentBuild
    {

        public int BuildId { get; set; }

        public int ComponentId { get; set; }

        [Required]
        public int Count { get; set; }

        public virtual Build Build { get; set; } = new();

        public virtual Component Component { get; set; } = new();

    }
}
