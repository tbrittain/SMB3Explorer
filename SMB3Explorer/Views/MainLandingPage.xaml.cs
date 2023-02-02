using System;
using System.Windows;
using SMB3Explorer.ViewModels;

namespace SMB3Explorer.Views;

public partial class MainLandingPage
{
    public MainLandingPage(MainLandingViewModel landingViewModel)
    {
        DataContext = landingViewModel;
        InitializeComponent();

        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            MessageBox.Show("This application is only supported on Windows.");
            Environment.Exit(1);
        }
    }
}