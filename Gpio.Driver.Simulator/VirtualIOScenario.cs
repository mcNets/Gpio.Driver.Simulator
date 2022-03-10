using System.Collections.Generic;
using System.Device.Gpio;

namespace Gpio.Driver.Simulator
{
    public class VirtualIOScenario
    {
        private List<VirtualIOPin> _signals = new List<VirtualIOPin>();
        private readonly VirtualGpioDriver _driver;
        private GpioController _controller;

        private VirtualIOScenario(GpioController controller, VirtualGpioDriver driver)
        {
            _controller = controller;
            _driver = driver;
        }

        public static VirtualIOScenario Create(GpioController controller, VirtualGpioDriver driver)
        {
            return new VirtualIOScenario(controller, driver);
        }

        public List<VirtualIOPin> Signals => _signals;
        public GpioController Controller => _controller;
        public VirtualGpioDriver Driver => _driver;

        public VirtualIOScenario Add(string name, int pinNumber, VirtualPinType pinType, object? control=null)
        {
            _driver.ValidatePinNumber(pinNumber);

            _signals.Add(new VirtualIOPin()
            {
                Name = name,
                PinNumber = pinNumber,
                PinType = pinType,
                Control = control
            });
           return this;
        }
    }

    public class VirtualIOPin
    {
        public string Name { get; set; } = "";
        public VirtualPinType PinType { get; set; }
        public int PinNumber { get; set; }
        public object? Control { get; set; }
    }

    public enum VirtualPinType
    {
        Input,
        Output,
        Counter,
        ManualCounter
    }
}
