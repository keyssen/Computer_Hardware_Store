using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;

namespace HardwareShopContracts.BindingModels
{
    public class UserBindingModel : IUserModel
    {
        public int Id { get; set; }

        public string Login { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public UserRole Role { get; set; } = UserRole.Неизвестен;
    }
}