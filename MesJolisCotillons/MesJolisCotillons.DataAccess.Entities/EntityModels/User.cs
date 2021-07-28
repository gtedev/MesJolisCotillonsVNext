namespace MesJolisCotillons.DataAccess.Entities.EntityModels
{
    public class User
    {
        public int UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Salt { get; set; }

        public virtual AdminUser AdminUser { get; set; }

        public virtual CustomerUser CustomerUser { get; set; }
    }
}
