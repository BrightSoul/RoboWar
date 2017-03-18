using RoboWar.Desktop.Models;
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
    /// Logica di interazione per Turno.xaml
    /// </summary>
    public partial class Turno : UserControl, ICanvasView
    {
        public Turno()
        {
            InitializeComponent();
        }

        public double Left => 0.5;

        public double Top => 0;

        public TimeSpan Delay => TimeSpan.FromSeconds(1);
    }
}
