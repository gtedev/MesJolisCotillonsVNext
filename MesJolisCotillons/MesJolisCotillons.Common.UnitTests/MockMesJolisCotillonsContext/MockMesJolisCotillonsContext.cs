using MesJolisCotillons.DataAccess.Entities.Context;
using Microsoft.EntityFrameworkCore;
using System;

namespace MesJolisCotillons.Common.UnitTests.MockMesJolisCotillonsContext 
{
    public class MockMesJolisCotillonsContext : MesJolisCotillonsContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }
    }
}
