using System;
using System.Windows;
using System.Windows.Input;

namespace WPFWidget
{
    public partial class MainWindow : Window
    {
        private PopupWindow popupWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Position the widget at the top right and make it persistent
            this.Left = SystemParameters.WorkArea.Width - this.Width - 10;
            this.Top = 10;
        }

        private void WidgetBorder_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (popupWindow == null || !popupWindow.IsVisible)
            {
                ShowPopup();
            }
            else
            {
                popupWindow.Close();
                popupWindow = null;
            }
        }

        private void ShowPopup()
        {
            double screenHeight = SystemParameters.WorkArea.Height;
            double screenWidth = SystemParameters.WorkArea.Width;

            double widgetHeight = this.Height + 10; // 10px padding below the widget

            popupWindow = new PopupWindow
            {
                Width = screenWidth * 0.5,             // 50% of screen width
                Height = screenHeight - widgetHeight,  // Slightly less than full height
                Left = screenWidth * 0.5,              // Stick to the right
                Top = widgetHeight,                    // Just below the widget
                Topmost = true
            };

            popupWindow.Show();
        }
    }
}
