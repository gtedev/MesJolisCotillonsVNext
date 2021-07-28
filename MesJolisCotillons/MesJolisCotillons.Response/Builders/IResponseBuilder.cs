namespace MesJolisCotillons.Response.Builders
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Validation.Validators;

    public interface IResponseBuilder<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : IResponse
    {
        TResponse Build(TResponse response, IValidationReport<TCommand> validationReport);
    }
}
