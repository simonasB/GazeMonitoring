﻿using System;
using System.Linq;
using System.Windows;
using Autofac;
using GazeMonitoring.Model;
using Hardcodet.Wpf.TaskbarNotification;

namespace GazeMonitoring {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow(IContainer container, TaskbarIcon notifyIcon) {
            this.DataContext = new MainViewModel(container, notifyIcon);
            InitializeComponent();
            CmbDataStreams.ItemsSource = Enum.GetValues(typeof(DataStream)).Cast<DataStream>();
        }

        private void Window_Deactivated(object sender, EventArgs e) {
            Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Right - this.Width;
            this.Top = desktopWorkingArea.Bottom - this.Height;
        }

        private void BtnStop_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CmbDataStreams_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}
