using Sample.WPF.Simulation1.Services;
using Sample.WPF.Simulation1.ViewModels;
using System.Windows;

namespace Sample.WPF.Simulation1
{
    /// <summary>
    /// Interaction logic for SimulatorWindow.xaml
    /// </summary>
    public partial class SimulatorWindow : Window
    {
        private IoDeviceService _ioService;

        public SimulatorWindow(IoDeviceService ioService)
        {
            InitializeComponent();
            _ioService = ioService;
            DataContext = new SimulationViewModel(ioService);
        }
    }
}
