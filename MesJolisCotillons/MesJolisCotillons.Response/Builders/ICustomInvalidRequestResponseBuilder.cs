namespace MesJolisCotillons.Response.Builders
{
    using System;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;

    public interface ICustomInvalidRequestResponseBuilder
    {
        Type RequestType { get; }

        UnsuccessResponseBase GetCustomInvalidResponse(IRequest request);
    }
}
