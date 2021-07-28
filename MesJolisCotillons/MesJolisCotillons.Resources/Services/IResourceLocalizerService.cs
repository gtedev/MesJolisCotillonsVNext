namespace MesJolisCotillons.Resources.Services
{
    public interface IResourceLocalizerService
    {
        string GetResourceValue(string keyName, ResourceName resourceName, string cultureCountryCode = null);
    }
}
