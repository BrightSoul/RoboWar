using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Robot.Decisioni
{
    /// <summary>
    /// Infligge un massimo di 180 punti danno al bersaglio
    /// </summary>
    public class Attacca : Decisione
    {
        public IRobot Bersaglio { get; set; }
    }
}
