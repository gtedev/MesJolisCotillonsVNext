using MesJolisCotillons.ViewModels;

namespace MesJolisCotillons.Service
{
    public interface IProductItemViewService
    {
        ProductItemViewModel GetProductViewResponse(int productId, string baseUrl, string absoluteUri);
    }
}
