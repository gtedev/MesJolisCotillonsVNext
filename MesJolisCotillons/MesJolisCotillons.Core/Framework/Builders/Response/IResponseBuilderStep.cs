namespace MesJolisCotillons.Core.Framework.Builders.Response
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Operation;
    using MesJolisCotillons.Response.Builders;

    public interface IResponseBuilderStep<TCommand, TResponse>
        where TCommand : ICommand
        where TResponse : ResponseBase
    {
        /// <summary>
        /// Add a ResponseBuilder to decide how to build the response of the Operation.
        /// </summary>
        /// <param name="responseBuilder"></param>
        /// <returns></returns>
        IOperationBuilderStep<TCommand, TResponse> AddResponseBuilder(IResponseBuilder<TCommand, TResponse> responseBuilder);

        /// <summary>
        /// Add a Default response builder to get a default response like "{OperationName}" has been executed with success.
        /// </summary>
        /// <returns></returns>
        IOperationBuilderStep<TCommand, TResponse> AddDefaultResponseBuilder();
    }
}
