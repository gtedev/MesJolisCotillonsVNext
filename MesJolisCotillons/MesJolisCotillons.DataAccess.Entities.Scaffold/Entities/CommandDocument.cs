using System;
using System.Collections.Generic;

namespace MesJolisCotillons.DataAccess.Entities.Scaffold.Entities
{
    public partial class CommandDocument
    {
        public int DocumentFk { get; set; }
        public int CommandFk { get; set; }

        public Command CommandFkNavigation { get; set; }
        public Document DocumentFkNavigation { get; set; }
    }
}
