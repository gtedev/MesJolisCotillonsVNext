namespace MesJolisCotillons.Resources.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using MesJolisCotillons.Resources.ResourceManagerResolvers;

    public class ResourceLocalizerService : IResourceLocalizerService
    {
        private readonly IReadOnlyDictionary<ResourceName, IResourceManagerResolver> resourceManagerResolvers;

        public ResourceLocalizerService(IEnumerable<IResourceManagerResolver> resourceManagerResolvers)
        {
            this.resourceManagerResolvers = resourceManagerResolvers.ToDictionary(r => r.ResourceName, r => r);
        }

        public string GetResourceValue(string keyName, ResourceName resourceName, string cultureCountryCode = null)
        {
            var resourceManager = this.resourceManagerResolvers[resourceName].GetResourceManager(cultureCountryCode);
            return resourceManager.GetString(keyName);
        }
    }
}
