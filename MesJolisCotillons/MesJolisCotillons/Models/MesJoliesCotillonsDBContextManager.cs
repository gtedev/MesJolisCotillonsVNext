using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;

namespace MesJolisCotillons.Models
{
    public class MesJoliesCotillonsDBContextManager
    {
        private const string ContextKey = "EF.DbContext.MesJolisCotillonsEntities";

        public static MesJolisCotillonsEntities GetDbContext()
        {
            MesJolisCotillonsEntities dbContext = GetCurrentDbContext();
            if (dbContext == null)
            {
                dbContext = new MesJolisCotillonsEntities();
                dbContext.Configuration.LazyLoadingEnabled = true;

#if DEBUG
                dbContext.Database.Log = sql => Debug.Write(sql);
#endif

                StoreCurrentDbContext(dbContext);
            }
            return dbContext;
        }

        private static void StoreCurrentDbContext(MesJolisCotillonsEntities dbContext)
        {
            if (HttpContext.Current != null)
            {
                if (HttpContext.Current.Items.Contains(ContextKey))
                    HttpContext.Current.Items[ContextKey] = dbContext;
                else
                    HttpContext.Current.Items.Add(ContextKey, dbContext);
            }
        }

        private static MesJolisCotillonsEntities GetCurrentDbContext()
        {
            MesJolisCotillonsEntities dbContext = null;
            if (HttpContext.Current != null && HttpContext.Current.Items.Contains(ContextKey))
                dbContext = (MesJolisCotillonsEntities)HttpContext.Current.Items[ContextKey];
            else
                dbContext = (MesJolisCotillonsEntities)Thread.GetData(Thread.GetNamedDataSlot(ContextKey));

            return dbContext;
        }

        public static void RemoveCurrentDbContext()
        {
            MesJolisCotillonsEntities dbContext = GetCurrentDbContext();
            if (dbContext != null)
            {
                if (HttpContext.Current != null)
                    HttpContext.Current.Items.Remove(ContextKey);
                if (Thread.GetData(Thread.GetNamedDataSlot(ContextKey)) != null)
                    Thread.SetData(Thread.GetNamedDataSlot(ContextKey), null);

                dbContext.Dispose();
            }
        }
    }
}