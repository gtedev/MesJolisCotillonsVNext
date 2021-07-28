using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.Models
{
    public abstract class Repository
    {
        protected MesJolisCotillonsEntities db = MesJoliesCotillonsDBContextManager.GetDbContext();

        public void  Save()
        {
            db.SaveChanges();
        }
    }
}