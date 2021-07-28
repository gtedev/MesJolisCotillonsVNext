namespace MesJolisCotillons.Core.Framework.Builders.Operation
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Core.Framework.Builders.Runner;

    public interface IOperationBuilderStep<TCommand, TResponse>
        where TResponse : IResponse
        where TCommand : ICommand
    {
        /// <summary>
        /// <para>Build the running operation function that glues all different builders together (Adapter, CommandBuilder,  ValidationBuilder...).</para>
        /// <para>The function is responsible to build the command, run the validation steps, run execution steps if validation has passed and return a response when the operation finishes.</para>
        /// </summary>
        /// <returns></returns>
        IRunnerOperationStep<TCommand, TResponse> Build();
    }
}
