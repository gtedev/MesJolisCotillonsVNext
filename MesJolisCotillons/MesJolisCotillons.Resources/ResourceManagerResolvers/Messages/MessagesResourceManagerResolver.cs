using System.Reflection;
using System.Resources;

namespace MesJolisCotillons.Resources.ResourceManagerResolvers
{
    public class MessagesResourceManagerResolver : ResourceManagerResolverBase
    {
        private readonly string baseName = "MesJolisCotillons.Resources.Messages.Messages-";
        private readonly string defaultCode = "fr-FR"; 

        public MessagesResourceManagerResolver() : base(ResourceName.Messages)
        {
        }

        public override ResourceManager GetResourceManager(string cultureCode)
        {
            var code = cultureCode ?? defaultCode;
            return new ResourceManager($"{this.baseName}{code}", Assembly.GetExecutingAssembly());
        }
    }
}
