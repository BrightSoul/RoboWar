using RoboWar.Desktop.Models;
using RoboWar.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RoboWar.Desktop.Views
{
    /// <summary>
    /// Logica di interazione per Robot.xaml
    /// </summary>
    public partial class Robot : UserControl, ICanvasView
    {
        public Robot()
        {
            InitializeComponent();
        }

        public double Left => ((SituazionePartita) DataContext).Posizione.X;

        public double Top => ((SituazionePartita) DataContext).Posizione.Y;
    }
}
