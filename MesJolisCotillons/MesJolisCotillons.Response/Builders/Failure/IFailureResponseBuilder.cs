namespace MesJolisCotillons.Response.Builders.Failure
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;

    public interface IFailureResponseBuilder<TCommand, TResponse> : IResponseBuilder<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
    }
}
