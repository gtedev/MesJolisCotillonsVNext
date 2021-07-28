namespace MesJolisCotillons.Response.Builders.Default
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;

    public interface IDefaultResponseBuilder<TCommand, TResponse> : IResponseBuilder<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
    }
}
