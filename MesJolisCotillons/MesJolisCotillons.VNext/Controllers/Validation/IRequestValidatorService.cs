namespace MesJolisCotillons.VNext.Controllers.Validation
{
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;

    public interface IRequestValidatorService
    {
        bool Validate(IRequest request);

        UnsuccessResponseBase GetCustomInvalidResponse(IRequest request);
    }
}
