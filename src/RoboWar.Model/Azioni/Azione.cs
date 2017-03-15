using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model.Azioni
{
    public abstract class Azione
    {
        public Guid IdPartita { get; set; }
        public abstract string Messaggio { get; }

        public override string ToString()
        {
            return Messaggio;
        }
    }
}
