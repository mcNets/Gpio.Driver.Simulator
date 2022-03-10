using Sample.WPF.Simulation2.Services;
using Sample.WPF.Simulation2.ViewModels;
using System.Windows;

namespace Sample.WPF.Simulation2
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
