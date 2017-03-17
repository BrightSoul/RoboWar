using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model
{
    public class OpzioniPartita : IOpzioniPartita
    {
        public TimeSpan TimeoutDenominazione { get; set; } = TimeSpan.FromMilliseconds(100);
        public TimeSpan TimeoutDecisione { get; set; } = TimeSpan.FromMilliseconds(2000);
        public int DannoBase { get; set; } = 190;
        public int VariazioneDanno { get; set; } = 10;
        public int TurniMassimiPerRound { get; set; } = 10;
        public int RoundTotali { get; set; } = 3;
        public int PuntiPerTurno { get; set; } = 1;
        public int PuntiPerVittoriaRound { get; set; } = 2;
        public int PuntiVita { get; set; } = 1000;
        public int DistanzaMinimaTraRobot { get; set; } = 100;
        public int LarghezzaPianoDiGioco { get; set; } = 1000;
        public int AltezzaPianoDiGioco { get; set; } = 1000;

        internal void Convalida()
        {
            if (RoundTotali <= 0)
                throw new InvalidOperationException("Non puoi creare una partita che non abbia almeno un round");

            if (TurniMassimiPerRound <= 0)
                throw new InvalidOperationException("Un round deve svolgersi in almeno un turno");

            if (TimeoutDecisione <= TimeSpan.Zero)
                throw new InvalidOperationException("Non puoi indicare un timeout uguale a zero per compiere la decisione.");

            if (TimeoutDenominazione <= TimeSpan.Zero)
                throw new InvalidOperationException("Non puoi indicare un timeout uguale a zero per la lettura della denominazione.");

            if (DannoBase <= 0)
            {
                throw new InvalidOperationException("Il danno base non può infliggere 0 o meno punti, altrimenti la partita non finirebbe mai");
            }

            if (VariazioneDanno < 0)
            {
                throw new InvalidOperationException("La variazione del danno non può essere negativa");
            }
        }
    }
}
