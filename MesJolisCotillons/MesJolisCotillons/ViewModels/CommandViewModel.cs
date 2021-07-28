using MesJolisCotillons.Area._Admin;
using MesJolisCotillons.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesJolisCotillons.ViewModels
{
    public class CommandViewModel
    {
        public string BaseUrl { get; set; }
        public Command Command { get; set; }
    }
}