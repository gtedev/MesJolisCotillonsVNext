namespace MesJolisCotillons.DataAccess.Entities.EntityModels
{
    public class AdminUser
    {
        public int UserFk { get; set; }

        public virtual User User { get; set; }
    }
}
