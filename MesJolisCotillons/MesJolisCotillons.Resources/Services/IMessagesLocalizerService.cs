namespace MesJolisCotillons.Resources.Services
{
    using MesJolisCotillons.Contracts;

    public interface IMessagesLocalizerService
    {
        string GetMessage(MessageCode messageCode, params string[] arguments);
    }
}
