using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace ChallengePad
{
    public class LoginPageModel : PageModel
    {
        private readonly IOptions<ChallengePadSettings> Settings;

        public LoginPageModel(IOptions<ChallengePadSettings> settings)
        {
            Settings = settings;
        }

        public async Task OnGet(string provider, string redirectUri, string guestPSK)
        {
            if (!Url.IsLocalUrl(redirectUri))
            {
                throw new Exception("Non-local redirects are forbidden");
            }
            if (provider == "OAuth")
            {
                await HttpContext.ChallengeAsync("OAuth", new AuthenticationProperties
                {
                    RedirectUri = redirectUri
                });
            }
            else if (provider == "GuestPSK")
            {
                if (guestPSK == Settings.Value.GuestPSK)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, "Guest"),
                        new Claim(ClaimTypes.Name, "Guest"),
                        new Claim(ClaimTypes.Role, "Guest")
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity));
                }
                Response.Redirect(redirectUri);
            }
        }
    }
}