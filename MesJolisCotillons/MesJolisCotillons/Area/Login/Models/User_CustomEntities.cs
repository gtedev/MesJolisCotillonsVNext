using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.Models
{
    public partial class User
    {
        public string FullName
        {
            get
            {
                //return this.FirstName + ", " + this.LastName;
                return this.FirstName + " " + this.LastName;
            }
        }
    }
}