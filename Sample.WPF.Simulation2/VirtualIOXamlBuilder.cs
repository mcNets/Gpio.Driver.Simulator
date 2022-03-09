using Gpio.Driver.Simulator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sample.WPF.Simulation2
{
    internal class VirtualIOXamlBuilder
    {
        private readonly VirtualIOSignals _signals;

        public VirtualIOXamlBuilder(VirtualIOSignals signals)
        {
            _signals = signals;
        }

        public StackPanel BuildStackPanel(Orientation orientation)
        {
            var sp = new StackPanel() { Orientation= orientation };

            foreach (var signal in _signals.Signals)
            {
                if (signal.SignalType == "input")
                {
                    ToggleButton tb = new ToggleButton();


                    sp.Children.Add(tb);
                }
            }
            return sp;
        }
    }
}
