using RoboWar.Robot;
using RoboWar.Robot.Decisioni;
using RoboWar.Model.Extensions;
using RoboWar.Model.Azioni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoboWar.Model
{
    public class Partita
    {
        private readonly Dictionary<IRobot, SituazionePartita> robotPartecipanti;
        private readonly Dictionary<IRobot, SituazioneRound> robotNelRoundCorrente;
        private readonly OpzioniPartita opzioni;
        private StatiPartita statoPartita;
        private Queue<IRobot> coda;
        private readonly Random random;
        
        public Partita(OpzioniPartita opzioni = null)
        {
            this.opzioni = opzioni ?? new OpzioniPartita();
            this.opzioni.Convalida();

            IdPartita = Guid.NewGuid();
            statoPartita = StatiPartita.PartitaDaIniziare;
            robotPartecipanti = new Dictionary<IRobot, SituazionePartita>();
            robotNelRoundCorrente = new Dictionary<IRobot, SituazioneRound>();
            coda = new Queue<IRobot>();
            RegistroAzioni = new List<Azione>();
            random = new Random();
        }

        public Guid IdPartita
        {
            get;
        }

        public IOpzioniPartita Opzioni
        {
            get
            {
                return opzioni;
            }
        }

        public List<Azione> RegistroAzioni { get;}

        public int RoundCorrente
        {
            get; private set;
        }

        public int RoundRimanenti
        {
            get
            {
                return opzioni.RoundTotali - RoundCorrente;
            }
        }

        public int TurnoCorrente
        {
            get; private set;
        }

        public int TurniRimanenti
        {
            get
            {
                return opzioni.TurniMassimiPerRound - TurnoCorrente;
            }
        }

        public SituazionePartita AggiungiPartecipante(IRobot robot)
        {
            if (statoPartita != StatiPartita.PartitaDaIniziare)
                throw new InvalidOperationException("Si possono aggiungere partecipanti solo prima dell'inizio della partita");

            var posizione = GeneraPosizioneCasuale(robotPartecipanti.Values.Select(p => p.Posizione));
            var denominazione = robot.DenominazioneConTimeout(opzioni.TimeoutDenominazione);
            var situazionePartita = new SituazionePartita(denominazione, posizione);
            robotPartecipanti.Add(robot, situazionePartita);
            return situazionePartita;
        }

        public IEnumerable<Azione> Gioca()
        {
            foreach (var azione in EseguiAzioni())
            {
                azione.IdPartita = IdPartita;
                RegistroAzioni.Add(azione);
                yield return azione;
            }
        }

        
        private Posizione GeneraPosizioneCasuale(IEnumerable<Posizione> posizioniEsistenti)
        {
            double distanzaMinima = 0;
            int tentativi = 10;
            Tuple<Posizione, double> migliorePosizione = null;
            do
            {
                var posizione = new Posizione(random.Next(0, opzioni.LarghezzaPianoDiGioco), random.Next(0, opzioni.AltezzaPianoDiGioco));
                if (posizioniEsistenti.Any())
                {
                    distanzaMinima = posizioniEsistenti.Min(p => CalcolaDistanzaTraDuePosizioni(p, posizione));
                    if (migliorePosizione == null || migliorePosizione.Item2 < distanzaMinima)
                    {
                        migliorePosizione = new Tuple<Posizione, double>(posizione, distanzaMinima);
                    }
                } else
                {
                    return posizione;
                }
            } while (distanzaMinima < opzioni.DistanzaMinimaTraRobot && --tentativi >= 0);

            return migliorePosizione.Item1;

        }

        private double CalcolaDistanzaTraDuePosizioni(Posizione posizione1, Posizione posizione2)
        {
            return Math.Sqrt(Math.Pow(posizione1.X - posizione2.X, 2.0) + Math.Pow(posizione1.Y - posizione2.Y, 2.0));
        }

        private IEnumerable<Azione> EseguiAzioni()
        {
            while (true)
            {
                switch (statoPartita)
                {
                    case StatiPartita.PartitaDaIniziare:
                        yield return PreparaLaPartita();
                        CambiaStato(StatiPartita.RoundDaIniziare);
                        break;

                    case StatiPartita.RoundDaIniziare:
                        yield return PreparaIlRound();
                        CambiaStato(StatiPartita.TurnoDaIniziare);
                        break;

                    case StatiPartita.TurnoDaIniziare:
                        yield return PreparaIlTurno();
                        CambiaStato(StatiPartita.TurnoIniziato);
                        break;

                    case StatiPartita.TurnoIniziato:
                        if (coda.Count > 0)
                            yield return FaiAgireIlProssimoRobot();
                        else
                            CambiaStato(StatiPartita.TurnoFinito);
                        break;

                    case StatiPartita.TurnoFinito:
                        yield return RimuoviRobotSconfitti();

                        if (robotNelRoundCorrente.Count > 1 && TurniRimanenti > 0)
                            CambiaStato(StatiPartita.TurnoDaIniziare);
                        else if (RoundRimanenti > 0)
                            CambiaStato(StatiPartita.RoundFinito);
                        else
                            CambiaStato(StatiPartita.PartitaFinita);
                        break;

                    case StatiPartita.RoundFinito:
                        yield return GeneraClassificaTemporanea();
                        CambiaStato(StatiPartita.RoundDaIniziare);
                        break;

                    case StatiPartita.PartitaFinita:
                        
                        yield return ProclamaVincitoriDellaPartita();
                        //non c'è più niente da fare
                        yield break;
                }
            }
        }

        internal void CambiaStato(StatiPartita statoPartita)
        {
            this.statoPartita = statoPartita;
        }

        private Azione PreparaLaPartita()
        {
            RoundCorrente = 0;
            return new PartitaIniziata
            {
                Partecipanti = robotPartecipanti.Select(p => p.Value.Denominazione).ToArray()
            };
        }
        private Azione PreparaIlRound()
        {
            robotNelRoundCorrente.Clear();
            foreach (var partecipante in robotPartecipanti)
            {
                var situazione = CreaSituazioneInizialePerIlRobot(partecipante.Value);
                robotNelRoundCorrente.Add(partecipante.Key, situazione);
                partecipante.Value.ResettaPunteggioRound();
            }

            RoundCorrente++;
            TurnoCorrente = 0;
            return new RoundIniziato
            {
                Round = RoundCorrente
            };
        }

        private Azione PreparaIlTurno()
        {
            //Generiamo una coda in base alla vitalità residua
            coda.Clear();
            var robotOrdinati = robotNelRoundCorrente
                .OrderBy(robot => robot.Value.PuntiVitaResidui)
                .ThenBy(robot => random.NextDouble())
                .Select(robot => robot.Key);

            foreach (var robot in robotOrdinati) {
                coda.Enqueue(robot);
            }
            TurnoCorrente++;
            return new TurnoIniziato
            {
                Turno = TurnoCorrente,
                Round = RoundCorrente
            };
        }

        private AzioneRobot FaiAgireIlProssimoRobot()
        {
            var robot = coda.Dequeue();
            var decisione = robot.DecidiConTimeout(
                TurnoCorrente,
                robotNelRoundCorrente.ToDictionary(r => r.Key, r => (ISituazione)r.Value),
                opzioni.TimeoutDecisione);

            AzioneRobot azione = null;

            var tipoDecisione = decisione?.GetType();
            if (tipoDecisione == typeof(Attacca))
            {
                var bersaglio = (decisione as Attacca).Bersaglio;
                if (bersaglio != null && robotNelRoundCorrente.ContainsKey(bersaglio))
                {
                    var situazione = robotNelRoundCorrente[bersaglio];
                    var danno = CalcolaDanno(opzioni.DannoBase, opzioni.VariazioneDanno);
                    situazione.InfliggiDanno(danno);
                    azione = new Attacco
                    {
                        Bersaglio = robotPartecipanti[bersaglio].Denominazione,
                        Danno = danno,
                        PuntiVitaResiduiDelBersaglio = situazione.PuntiVitaResidui
                    };
                }
            }

            azione = azione ?? new TurnoSaltato();
            if (robotPartecipanti.ContainsKey(robot))
            {
                azione.Robot = robotPartecipanti[robot].Denominazione;
            }
            azione.Turno = TurnoCorrente;
            return azione;
        }

        private Azione RimuoviRobotSconfitti()
        {
            var robotSconfitti = robotNelRoundCorrente.Where(robot => robot.Value.Sconfitto).Select(robot => robot.Key).ToList();
            foreach (var robot in robotSconfitti)
            {
                robotNelRoundCorrente.Remove(robot);
            }

            //I robot sopravvissuti al round corrente prendono punti
            foreach (var robot in robotNelRoundCorrente)
            {
                robot.Value.AssegnaPunti(opzioni.PuntiPerTurno);
            }

            return new RobotSconfittiRimossi
            {
                RobotSconfitti = robotSconfitti.Select(robot => robotPartecipanti[robot].Denominazione).ToArray()
            };
        }

        private Azione GeneraClassificaTemporanea()
        {
            return new ClassificaProvvisoriaGenerata
            {
                Turno = TurnoCorrente,
                Classifica = robotPartecipanti.Values.ToArray()
            };
        }

        private Azione ProclamaVincitoriDellaPartita()
        {
            if (robotNelRoundCorrente.Count == 1)
            {
                //Assegniamo i punti della vittoria solo se è rimasto un robot
                var robotVincitore = robotNelRoundCorrente.First();
                robotVincitore.Value.AssegnaPunti(opzioni.PuntiPerVittoriaRound);
            }

            return new ClassificaFinaleGenerata
            {
                  Classifica = robotPartecipanti.Values.ToArray()
            };
        }
 

        private SituazioneRound CreaSituazioneInizialePerIlRobot(SituazionePartita situazionePartita)
        {
            return new SituazioneRound(puntiVitaIniziali: opzioni.PuntiVita, situazionePartita: situazionePartita);
        }
    
        private int CalcolaDanno(int dannoBase, int variazioneDanno)
        {
            if (variazioneDanno <= 0)
                return dannoBase;

            var variazione = (random.NextDouble() * 2 -1.0) * variazioneDanno;
            var danno = Convert.ToInt32(Math.Round(dannoBase + variazione));
            return danno;
        }
        
    }
}
