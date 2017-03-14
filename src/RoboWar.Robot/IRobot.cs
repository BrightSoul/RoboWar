using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Robot
{
    public interface IRobot
    {
        string Nome { get; }
        IEnumerable<string> Proprietari { get; }
        Stream Immagine { get; }
        //string GridoDiVittoria

        Decisione Decidi(int turno, Dictionary<IRobot, ISituazione> partecipanti);

    }
}
