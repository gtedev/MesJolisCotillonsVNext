using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class CommandHistory
    {
        public int CommandHistoryId { get; set; }
        public int CommandFk { get; set; }
        public int CommandHistoryAction { get; set; }
        public string XmlData { get; set; }
        public DateTime Date { get; set; }

        public Command CommandFkNavigation { get; set; }
    }
}
