using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChallengePad
{
    public class LoginPageModel : PageModel
    {
        public async Task OnGet(string redirectUri)
        {
            if (!Url.IsLocalUrl(redirectUri))
            {
                throw new Exception("Non-local redirects are forbidden");
            }
            await HttpContext.ChallengeAsync("OAuth", new AuthenticationProperties
            {
                RedirectUri = redirectUri
            });
        }
    }
}