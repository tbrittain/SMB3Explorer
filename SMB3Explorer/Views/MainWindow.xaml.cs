using System;
using System.Windows;
using System.Windows.Media;
using FontAwesome.WPF;

namespace SMB3Explorer.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        Icon = ImageAwesome.CreateImageSource(FontAwesomeIcon.Database, Brushes.Black);
        
        if (Environment.OSVersion.Platform != PlatformID.Win32NT)
        {
            MessageBox.Show("This application is only supported on Windows.");
            Environment.Exit(1);
        }
    }
}