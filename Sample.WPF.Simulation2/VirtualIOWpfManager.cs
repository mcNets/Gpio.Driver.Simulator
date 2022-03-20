using Gpio.Driver.Simulator;
using Sample.WPF.Simulation2.Services;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace Sample.WPF.Simulation2
{
    public class VirtualIOWpfManager
    {
        private readonly VirtualIOScenario _scenario;
        private readonly DispatcherTimer _timer;

        private IEnumerable<VirtualIOPin> _counters;

        public VirtualIOWpfManager(VirtualIOScenario scenario)
        {
            _scenario = scenario;
            _counters = new List<VirtualIOPin>();

            _timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 3),
                IsEnabled = false
            };

            _timer.Tick += async (sender, e) =>
            {
                foreach (var counter in _counters!)
                {
                    (counter.Control as TextBlock)!.Text = $"{counter?.Name} ON";

                    await Task.Factory.StartNew(() =>
                    {
                        _scenario.Driver.WriteInPin(_scenario.Controller, counter!.PinNumber, PinValue.High);
                        Thread.Sleep(300);
                        _scenario.Driver.WriteInPin(_scenario.Controller, counter!.PinNumber, PinValue.Low);
                    });

                    (counter!.Control as TextBlock)!.Text = $"{counter?.Name}";
                }
            };
        }

        public Window Run()
        {
            var wnd = new Window() { Width=200.0 };
            wnd.Content = WindowContent();
            wnd.Show();

            // Save a list with all counters for further use in the dispatch timer tick event
            _counters = _scenario.Signals.Where(x => x.PinType == VirtualPinType.Counter);

            _timer.Start();

            return wnd;
        }

        /// <summary>
        /// Builds the content of the window
        /// </summary>
        /// <returns>Returns an instance of a <see cref="UIElement"/></returns>
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

            foreach (var signal in _scenario.Signals)
            {
                if (signal.PinType == VirtualPinType.Input )
                {
                    stackPanel.Children.Add(InputSignalControl(signal));
                }
                else if (signal.PinType == VirtualPinType.Counter)
                {
                    stackPanel.Children.Add(CounterControl(signal));
                }
                else if (signal.PinType == VirtualPinType.ManualCounter)
                {
                    stackPanel.Children.Add(ManualCounterControl(signal));
                }
                else if (signal.PinType == VirtualPinType.Output)
                {
                    stackPanel.Children.Add(OutputControl(signal));
                }
            }

            scrollViewer.Content = stackPanel;

            return scrollViewer;
        }

        /// <summary>
        /// A input signal is managed by a ToggleButton
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private UIElement InputSignalControl(VirtualIOPin signal)
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
                var signal = ((ToggleButton)s).Tag as VirtualIOPin;
                ((ToggleButton)s).Content = $"{signal?.Name} ON";

                _scenario.Driver.WriteInPin(_scenario.Controller, signal!.PinNumber, PinValue.High);
            };

            // Puts the signal Low
            bt.Unchecked += (s, e) =>
            {
                var signal = ((ToggleButton)s).Tag as VirtualIOPin;
                ((ToggleButton)s).Content = signal?.Name;

                _scenario.Driver.WriteInPin(_scenario.Controller, signal!.PinNumber, PinValue.Low);
            };

            signal.Control = bt;

            return bt;
        }

        /// <summary>
        /// A Counter signal is managed by the DispatchTimer and visualized as a TextBlock
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private UIElement CounterControl(VirtualIOPin signal)
        {
            var tb = new TextBlock()
            {
                Text = $"Counter: {signal.Name}",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 15)
            };

            signal.Control = tb;

            return tb;
        }

        /// <summary>
        /// A manual counter is managed by a Button
        /// </summary>
        /// <param name="signal"></param>
        /// <returns></returns>
        private UIElement ManualCounterControl(VirtualIOPin signal)
        {
            var bt = new Button()
            {
                Tag = signal,
                Content = signal.Name,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 15),
            };

            // Puts the signal Hight for 300 ms
            bt.Click += async (s, e) =>
            {
                var signal = ((Button)s).Tag as VirtualIOPin;
                ((Button)s).Content = $"{signal?.Name} ON";

                await Task.Factory.StartNew(() =>
                {
                    _scenario.Driver.WriteInPin(_scenario.Controller, signal!.PinNumber, PinValue.High);
                    Thread.Sleep(300);
                    _scenario.Driver.WriteInPin(_scenario.Controller, signal!.PinNumber, PinValue.Low);
                });

                ((Button)s).Content = $"{signal?.Name}";
            };

            signal.Control = bt;

            return bt;
        }

        private UIElement OutputControl(VirtualIOPin signal)
        {
            var tb = new TextBlock()
            {
                Text = $"{signal.Name}",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Padding = new Thickness(10),
                Margin = new Thickness(0, 0, 0, 15),
                Foreground = Brushes.White,
                Background = Brushes.Navy,
            };

            _scenario.Controller.RegisterCallbackForPinValueChangedEvent(signal.PinNumber, PinEventTypes.Rising | PinEventTypes.Falling, (s, e) =>
            {
                foreach (var signal in _scenario.Signals)
                {
                    if (signal.PinNumber == e.PinNumber)
                    {
                        if (e.ChangeType == PinEventTypes.Rising)
                        {
                            (signal.Control as TextBlock)!.Text = $"{signal.Name} ON";
                            (signal.Control as TextBlock)!.Background = Brushes.DarkRed;
                        }
                        else
                        {
                            (signal.Control as TextBlock)!.Text = signal.Name;
                            (signal.Control as TextBlock)!.Background = Brushes.Navy;
                        }
                    }
                }
            });

            signal.Control = tb;

            return tb;
        }
    }
}
