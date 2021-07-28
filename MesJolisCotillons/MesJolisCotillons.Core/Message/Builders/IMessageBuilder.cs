namespace MesJolisCotillons.Core.Message.Builders
{
    using MesJolisCotillons.Commands;
    using MesJolisCotillons.Contracts;

    public interface IMessageBuilder
    {
        MessageCode MessageCode { get; }

        string GetMessageString(ICommand command);
    }
}
