using HardwareShopDataModels.Enums;

namespace HardwareShopDataModels.Models
{
    public interface IUserModel : IId
    {
        string Login { get; }

        string Email { get; }

        string Password { get; }

        UserRole Role { get; }
    }
}