namespace MesJolisCotillons.VNext.Controllers.Validation
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using MesJolisCotillons.Contracts.Requests;
    using MesJolisCotillons.Contracts.Responses;
    using MesJolisCotillons.Response.Builders;

    public class RequestValidatorService : IRequestValidatorService
    {
        private readonly IReadOnlyDictionary<Type, ICustomInvalidRequestResponseBuilder> requestResponseBuilders;

        public RequestValidatorService(IEnumerable<ICustomInvalidRequestResponseBuilder> requestResponseBuilders)
        {
            this.requestResponseBuilders = requestResponseBuilders.ToDictionary(item => item.RequestType, item => item);
        }

        public bool Validate(IRequest request)
        {
            var validationContext = new ValidationContext(request);
            ICollection<ValidationResult> validationResults = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(request, validationContext, validationResults);

            return isValid;
        }

        public UnsuccessResponseBase GetCustomInvalidResponse(IRequest request)
        {
            this.requestResponseBuilders.TryGetValue(request.GetType(), out var builder);
            if (builder == null)
            {
                return new UnsuccessResponseBase();
            }

            return builder.GetCustomInvalidResponse(request);
        }
    }
}
