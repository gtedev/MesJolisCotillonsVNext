namespace MesJolisCotillons.VNext.Controllers
{
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class TestController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public TestController(IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
        }

        public string CheckHttpContext()
        {
            var yaya = this.httpContextAccessor.HttpContext;
            return "Ok";
        }

        [Authorize]
        public string Test()
        {
            return "je suis authentifie";
        }

        [Authorize]
        public string SignOut()
        {
            this.httpContextAccessor.HttpContext.SignOutAsync();
            return "je ne plus authentifie";
        }
    }
}