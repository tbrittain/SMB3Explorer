using System;
using System.Windows;

namespace SMB3Explorer.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            MessageBox.Show("This application is only supported on Windows.");
            Environment.Exit(1);
        }
    }
}