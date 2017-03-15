using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model.Azioni
{
    public class TurnoSaltatoPerTimeout : AzioneRobot
    {
        public override string Messaggio
        {
            get
            {
                return $"Il robot {Robot.Nome} salta il turno perché non ha deciso in tempo.";
            }
        }
    }
}
