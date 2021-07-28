namespace MesJolisCotillons.Response.Builders.User.Create
{
    using MesJolisCotillons.Contracts.Requests.User.Create;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Resources;
    using MesJolisCotillons.Resources.Services;

    public class CreateUserResponseBuilderFormValidation : CustomInvalidResponseBuilderBase<CreateUserRequest>
    {
        private readonly IResourceLocalizerService resourceService;

        public CreateUserResponseBuilderFormValidation(IResourceLocalizerService resourceService)
        {
            this.resourceService = resourceService;
        }

        public override UnsuccessResponseBase GetCustomInvalidResponse(CreateUserRequest request)
        {
            var response = new UnsuccessResponseBase();
            var message = this.resourceService.GetResourceValue("PleaseFillRequiredFieldsForm", ResourceName.Messages);
            response.AddMessages(message);

            return response;
        }
    }
}
