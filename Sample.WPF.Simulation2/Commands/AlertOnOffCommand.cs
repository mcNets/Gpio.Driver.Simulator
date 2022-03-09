using Sample.WPF.Simulation2.ViewModels;
using System;
using System.Windows.Input;

namespace Sample.WPF.Simulation2.Commands
{
    internal class AlertOnOffCommand : ICommand
    {
        private readonly MainWindowViewModel _viewModel;

        public AlertOnOffCommand(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            _viewModel.Alert = !_viewModel.Alert;
        }
    }
}
