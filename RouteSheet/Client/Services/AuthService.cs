using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using RouteSheet.Client.Providers;
using RouteSheet.Shared.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace RouteSheet.Client.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;

        public AuthService(HttpClient httpClient,
            AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }


        public async Task<LoginResultViewModel> Login(LoginViewModel loginModel)
        {
            var result = await _httpClient.PostAsJsonAsync("api/auth/login", loginModel);

            if (result.IsSuccessStatusCode)
            {
                var loginResult = await result.Content.ReadFromJsonAsync<LoginResultViewModel>();
                await _localStorage.SetItemAsync("authToken", loginResult.Token);
                ((AppAuthStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.Login);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", loginResult.Token);

                return new LoginResultViewModel { Successful = true };
            }

            return new LoginResultViewModel { Successful = false, Error = "Неверное имя пользователя или пароль!" };
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((AppAuthStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }
    }
}
