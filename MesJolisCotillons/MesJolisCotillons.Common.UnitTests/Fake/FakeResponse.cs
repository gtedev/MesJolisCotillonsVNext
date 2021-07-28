using MesJolisCotillons.Contracts.Responses;

namespace MesJolisCotillons.Common.UnitTests.Fake
{
    public class FakeResponse : ResponseBase
    {
        public FakeResponse(bool success)
            : base(success)
        {
        }
    }
}
