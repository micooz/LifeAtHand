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

namespace Life_at_hand
{
    /// <summary>
    /// forecastItem.xaml 的交互逻辑
    /// </summary>
    public partial class forecastItem : UserControl
    {
        public forecastItem()
        {
            InitializeComponent();
        }

        public void init(DropDown.ListItem it)
        {
            lbl1.Content = it.w;
            lbl2.Content = it.dur;
            BitmapImage image = new BitmapImage(new Uri(string.Format(@"res\wth\{0}.png", it.imgsrc), UriKind.Relative));
            img.Source = image;
            rect.Fill = it.brush;
            rect.Opacity = 0.60;
        }

    }
}
