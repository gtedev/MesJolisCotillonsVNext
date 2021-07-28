namespace MesJolisCotillons.VNext.Controllers
{
    using MesJolisCotillons.VNext.Controllers.Service;
    using Microsoft.AspNetCore.Mvc;

    public abstract class ApiController : Controller
    {
        protected readonly IControllerService controllerService;

        public ApiController(IControllerService controllerService)
            => this.controllerService = controllerService;
    }
}