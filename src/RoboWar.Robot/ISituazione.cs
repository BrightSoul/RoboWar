using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Robot
{
    public interface ISituazione
    {
        //Ad ogni round, ogni robot ha 1000 punti vita
        int PuntiVitaResidui { get; }
    }
}
