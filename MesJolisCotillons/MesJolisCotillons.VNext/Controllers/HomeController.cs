namespace MesJolisCotillons.VNext.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using MesJolisCotillons.DataAccess.Repositories.Repositories;
    using MesJolisCotillons.VNext.Models;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class HomeController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IProductRepository productRepository;
        private readonly IHttpContextAccessor httpContextAccessor;

        public HomeController(IUserRepository userRepository, IProductRepository productRepository, IHttpContextAccessor httpContextAccessor)
        {
            this.userRepository = userRepository;
            this.productRepository = productRepository;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index()
        {
            ////var user = this.context.FindAllAdminUser();
            var user = await this.userRepository.FindUser(2);
            var product = await this.productRepository.FindAllProducts(includeFirstPicture: true);

            ////await this.userRepository.FindAllProducts();
            ////var list = new List<Claim>();
            ////list.Add(new Claim("CurrentUser.UserId", user.UserId.ToString()));
            ////list.Add(new Claim("CurrentUser.FirstName", user.FirstName));
            ////list.Add(new Claim("CurrentUser.LastName", user.LastName));
            ////list.Add(new Claim("CurrentUser.Email", user.Email));

            ////var identity = new ClaimsIdentity(list, CookieAuthenticationDefaults.AuthenticationScheme);
            ////var claimUser = new ClaimsPrincipal(identity);

            ////await this.httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimUser);

            return this.View();
        }

        public IActionResult About()
        {
            this.ViewData["Message"] = "Your application description page.";

            return this.View();
        }

        public IActionResult Contact()
        {
            this.ViewData["Message"] = "Your contact page.";

            return this.View();
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
