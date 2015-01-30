using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Interop;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Life_at_hand
{

    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            if (!Common.Ping("115.239.210.26"))
            {
                MessageBox.Show("请先连接互联网络！");
                Process.GetCurrentProcess().Kill();
            }
        }


        private void navWeather_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mainFrame.Content as string != "pageWeather.xaml")
                mainFrame.Navigate(new Uri("pageWeather.xaml", UriKind.Relative));
        }

        private void navTrans_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mainFrame.Content as string != "pageTrans.xaml")
                mainFrame.Navigate(new Uri("pageTrans.xaml", UriKind.Relative));

        }

        private void navMap_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Process.Start("http://map.baidu.com/");
        }

        private void btnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void btnSet_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void btnAbout_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (mainFrame.Content as string != "pageAbout.xaml")
                mainFrame.Navigate(new Uri("pageAbout.xaml", UriKind.Relative));
        }




    }


}
