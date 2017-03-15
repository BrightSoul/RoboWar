using RoboWar.Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model.Azioni
{
    public class Attacco : AzioneRobot
    {
        public DenominazioneRobot Bersaglio { get; set; }
        public int Danno { get; set; }
        public int PuntiVitaResiduiDelBersaglio { get; set; }

        public override string Messaggio
        {
            get
            {
                var nomeBersaglio = Bersaglio.Nome ?? "<null>";
                return $"Il robot {Robot?.Nome} infligge {Danno} punti di danno a {Bersaglio?.Nome} che ora ha {PuntiVitaResiduiDelBersaglio} punti residui. {base.Messaggio}"; 
            }
        }
    }
}
