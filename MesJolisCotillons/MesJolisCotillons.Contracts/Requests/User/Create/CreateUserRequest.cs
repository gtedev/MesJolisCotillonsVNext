namespace MesJolisCotillons.Contracts.Requests.User.Create
{
    using System.ComponentModel.DataAnnotations;

    public class CreateUserRequest : IRequest
    {
        public string Country { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmedPassword { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string ZipCode { get; set; }

        public string City { get; set; }

        public string Phone { get; set; }
    }
}
