using RouteSheet.Shared.ViewModels;

namespace RouteSheet.Client.Services
{
    public interface IAuthService
    {
        Task<LoginResultViewModel> Login(LoginViewModel loginModel);
        Task Logout();
    }
}
