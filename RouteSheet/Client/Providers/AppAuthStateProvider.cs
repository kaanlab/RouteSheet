using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace RouteSheet.Client.Providers
{
    public class AppAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationState _anonymous;

        public AppAuthStateProvider(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _anonymous = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var savedToken = await _localStorage.GetItemAsync<string>("authToken");

            if (string.IsNullOrWhiteSpace(savedToken))
            {
                return _anonymous;
            }

            var claims = ParseClaimsFromJwt(savedToken);

            if (claims.Count() <= 0)
            {
                await _localStorage.RemoveItemAsync("authToken");
                MarkUserAsLoggedOut();
                _httpClient.DefaultRequestHeaders.Authorization = null;
                return _anonymous;
            }

            if (TokenIsExpired(claims.First(c => c.Type == ClaimTypes.Expired)))
            {
                await _localStorage.RemoveItemAsync("authToken");
                MarkUserAsLoggedOut();
                _httpClient.DefaultRequestHeaders.Authorization = null;
                return _anonymous;
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", savedToken);

            return new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt")));
        }

        public void MarkUserAsAuthenticated(string loginName)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, loginName) }, "jwt"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void MarkUserAsLoggedOut()
        {
            var authState = Task.FromResult(_anonymous);
            NotifyAuthenticationStateChanged(authState);
        }

        private IEnumerable<Claim> ParseClaimsFromJwt(string jwtToken)
        {
            var token = new JwtSecurityToken(jwtEncodedString: jwtToken);

            var id = token.Claims.FirstOrDefault(c => c.Type.Equals("nameid"))?.Value;
            if (!string.IsNullOrEmpty(id))
                yield return new Claim(ClaimTypes.NameIdentifier, id);

            var name = token.Claims.FirstOrDefault(c => c.Type.Equals("name"))?.Value;
            if (!string.IsNullOrEmpty(name))
                yield return new Claim(ClaimTypes.Name, name);

            var givenName = token.Claims.FirstOrDefault(c => c.Type.Equals("given_name"))?.Value;
            if (!string.IsNullOrEmpty(givenName))
                yield return new Claim(ClaimTypes.GivenName, givenName);

            var role = token.Claims.FirstOrDefault(c => c.Type.Equals("role"))?.Value;
            if (!string.IsNullOrEmpty(role))
                yield return new Claim(ClaimTypes.Role, role);

            var exp = token.Claims.FirstOrDefault(c => c.Type.Equals("exp"))?.Value;
            if (!string.IsNullOrEmpty(exp))
                yield return new Claim(ClaimTypes.Expired, exp);
        }

        private bool TokenIsExpired(Claim exp)
        {
            if (exp is not null)
            {
                var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp.Value));
                var timeUTC = DateTime.UtcNow;
                var diff = expTime - timeUTC;
                if (diff.TotalMinutes <= 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
