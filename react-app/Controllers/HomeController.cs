using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using react_app.Models;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace react_app.Controllers {

    public class HomeController : ControllerBase {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginInfo loginInfo)
        {
            var user = await _userManager.FindByNameAsync(loginInfo.Username);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, loginInfo.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return Ok();
                }
            }

            return Unauthorized();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]LoginInfo loginInfo)
        {
            var user = new IdentityUser
            {
                UserName = loginInfo.Username
            };

            var result = await _userManager.CreateAsync(user, loginInfo.Password);

            if (result.Succeeded)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, loginInfo.Password, false, false);

                if (signInResult.Succeeded)
                {
                    return Ok();
                }
            }

            return Unauthorized();
        }

        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();

            return NoContent();
        }

        public IActionResult Authenticate() {
            var aClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Test"),
                new Claim(ClaimTypes.Email, "test@test.com"),
                new Claim("aClaim.Test", "Some test text.")
            };

            var bClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "Test Test"),
                new Claim("bClaim.Test", "Some test text.")
            };

            var aIdentity = new ClaimsIdentity(aClaims, "a Identity");
            var bIdentity = new ClaimsIdentity(bClaims, "b Identity");

            var userPrincipal = new ClaimsPrincipal(new[] { aIdentity, bIdentity });

            HttpContext.SignInAsync(userPrincipal);

            return new EmptyResult();
        }

        public async Task<IActionResult> DoSomething([FromServices] IAuthorizationService authorizationService)
        {
            var authResult = await authorizationService.AuthorizeAsync(User, "aPolicy");

            if (authResult.Succeeded){
                return Ok();
            }

            return Ok();
        }
    }
}
