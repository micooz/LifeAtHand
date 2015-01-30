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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace Life_at_hand
{
    /// <summary>
    /// pageMap.xaml 的交互逻辑
    /// </summary>
    public partial class pageMap : Page
    {
        const string uTarget = "http://openapi.baidu.com/public/2.0/bmt/translate?client_id=ouC0PCpwVrtcvIeKju0iSuGA&from=auto&to=zh&q=";

        public pageMap()
        {
            InitializeComponent();
        }
        //消息弹窗
        public void popMsg(string msg, int msec = 1500)
        {
            popMsg obj = new popMsg();
            GridMain.Children.Add(obj);
            obj.show(msg, msec);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            txtFrom.Clear();
            txtResult.Text = string.Empty;
            txtResult.Focus();
        }

        void translate()
        {
            if (txtFrom.Text == string.Empty)
            {
                popMsg("先写你要翻译的内容吧！");
                txtFrom.Focus();
                return;
            }
            popMsg("翻译中");

            string orgData = string.Empty;
            Common.getJson(uTarget + txtFrom.Text, ref orgData);

            //var json = JsonConvert.DeserializeObject<Dictionary<string,string>>(orgData);
            JObject jo = JObject.Parse(orgData);
            string[] values = jo.Properties().Select(item => item.Value.ToString()).ToArray();
            // values = JObject.Parse(values[2]).Properties().Select(item => item.Value.ToString()).ToArray();
            char[] a = { '[', ']' };
            var json = JsonConvert.DeserializeObject<Dictionary<string, string>>(values[2].Trim(a));


            txtResult.Text = json["dst"];
            txtFrom.Focus();
            txtFrom.SelectAll();

        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            translate();
        }

        private void txtFrom_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                translate();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(txtResult.Text);
            popMsg("已复制");

        }

    }
}
