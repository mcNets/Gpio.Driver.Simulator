using System.Device.Gpio;
using Gpio.Driver.Simulator;

namespace Sample.WPF.Simulation1.Services
{
    public class IoDeviceService
    {
        public IoDeviceService(GpioController controller, GpioDriver driver)
        {
            Controller = controller;
            Driver = driver;
        }

        public GpioController Controller { get; }
        public GpioDriver Driver { get; }

        public void Configure()
        {
            Controller.OpenPin(IOPins.Power, PinMode.Input, PinValue.Low);
            Controller.OpenPin(IOPins.Run, PinMode.Input, PinValue.Low);
            Controller.OpenPin(IOPins.NewUnit, PinMode.Input, PinValue.Low);
            Controller.OpenPin(IOPins.Alert, PinMode.Output);
        }
    }
}
