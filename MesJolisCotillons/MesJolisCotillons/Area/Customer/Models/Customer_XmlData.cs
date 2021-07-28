
using MesJolisCotillons.Extensions.XmlData;
using System.Collections.Generic;

namespace MesJolisCotillons.Area.Customer
{
    public class Customer_XmlData : XmlSerializer<Customer_XmlData>
    {
        public InsriptionConfirmationProcess InsriptionConfirmationProcess { get; set; }
        public List<RedefinitionPasswordProcess> RedefinitionPasswordProcess_Set { get; set; } = new List<RedefinitionPasswordProcess>();

        public string Phone { get; set; }
    }
}