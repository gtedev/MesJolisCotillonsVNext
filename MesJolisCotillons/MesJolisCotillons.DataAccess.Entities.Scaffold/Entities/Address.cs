using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class Address
    {
        public Address()
        {
            CustomerAddress = new HashSet<CustomerAddress>();
        }

        public int AddressId { get; set; }
        public string Address1 { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public int? CountryFk { get; set; }

        public Country CountryFkNavigation { get; set; }
        public CommandAddress CommandAddress { get; set; }
        public ICollection<CustomerAddress> CustomerAddress { get; set; }
    }
}
