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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Life_at_hand
{
    /// <summary>
    /// popMsg.xaml 的交互逻辑
    /// </summary>
    public partial class popMsg : UserControl
    {
        public popMsg()
        {
            InitializeComponent();
            this.Name = "popMsg";
            this.Opacity = 0;
            this.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            this.VerticalAlignment = System.Windows.VerticalAlignment.Bottom;
            this.Margin = new Thickness(0, 0, 0, 20);
            NameScope.SetNameScope(this, new NameScope());
            RegisterName(this.Name, this);
        }

        public void show(string msg, int dur)
        {
            txtMsg.Content = msg;
            //
            DoubleAnimation animation = new DoubleAnimation();
            animation.From = 100;
            animation.To = 0;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(dur));
            Storyboard.SetTargetName(animation, this.Name);
            Storyboard.SetTargetProperty(animation, new PropertyPath(OpacityProperty));
            Storyboard sb = new Storyboard();
            sb.Children.Add(animation);
            sb.Begin(this,true);
            sb.Completed += new EventHandler(showFinished);                      
        }

        void showFinished(object sender, EventArgs e)
        {
            (sender as Storyboard).Remove(this);

            //(Parent as Grid).Children.Remove(this);
        }

    }
}
