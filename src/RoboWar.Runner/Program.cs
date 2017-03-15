using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using RoboWar.Model;
using RoboWar.Model.Azioni;

namespace RoboWar.Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            var partita = CreaPartita();
            var azioniDiGioco = partita.Gioca();
            azioniDiGioco = Logga(azioniDiGioco);
            azioniDiGioco = Visualizza(azioniDiGioco);
            StampaRiepilogo(azioniDiGioco);
        }

        private static Partita CreaPartita() {
            var partita = new Partita();
            partita.AggiungiPartecipante(new BruttoRobot.BruttoRobot());
            partita.AggiungiPartecipante(new BruttoRobot.BruttoRobot());
            partita.AggiungiPartecipante(new BruttoRobot.BruttoRobot());
            partita.AggiungiPartecipante(new BruttoRobot.BruttoRobot());
            return partita;
        }

        private static IEnumerable<Azione> Logga(IEnumerable<Azione> azioniDiGioco) {
            var filePath = Path.Combine(System.AppContext.BaseDirectory, "log.txt");
            using (var file = File.CreateText(filePath)) {
                foreach (var azioneDiGioco in azioniDiGioco) {
                    yield return azioneDiGioco;
                }
            }
        }

        private static IEnumerable<Azione> Visualizza(IEnumerable<Azione> azioniDiGioco) {
            foreach (var azioneDiGioco in azioniDiGioco) {
                Console.WriteLine(azioneDiGioco.ToString());
                if (azioneDiGioco is ClassificaFinaleGenerata) {
                    var numeroMassimoBlocchi = 50;
                    var classifica = (azioneDiGioco as ClassificaFinaleGenerata).Classifica.OrderByDescending(posizione => posizione.PunteggioPartita);
                    var massimiPuntiConquistati = classifica.Max(c => c.PunteggioPartita);
                    Console.WriteLine();
                    Console.WriteLine("=== CLASSIFICA FINALE ===");
                    var caratteriMassimi = Math.Min(30, classifica.Max(c => (c.Denominazione?.ToString().Length ?? 0)));
                    foreach (var posizione in classifica) {
                        var numeroBlocchi = Convert.ToInt32((posizione.PunteggioPartita / (double) massimiPuntiConquistati) * numeroMassimoBlocchi);
                        var blocchi = "".PadRight(numeroBlocchi, (char) 0x2593).PadRight(numeroMassimoBlocchi, ' ');
                        var denominazione = posizione.Denominazione?.ToString() ?? string.Empty;
                        if (denominazione.Length > caratteriMassimi) 
                            denominazione = denominazione.Substring(0, caratteriMassimi);
                        denominazione = denominazione.PadRight(caratteriMassimi, ' ');
                        var punti = posizione.PunteggioPartita;
                        Console.WriteLine($"{denominazione} [{blocchi}] {punti}p");
                    }
                    Console.WriteLine();
                }

                yield return azioneDiGioco;
            }
        }

        private static void StampaRiepilogo(IEnumerable<Azione> azioniDiGioco) {
            var cronometro = new Stopwatch();
            cronometro.Start();
            Console.WriteLine("Partita in corso...");
            var numeroAzioni = azioniDiGioco.Count(); //Questo itera attraverso tutte le fasi di gioco
            cronometro.Stop();
            Console.WriteLine($"La partita si conclude dopo {numeroAzioni} azioni e con un tempo di {cronometro.Elapsed}.");
        }
    }
}
