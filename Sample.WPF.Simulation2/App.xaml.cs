using Gpio.Driver.Simulator;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Device.Gpio;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Sample.WPF.Simulation2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            /*
            IoDeviceService? _ioService;
        
            protected override void OnStartup(StartupEventArgs e)
            {
                base.OnStartup(e);

                VirtualGpioDriver driver = new VirtualGpioDriver();
                GpioController controller = new GpioController(PinNumberingScheme.Logical, driver);

                _ioService = new IoDeviceService(controller, driver);
                _ioService.Configure();

                MainWindow mainWindow = new MainWindow(_ioService);
                mainWindow.Show();

                SimulatorWindow simulatorWindow = new SimulatorWindow(_ioService);
                simulatorWindow.Show();
            }
            */


            MainWindow wnd = new MainWindow();
            wnd.Show();

            VirtualIOSignals ioSignals = new VirtualIOSignals();
            ioSignals.Add("Power", "24", "input");
            ioSignals.Add("CNC Program", "18", "input");
            ioSignals.Add("Production", "18", "counter");
            ioSignals.Add("Alert Msg", "26", "output");

            VirtualIOSimulatorControls ioBuilder = new VirtualIOSimulatorControls(ioSignals);
            ioBuilder.BuildWindow();
        }
    }

}
