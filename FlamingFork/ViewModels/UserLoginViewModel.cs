﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FlamingFork.Pages;

namespace FlamingFork.ViewModels
{
    public partial class UserLoginViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _Email;

        [ObservableProperty]
        private string _Password;

        [ObservableProperty]
        private string _EmailError;

        [ObservableProperty]
        private string _PasswordError;

        private INavigation _Navigation;

        public UserLoginViewModel(INavigation navigation)
        {
            _Navigation = navigation;
        }

        [RelayCommand]
        public void ValidateEmail()
        {
        }

        [RelayCommand]
        public void ValidatePassword()
        {
        }

        [RelayCommand]
        public void ValidateForm()
        {
        }

        [RelayCommand]
        public async Task CreateAnAccount()
        {
            await _Navigation.PushModalAsync(new UserRegistrationPage());
        }
    }
}