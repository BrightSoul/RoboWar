using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using RoboWar.Robot;
using System.Reflection;
using RoboWar.Robot.Decisioni;

namespace RoboWar.BruttoRobot
{
    public class BruttoRobot : IRobot
    {
        public Stream Immagine
        {
            get
            {

                var assembly = this.GetType().GetTypeInfo().Assembly;
                var nomeRisorsaIncorporata = $"RoboWar.BruttoRobot.Robot.png";
                return assembly.GetManifestResourceStream(nomeRisorsaIncorporata);
            }
        }

        private static Random random = new Random();

        private string nome = $"BruttoRobot{random.Next(100, 1000).ToString()}";
        public string Nome
        {
            get
            {
                return nome;
            }
        }

        public IEnumerable<string> Proprietari
        {
            get
            {
                return new string[] { "Moreno" };
            }
        }

        private readonly string[] commenti = new string[] { "Kamehameha!", "Hey Ho!" };
        public Decisione Decidi(int turno, Dictionary<IRobot, ISituazione> partecipanti)
        {
            var altriPartecipanti = partecipanti.Select(r => r.Key).Except(new[] { this }).ToList();
            return new Attacca
            {
                 Commento = commenti[random.Next(commenti.Length)],
                 Bersaglio = altriPartecipanti[random.Next(altriPartecipanti.Count)]
            };

        }
        
    }
}
