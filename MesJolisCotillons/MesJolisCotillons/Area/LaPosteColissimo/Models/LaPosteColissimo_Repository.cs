using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using MesJolisCotillons.ViewModels;
using MesJolisCotillons.Extensions.Paypal;

namespace MesJolisCotillons.Models
{
    public class LaPosteColissimo_Repository : Repository
    {
        public decimal FindColissimoCostValueByWeight(int Weight)
        {
            decimal result = 0;
            var weightItem = db.LaPosteColissimoes.Where(item => item.Weight_ID == Weight).FirstOrDefault();
            if (weightItem != null)
            {
                result = weightItem.Cost;
            }

            return result;
        }

        public void createAndAddLaPosteCost(int Weight_ID, decimal cost)
        {
            var item = new LaPosteColissimo
            {
                Weight_ID = Weight_ID,
                Cost = cost
            };

            db.LaPosteColissimoes.Add(item);
        }
    }
}