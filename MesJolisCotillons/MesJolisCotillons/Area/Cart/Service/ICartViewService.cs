using MesJolisCotillons.ViewModels;

namespace MesJolisCotillons.Area.Cart.Service
{
    public interface ICartViewService
    {
        CartViewModel GetCartViewModel(MySessionCartModel cart);
    }
}
