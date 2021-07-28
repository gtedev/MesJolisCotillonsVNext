using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.ViewModels
{
    public class CustomerAccountViewModel
    {
        public string eMail { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public int? Address_FK { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string Phone { get; set; }

        public string Country { get; set; }

        public int? Country_FK { get; set; }

        public int? CustomerUser_FK { get; set; }

        public string itemSelectedId { get; set; }

        public List<Command> Command_Set { get; set; }

        public decimal OptionShipmentPrice { get; set; }

        public IEnumerable<CountryViewModel> Country_Set { get; set; }
    }
}