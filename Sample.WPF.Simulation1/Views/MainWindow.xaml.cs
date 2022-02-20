using Sample.WPF.Simulation1.Services;
using Sample.WPF.Simulation1.ViewModels;
using System.Windows;

namespace Sample.WPF.Simulation1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IoDeviceService _ioService;

        public MainWindow(IoDeviceService ioService)
        {
            InitializeComponent();
            _ioService = ioService;
            DataContext = new MainWindowViewModel(_ioService);
        }
    }
}
