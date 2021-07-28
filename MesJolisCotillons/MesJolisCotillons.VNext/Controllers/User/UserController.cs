namespace MesJolisCotillons.VNext.Controllers.User
{
    using System.Threading.Tasks;
    using MesJolisCotillons.Contracts.Requests.User.Create;
    using MesJolisCotillons.Contracts.Responses.User.Create;
    using MesJolisCotillons.Operations.User.Create;
    using MesJolisCotillons.Resources;
    using MesJolisCotillons.Resources.Services;
    using MesJolisCotillons.VNext.Controllers.Service;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/users")]
    public class UserController : ApiController
    {
        private readonly IResourceLocalizerService resourceService;

        public UserController(IControllerService controllerService, IResourceLocalizerService resourceService)
       : base(controllerService)
            => this.resourceService = resourceService;

        /// <summary>
        /// Create a user.
        /// </summary>
        /// <returns>A response that user has been created.</returns>
        [Produces("application/json")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateUserRequest request)
        {
            return await this.controllerService
                    .ExecuteOperationAsync<CreateUserRequest, CreateUserResponse, CreateUserOperation>(request);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public Task<IActionResult> TestCreate()
        {
            var request = new CreateUserRequest
            {
                Email = "supersangdokuu@gmail.com",
                FirstName = "Toto",
                LastName = "Toto",
                Password = "aaaaaa",
                ConfirmedPassword = "aaaaaa"
            };

            return this.controllerService
                .ExecuteOperationAsync<CreateUserRequest, CreateUserResponse, CreateUserOperation>(request);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult TestResource()
        {
            var result = this.resourceService.GetResourceValue("PleaseFillRequiredFieldsForm", ResourceName.Messages, "en-GB");

            return this.Json(new
            {
                result = result
            });
        }
    }
}