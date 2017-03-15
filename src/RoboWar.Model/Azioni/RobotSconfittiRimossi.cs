using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoboWar.Model.Azioni
{
    public class RobotSconfittiRimossi : Azione
    {
        public DenominazioneRobot[] RobotSconfitti { get; set; }

        public override string Messaggio
        {
            get
            {
                var sconfitti = "<nessuno>";
                if (RobotSconfitti?.Any() == true)
                {
                    sconfitti = string.Join(", ", RobotSconfitti.Select(r => r.ToString()));
                }
                return $"In questo round sono stati sconfitti i seguenti robot: {sconfitti}.";
            }
        }
    }
}
