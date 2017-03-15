using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model.Azioni
{
    public class ClassificaFinaleGenerata : Azione
    {
        public SituazionePartita[] Classifica { get; set; }
        public override string Messaggio
        {
            get
            {
                var classifica = "";
                if (Classifica != null)
                {
                    classifica = string.Join(", ", Classifica.OrderByDescending(p => p.PunteggioPartita).Select(p => $"[{p.PunteggioPartita} punti] {p.Denominazione}"));
                }
                
                return $"Fine della partita. Classifica finale: {classifica}.";
            }
        }
    }
}
