using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class KeyWord
    {
        public KeyWord()
        {
            ProductKeyWord = new HashSet<ProductKeyWord>();
        }

        public int KeyWordId { get; set; }
        public string Value { get; set; }

        public ICollection<ProductKeyWord> ProductKeyWord { get; set; }
    }
}
