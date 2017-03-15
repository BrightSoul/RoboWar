using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model
{
    public class DenominazioneRobot
    {
        public string Nome { get; set; }
        public string NomeCompleto { get; set; }
        public string[] Proprietari { get; set; }

        private const string NullString = "<null>";
        public override string ToString()
        {
            var nome = Nome ?? NullString;
            string proprietari = NullString;
            if (Proprietari?.Any() == true)
            {
                proprietari = string.Join(", ", Proprietari.Select(p => p ?? NullString));
            }
            return $"{nome} ({proprietari})";
        }
    }
}
