using System.Device.Gpio;

namespace Sample.WPF.Simulation2.Services
{
    public static class IOPins
    {
        public static int Power = 8;
        public static int Run = 10;
        public static int NewUnit = 12;
        public static int Alert = 16;
    }

    public class IoDeviceService
    {
        private readonly GpioController _controller;
        private readonly GpioDriver _driver;

        public IoDeviceService(GpioController controller, GpioDriver driver)
        {
            _controller = controller;
            _driver = driver;
        }

        public GpioController Controller => _controller;

        public GpioDriver Driver => _driver;

        public void Configure()
        {
            _controller.OpenPin(IOPins.Power, PinMode.Input, PinValue.Low);
            _controller.OpenPin(IOPins.Run, PinMode.Input, PinValue.Low);
            _controller.OpenPin(IOPins.NewUnit, PinMode.Input, PinValue.Low);
            _controller.OpenPin(IOPins.Alert, PinMode.Output, PinValue.Low);
        }
    }
}
