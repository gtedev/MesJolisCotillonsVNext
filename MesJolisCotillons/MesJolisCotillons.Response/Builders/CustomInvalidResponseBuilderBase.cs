namespace MesJolisCotillons.Response.Builders
{
    using System;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;

    public abstract class CustomInvalidResponseBuilderBase<TRequest> : ICustomInvalidRequestResponseBuilder
        where TRequest : IRequest, new()
    {
        public Type RequestType => new TRequest().GetType();

        public UnsuccessResponseBase GetCustomInvalidResponse(IRequest request)
        {
            var castRequest = (TRequest)request;
            return this.GetCustomInvalidResponse(castRequest);
        }

        public abstract UnsuccessResponseBase GetCustomInvalidResponse(TRequest request);
    }
}
