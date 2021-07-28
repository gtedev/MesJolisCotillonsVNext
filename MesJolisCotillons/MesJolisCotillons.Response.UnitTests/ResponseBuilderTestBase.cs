using MesJolisCotillons.Commands;
using MesJolisCotillons.Contracts.Responses;
using MesJolisCotillons.Resources.Services;
using MesJolisCotillons.Response.Builders;
using MesJolisCotillons.Validation.Validators;
using NSubstitute;
using System;

namespace MesJolisCotillons.Response.UnitTests
{
    public abstract class ResponseBuilderTestBase<TCommand, TResponse, TResponseBuilder>
        where TCommand : ICommand
        where TResponse : class, IResponse
        where TResponseBuilder : IResponseBuilder<TCommand, TResponse>
    {
        public ResponseBuilderTestBase()
        {
            // Create a response by default with Success =  true
            // Need to cast to `object`, otherwise passing isValid, a boolean value, calls the wrong overload of CreateInstance
            object isValidParam = true;
            this.Response = Activator.CreateInstance(typeof(TResponse), isValidParam) as TResponse;
            this.MessagesServiceMock = Substitute.For<IMessagesLocalizerService>();

            this.ValidationReportMock = Substitute.For<IValidationReport<TCommand>>();
        }


        protected IMessagesLocalizerService MessagesServiceMock { get; set; }

        protected IValidationReport<TCommand> ValidationReportMock { get; set; }

        protected TResponse Response { get; set; }

        protected TCommand Command { get; set; }

        protected TResponseBuilder ResponseBuilder { get; set; }
    }
}
