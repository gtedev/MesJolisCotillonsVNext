using System.Resources;

namespace MesJolisCotillons.Resources.ResourceManagerResolvers
{
    public abstract class ResourceManagerResolverBase : IResourceManagerResolver
    {
        public ResourceManagerResolverBase(ResourceName resourceName)
        {
            this.ResourceName = resourceName;
        }

        public ResourceName ResourceName { get; }

        public abstract ResourceManager GetResourceManager(string cultureCode);
    }
}
