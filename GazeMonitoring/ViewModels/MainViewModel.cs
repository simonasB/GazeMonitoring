﻿using GazeMonitoring.Base;
using GazeMonitoring.Messaging;
using GazeMonitoring.Messaging.Messages;
using System.Collections.Generic;
using System.Linq;

namespace GazeMonitoring.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private IMainSubViewModel _currentViewModel;
        private readonly Dictionary<EMainSubViewModel, IMainSubViewModel> _viewModels;
        private readonly IMessenger _messenger;

        public IMainSubViewModel CurrentViewModel
        {
            get => _currentViewModel;
            set => SetProperty(ref _currentViewModel, value);
        }

        public MainViewModel(IEnumerable<IMainSubViewModel> viewModels, IMessenger messenger)
        {
            _viewModels = viewModels.ToDictionary(o => o.SubViewModel);
            _messenger = messenger;
            CurrentViewModel = _viewModels[EMainSubViewModel.MainNavigationViewModel];
            SetupMessageRegistrations();
        }

        private void ShowView(EMainSubViewModel viewModel)
        {
            //_messenger.Send(new MainSubViewModelChangedMessage { SubViewModel = viewModel });
            CurrentViewModel = _viewModels[viewModel];
        }

        private void SetupMessageRegistrations()
        {
            _messenger.Register<ShowMainNavigationMessage>(_ => ShowView(EMainSubViewModel.MainNavigationViewModel));
            _messenger.Register<ShowProfilesMessage>(_ => ShowView(EMainSubViewModel.ProfilesViewModel));
            _messenger.Register<ShowStartNewSessionMessage>(_ => ShowView(EMainSubViewModel.SessionViewModel));
        }
    }
}
