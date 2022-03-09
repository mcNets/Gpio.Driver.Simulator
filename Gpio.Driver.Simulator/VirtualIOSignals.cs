using System.Collections.Generic;

namespace Gpio.Driver.Simulator
{
    public class VirtualIOSignals
    {
        private List<VirtualIOSignal> _signals = new List<VirtualIOSignal>();

        public VirtualIOSignals()
        {
        }

        public VirtualIOSignals(List<VirtualIOSignal> signals)
        {
            _signals = signals;
        }

        public List<VirtualIOSignal> Signals 
        { 
            get => _signals;
        }

        public void Add(string name, string pinNumber, string signalType)
        {
            _signals.Add(new VirtualIOSignal()
            {
                Name = name,
                PinNumber = pinNumber,
                SignalType = signalType
            });
        }
    }

    public class VirtualIOSignal
    {
        public string Name { get; set; } = "";
        public string PinNumber { get; set; } = "";
        public string SignalType { get; set; } = "";
    }
}
