using RoboWar.BruttoRobot;
using RoboWar.Desktop.Helpers;
using RoboWar.Desktop.Views;
using RoboWar.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
                await Task.Delay(500);
                Log.Add(faseDiGioco.ToString());
            }
        }
    }
}
