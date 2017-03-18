using Newtonsoft.Json;
using RoboWar.BruttoRobot;
using RoboWar.Desktop.Helpers;
using RoboWar.Desktop.Models;
using RoboWar.Desktop.Views;
using RoboWar.Model;
using RoboWar.Model.Azioni;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RoboWar.Desktop.ViewModels
{

    public class MainWindow : ViewModel
    {
        public ObservableCollection<UserControl> Robots { get; private set; }
        public ObservableCollection<UserControl> Layers { get; private set; }
        public ObservableCollection<string> Log { get; private set; }
        private readonly Partita partita;
        public ICommand ComandoAvviaPartita { get; private set; }

        public int LarghezzaPianoDiGioco { get; private set; }
        public int AltezzaPianoDiGioco { get; private set; }
        private readonly string filename = "log." + DateTime.Now.ToString("yyyyMMddHHmmss");
        public MainWindow()
        {
            partita = new Partita();
            LarghezzaPianoDiGioco = partita.Opzioni.LarghezzaPianoDiGioco;
            AltezzaPianoDiGioco = partita.Opzioni.AltezzaPianoDiGioco;
            Robots = new ObservableCollection<UserControl>();
            Layers = new ObservableCollection<UserControl>();
            Log = new ObservableCollection<string>();
            for (var i = 0; i<4; i++)
            {
                var situazione = partita.AggiungiPartecipante(new BruttoRobot.BruttoRobot());
                Robots.Add(new Views.Robot { DataContext = situazione });
            }

            ComandoAvviaPartita = new RelayCommand(AvviaPartita);


        }

        private async void AvviaPartita(object parameter)
        {
            var fasiDiGioco = partita.Gioca();
            foreach (var faseDiGioco in fasiDiGioco)
            {
                Logga(faseDiGioco);
                if (faseDiGioco  is TurnoIniziato)
                {
                    var layer = new Turno { DataContext = faseDiGioco };
                    await AggiungiLayers(layer);
                } else
                {
                    await AggiungiLayers();
                }
            }
        }

        private async Task AggiungiLayers(params UserControl[] layers)
        {
            layers = layers ?? new UserControl[] { };
            Layers.Clear();

            foreach (var layer in layers) {
                Layers.Add(layer);
            }

            //var delay = layers.OfType<ICanvasView>().DefaultIfEmpty().Max(c => c.Delay);
            var delay = TimeSpan.Zero;
            if (delay < TimeSpan.FromMilliseconds(500))
                delay = TimeSpan.FromMilliseconds(500);
            await Task.Delay(delay);
              
        }

        private void Logga(Azione azione)
        {
            var root = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            Log.Add(azione.ToString());
            var azioneSerializzata = JsonConvert.SerializeObject(azione);
            File.AppendAllText(Path.Combine(root, $"{filename}.json"), $"{DateTime.Now.ToString("yyyy/MM/dd HH.mm.ss")}\t{azioneSerializzata}");
            File.AppendAllText(Path.Combine(root, $"{filename}.txt"), azione.ToString());
        }
    }
}
