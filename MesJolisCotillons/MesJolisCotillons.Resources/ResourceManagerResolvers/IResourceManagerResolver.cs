using System.Resources;

namespace MesJolisCotillons.Resources.ResourceManagerResolvers
{
    public interface IResourceManagerResolver
    {
        ResourceName ResourceName { get; }

        ResourceManager GetResourceManager(string cultureCode);
    }
}
