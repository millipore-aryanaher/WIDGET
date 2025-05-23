using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFWidget
{
    public partial class MainWindow : Window
    {
        private PopupWindow? popupWindow;


        public MainWindow()
        {
            InitializeComponent();
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
        private void ShowPopup(string filename = null)
        {
            double screenHeight = SystemParameters.WorkArea.Height;
            double screenWidth = SystemParameters.WorkArea.Width;

            double widgetHeight = this.Height + 10;

            popupWindow = new PopupWindow
            {
                Width = screenWidth * 0.5,
                Height = screenHeight - widgetHeight,
                Left = screenWidth * 0.5,
                Top = widgetHeight,
                Topmost = true
            };

            popupWindow.Show();

            if (!string.IsNullOrEmpty(filename))
            {
                popupWindow.SearchBox.Text = filename;
                popupWindow.SearchButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }




        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Left = SystemParameters.WorkArea.Width - this.Width - 10;
            this.Top = 10;

            // Check for command-line argument passed via Tag
            if (this.Tag is string passedFilename && !string.IsNullOrWhiteSpace(passedFilename))
            {
                // Auto-launch popup with filename
                ShowPopup(passedFilename);
            }
        }



    }
}
