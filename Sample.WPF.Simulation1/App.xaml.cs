using Gpio.Driver.Simulator;
using Sample.WPF.Simulation1.Services;
using System.Device.Gpio;
using System.Windows;

namespace Sample.WPF.Simulation1
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IoDeviceService? _ioService;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GpioDriverSimulator driver = new GpioDriverSimulator();
            GpioController controller = new GpioController(PinNumberingScheme.Logical, driver);

            _ioService = new IoDeviceService(controller, driver);
            _ioService.Configure();

            MainWindow mainWindow = new MainWindow(_ioService);
            mainWindow.Show();

            SimulatorWindow simulatorWindow = new SimulatorWindow(_ioService);
            simulatorWindow.Show();
        }
    }
}
