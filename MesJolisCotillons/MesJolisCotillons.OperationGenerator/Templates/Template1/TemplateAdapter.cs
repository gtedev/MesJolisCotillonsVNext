namespace MesJolisCotillons.Adapters.TemplateNamespace
{
    using MesJolisCotillons.Contracts.Requests.TemplateNamespace;
    using System.Threading.Tasks;

    public class TemplateOperationNameAdapter : AdapterBase<TemplateOperationNameRequest>, ITemplateOperationNameAdapter
    {
        public TemplateOperationNameAdapter()
        {
        }

        public override async Task InitAdapter(TemplateOperationNameRequest request)
        {
        }
    }
}
