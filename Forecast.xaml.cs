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
    /// DropDown.xaml 的交互逻辑
    /// </summary>
    public partial class DropDown : UserControl
    {
        //Dictionary<string, string> imageMap = new Dictionary<string, string>();
        
        public struct ListItem
        {
            public string w;
            public string dur;
            public string imgsrc;
            public SolidColorBrush brush;
        };

        public DropDown()
        {
            InitializeComponent();
            PanelMain.Children.Clear();
        }

        public void addItem(ListItem it)
        {
            forecastItem obj = new forecastItem();
            obj.Margin = new Thickness(0, 5, 0, 0);
            obj.init(it);
            
            PanelMain.Children.Add(obj);
        }

        public void clear()
        {
            PanelMain.Children.Clear();
        }

    }
}
