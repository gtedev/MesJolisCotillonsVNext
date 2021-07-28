using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MesJolisCotillons.Extensions
{
    public static class ProductExtensions
    {
        public static IEnumerable<string> GetProductStreamInBase64Images(this Product product)
        {

            return product.Blob_Set
                          .Where(p => p.Stream != null)
                          .Select(p => Convert.ToBase64String(p.Stream));
        }
    }
}