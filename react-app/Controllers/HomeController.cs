using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;

namespace react_app.Controllers {

    public class HomeController : Controller {
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
    }
}
