using Gpio.Driver.Simulator;
using Sample.WPF.Simulation2.Services;
using System.Device.Gpio;
using System.Windows;

namespace Sample.WPF.Simulation2
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

            VirtualGpioDriver driver = new VirtualGpioDriver();

            GpioController controller = new GpioController(PinNumberingScheme.Logical, driver);

            _ioService = new IoDeviceService(controller, driver);
            _ioService.Configure();

            MainWindow mainWnd = new MainWindow(_ioService);
            mainWnd.Show();

            //
            // Set-up a virtual scenario that can be shared by all virtual managers (UWP, WPF, WinUI, WinForms)
            //
            VirtualIOScenario ioScenario = VirtualIOScenario
                                            .Create(controller, driver)
                                            .Add("Power", IOPins.Power, VirtualPinType.Input)
                                            .Add("CNC Program", IOPins.Run, VirtualPinType.Input)
                                            .Add("Production", IOPins.NewUnit, VirtualPinType.Counter)
                                            .Add("Stop Alert", IOPins.Alert, VirtualPinType.Output);

            //
            // Virtual manager window for WPF apps
            //
            VirtualIOWpfManager ioManager = new VirtualIOWpfManager(ioScenario);
            ioManager.Run();
        }
    }

}
