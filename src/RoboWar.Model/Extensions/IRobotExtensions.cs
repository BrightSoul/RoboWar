using RoboWar.Robot;
using RoboWar.Model;
using RoboWar.Model.Decisioni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RoboWar.Model.Extensions
{
    public static class IRobotExtensions
    {

        private static TRisultato EseguiTaskConTimeout<TRisultato>(Func<TRisultato> func, Func<TRisultato> valoreDefault, TimeSpan timeout)
        {
            var tokenSource = new CancellationTokenSource();
            TRisultato risultato;
            var task = Task.Run(func, tokenSource.Token);
            var resultedTask = Task.WhenAny(task, Task.Delay(TimeSpan.FromMilliseconds(100))).Result;
            if (resultedTask == task)
            {
                risultato = task.Result;
            }
            else
            {
                tokenSource.Cancel();
                risultato = valoreDefault();
            }
            return risultato;
        }

        public static DenominazioneRobot DenominazioneConTimeout(this IRobot robot, TimeSpan timeout)
        {

            //Attenzione: questa implementazione che si avvale dei task
            //serve solo nel momento in cui volete evitare che implementazioni
            //di terze parti influenzino il corretto comportamento del programma
            //per tutti gli altri componenti del software.
            //E' una tecnica di "defensive programming"
            var denominazione = EseguiTaskConTimeout(
            () => new DenominazioneRobot
            {
                Nome = robot.Nome,
                NomeCompleto = robot.GetType().AssemblyQualifiedName,
                Proprietari = robot.Proprietari.ToArray()
            },
            () => new DenominazioneRobot(),
            timeout
            );

            return denominazione;
        }

        public static Decisione DecidiConTimeout(this IRobot robot, int turno, Dictionary<IRobot, ISituazione> partecipanti, TimeSpan timeout)
        {
            var decisione = EseguiTaskConTimeout(
                () => robot.Decidi(turno, partecipanti),
                () => new SaltaIlTurnoPerTimeout(),
                timeout
                );
            return decisione;
        }

        private static object EseguiTaskConTimeout<TRisultato>(Func<TRisultato> p1, object p2)
        {
            throw new NotImplementedException();
        }
    }
}
