using MesJolisCotillons.Contracts.Requests;
using NSubstitute;

namespace MesJolisCotillons.Adapters.UnitTests
{
    public class AdapterTestBase<TAdapter, TReq>
        where TReq : class, IRequest
    {
        public AdapterTestBase()
        {
            this.Request = Substitute.For<TReq>();
        }

        public TReq Request { get; set; }

        public TAdapter Adapter { get; set; }
    }
}
