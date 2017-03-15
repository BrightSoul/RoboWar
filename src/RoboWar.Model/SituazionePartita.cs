using RoboWar.Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model
{
    public class SituazionePartita
    {
        public SituazionePartita(DenominazioneRobot denominazione, Posizione posizione)
        {
            PunteggioPartita = 0;
            PunteggioRound = 0;
            Denominazione = denominazione;
            Posizione = posizione;
        }
        public int PunteggioPartita { get; private set; }
        public int PunteggioRound { get; private set; }
        public DenominazioneRobot Denominazione { get; private set; }
        public Posizione Posizione { get; private set; } 



        public void Sposta(Posizione posizione)
        {
            //TODO: deve esserci un massimo consentito
            Posizione = posizione;
        }

        internal void ResettaPunteggioRound()
        {
            PunteggioRound = 0;
        }
        internal void AssegnaPunti(int punti)
        {
            PunteggioPartita += punti;
            PunteggioRound += punti;
        }
    }
}
