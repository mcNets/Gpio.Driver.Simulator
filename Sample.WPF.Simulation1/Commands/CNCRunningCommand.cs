using Sample.WPF.Simulation1.ViewModels;
using System;
using System.Windows.Input;

namespace Sample.WPF.Simulation1.Commands
{
    internal class CNCRunningCommand : ICommand
    {
        private readonly SimulationViewModel _viewModel;

        public CNCRunningCommand(SimulationViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter) => true;

        public void Execute(object? parameter)
        {
            _viewModel.CNCProgram = ! _viewModel.CNCProgram;
        }
    }
}
