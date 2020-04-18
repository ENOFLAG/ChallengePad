using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ChallengePad.Controllers
{
    [Route("api/[controller]")]
    public class OAuth2Controller : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = "/" }, new string[] { "OAuth"});
        }
    }
}
