using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class Country
    {
        public Country()
        {
            Address = new HashSet<Address>();
        }

        public int CountryId { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }

        public ICollection<Address> Address { get; set; }
    }
}
