using HardwareShopContracts.BindingModels;
using HardwareShopContracts.ViewModels;
using HardwareShopDatabaseImplement.Models.Storekeeper;
using HardwareShopDatabaseImplement.Models.Worker;
using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HardwareShopDatabaseImplement.Models
{
    public class User : IUserModel
    {
        public int Id { get; set; }

        [Required]
        public string Login { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public UserRole Role { get; set; } = UserRole.Неизвестен;

        [ForeignKey("UserId")]
        public virtual List<Order> Orders { get; set; } = new();

        [ForeignKey("UserId")]
        public virtual List<Build> Builds { get; set; } = new();

        [ForeignKey("UserId")]
        public virtual List<Comment> Comments { get; set; } = new();

        [ForeignKey("UserId")]
        public virtual List<Purchase> Purchases { get; set; } = new();

        [ForeignKey("UserId")]
        public virtual List<Component> Components { get; set; } = new();

        [ForeignKey("UserId")]
        public virtual List<Good> Goods { get; set; } = new();


        public static User? Create(UserBindingModel? model)
        {
            if (model == null)
            {
                return null;
            }
            return new User()
            {
                Id = model.Id,
                Login = model.Login,
                Email = model.Email,
                Password = model.Password,
                Role = model.Role
            };
        }

        public void Update(UserBindingModel? model)
        {
            if (model == null)
            {
                return;
            }
            Login = model.Login;
            Password = model.Password;
            Email = model.Email;
        }

        public UserViewModel GetViewModel => new()
        {
            Id = Id,
            Login = Login,
            Email = Email,
            Password = Password,
            Role = Role,
        };
    }
}