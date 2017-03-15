using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model
{
    public enum StatiPartita : int
    {
        PartitaDaIniziare,
        RoundDaIniziare,
        TurnoDaIniziare,
        TurnoIniziato,
        TurnoFinito,
        RoundFinito,
        ProclamazioneVincitoriDelRound,
        ProclamazioneVincitoriDellaPartita,
        PartitaFinita
    }
}
