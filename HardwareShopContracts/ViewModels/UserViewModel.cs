using HardwareShopDataModels.Enums;
using HardwareShopDataModels.Models;
using System.ComponentModel;

namespace HardwareShopContracts.ViewModels
{
    public class UserViewModel : IUserModel
    {
        public int Id { get; set; }

        [DisplayName("Логин клиента")]
        public string Login { get; set; } = string.Empty;

        [DisplayName("Email клиента")]
        public string Email { get; set; } = string.Empty;

        [DisplayName("Пароль")]
        public string Password { get; set; } = string.Empty;

        [DisplayName("Роль")]
        public UserRole Role { get; set; } = UserRole.Неизвестен;
    }
}