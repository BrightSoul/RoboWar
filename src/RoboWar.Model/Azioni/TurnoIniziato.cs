using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model.Azioni
{
    public class TurnoIniziato : Azione
    {
        public int Turno { get; set; }
        public int Round { get; set; }
        public override string Messaggio
        {
            get
            {
                return $"Inizia il turno {Turno} del round {Round}.";
            }
        }
    }
}
