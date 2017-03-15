using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model.Azioni
{
    public class PartitaIniziata : Azione
    {
        public DenominazioneRobot[] Partecipanti { get; set; }

        public override string Messaggio
        {
            get
            {
                var partecipanti = string.Join(", ", Partecipanti.Select(p => p.ToString()));
                return $"Partita iniziata con {Partecipanti.Length} robot partecipanti: {partecipanti}.";
            }
        }
    }
}
