using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model.Azioni
{
    public abstract class AzioneRobot : Azione
    {
        public DenominazioneRobot Robot { get; set; }
        public int Turno { get; set; }
        public string Commento { get; set; }

        public override string Messaggio
        {
            get
            {
                return Commento;
            }
        }
    }
}
