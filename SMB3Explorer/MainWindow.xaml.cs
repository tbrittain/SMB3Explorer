using System;
using System.Windows;
using SMB3Explorer.ViewModels;

namespace SMB3Explorer;

public partial class MainWindow
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();

        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            MessageBox.Show("This application is only supported on Windows.");
            Environment.Exit(1);
        }
    }
}