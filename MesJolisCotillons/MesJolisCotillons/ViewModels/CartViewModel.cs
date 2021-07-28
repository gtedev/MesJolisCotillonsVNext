using System.Collections.Generic;

namespace MesJolisCotillons.ViewModels
{
    public class CartViewModel
    {
        public MySessionCartModel SessionCartModel { get; set; }

        public Dictionary<int, byte[]> ProductIdBlobs { get; set; }
    }
}