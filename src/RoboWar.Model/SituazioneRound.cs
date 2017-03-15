using RoboWar.Robot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model
{
    public class SituazioneRound : ISituazione
    {
        private readonly SituazionePartita situazionePartita;
        public SituazioneRound(int puntiVitaIniziali, SituazionePartita situazionePartita)
        {
            PuntiVitaResidui = puntiVitaIniziali;
            this.situazionePartita = situazionePartita;
        }

        internal void AssegnaPunti(int punti)
        {
            PuntiRound += punti;
            situazionePartita.AssegnaPunti(punti);
        }

        public int PuntiRound { get; private set; }

        public Posizione Posizione
        {
            get
            {
                return situazionePartita.Posizione;
            }
        }
        
        public int PuntiVitaResidui
        {
            get; private set;
        }
        internal void InfliggiDanno(int puntiVita)
        {
            PuntiVitaResidui -= puntiVita;
        }

        public bool Sconfitto
        {
            get {
                return PuntiVitaResidui <= 0;
            }
        }

    }
}
