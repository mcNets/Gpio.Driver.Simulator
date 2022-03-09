using Gpio.Driver.Simulator;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace Sample.WPF.Simulation2
{
    public class VirtualIOSimulatorControls
    {
        private readonly VirtualIOSignals _signals;

        public VirtualIOSimulatorControls(VirtualIOSignals signals)
        {
            _signals = signals;
        }

        public Window BuildWindow()
        {
            var wnd = new Window() { Width=200.0 };
            wnd.Content = WindowContent();
            wnd.Show();
            return wnd;
        }

        /// <summary>
        /// Builds the content of the window
        /// </summary>
        /// <param name="orientation"></param>
        /// <returns></returns>
        public UIElement WindowContent()
        {
            ScrollViewer scrollViewer = new ScrollViewer()
            {
                VerticalScrollBarVisibility = ScrollBarVisibility.Auto
            };

            StackPanel stackPanel = new StackPanel() 
            { 
                Orientation = Orientation.Vertical, 
                Margin=new Thickness(20) 
            };

            foreach (var signal in _signals.Signals)
            {
                if (signal.SignalType.ToLower() == "input")
                {
                    stackPanel.Children.Add(InputSignalControl(signal));
                }
                else if (signal.SignalType.ToLower() == "counter")
                {
                    stackPanel.Children.Add(CounterControl(signal));
                }
            }

            scrollViewer.Content = stackPanel;

            return scrollViewer;
        }

        private UIElement InputSignalControl(VirtualIOSignal signal)
        {
            var bt = new ToggleButton()
            {
                Tag = signal,
                Content = signal.Name,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 15),
            };

            // Puts the signal Hight
            bt.Checked += (s, e) =>
            {
                var signal = ((ToggleButton)s).Tag as VirtualIOSignal;
                ((ToggleButton)s).Content = $"{signal?.Name} ON";
            };

            // Puts the signal Low
            bt.Unchecked += (s, e) =>
            {
                var signal = ((ToggleButton)s).Tag as VirtualIOSignal;
                ((ToggleButton)s).Content = signal?.Name;
            };

            return bt;
        }

        private UIElement CounterControl(VirtualIOSignal signal)
        {
            return new TextBlock()
            {
                Text = $"Counter: {signal.Name}",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 15)
            };
        }
    }
}
