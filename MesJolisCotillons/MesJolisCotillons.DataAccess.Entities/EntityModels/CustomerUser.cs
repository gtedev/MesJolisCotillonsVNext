namespace MesJolisCotillons.DataAccess.Entities.EntityModels
{
    using MesJolisCotillons.DataAccess.Entities.Enums;

    public class CustomerUser
    {
        public int UserFk { get; set; }

        public CustomerUserStatus Status { get; set; }

        public string XmlData { get; set; }

        public virtual User User { get; set; }
    }
}
