namespace MesJolisCotillons.DataAccess.Entities.Enums
{
    public enum CustomerUserStatus : int
    {
        Disabled = -200,
        Creation = 0,
        Awaiting_MailConfirmation = 100,
        Active = 200,
        CommandWithoutSubscription = -180
    }
}
