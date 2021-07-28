namespace MesJolisCotillons.DataAccess.Entities.Context
{
    using System;
    using MesJolisCotillons.DataAccess.Entities.EntityModels;
    using Microsoft.EntityFrameworkCore;

    public interface IDbContext : IDisposable
    {
         DbSet<Blob> Blob { get; set; }
    }
}
