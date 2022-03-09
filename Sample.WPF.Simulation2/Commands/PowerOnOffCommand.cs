using System;
using System.Windows.Input;

namespace Sample.WPF.Simulation2.Commands
{
    internal class PowerOnOffCommand : ICommand
    {
        private readonly SimulationViewModel _viewModel;

        public PowerOnOffCommand(SimulationViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            _viewModel.Power = !_viewModel.Power;
        }
    }
}
