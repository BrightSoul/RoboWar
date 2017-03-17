using System;
using System.Collections.Generic;
using System.Text;

namespace RoboWar.Model
{
    public interface IOpzioniPartita
    {
        TimeSpan TimeoutDenominazione { get; }
        TimeSpan TimeoutDecisione { get; }
        int DannoBase { get; }
        int VariazioneDanno { get; }
        int TurniMassimiPerRound { get; }
        int RoundTotali { get; }
        int PuntiPerTurno { get; }
        int PuntiPerVittoriaRound { get; } 
        int PuntiVita { get; }
        int DistanzaMinimaTraRobot { get; }
        int LarghezzaPianoDiGioco { get; }
        int AltezzaPianoDiGioco { get; }
    }
}
