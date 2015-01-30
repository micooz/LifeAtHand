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
using System.Runtime.InteropServices;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Life_at_hand
{
    /// <summary>
    /// pageWeather.xaml 的交互逻辑
    /// </summary>
    public partial class pageWeather : Page
    {
        public pageWeather()
        {
            InitializeComponent();
        }

        const string uPM25 = "http://www.cnpm25.cn/paiming.htm";

        private const string sConfigFile = "config.xml";
        private const string uProvince = "http://www.weather.com.cn/data/city3jdata/china.html";
        private const string uCity = "http://www.weather.com.cn/data/city3jdata/provshi/";//+10119.html";
        private const string uArea = "http://www.weather.com.cn/data/city3jdata/station/";//+10119 01.html
        private const string uWeather_3 = "http://m.weather.com.cn/data/";//+10119 01 01.html";
        private const string uWeather_2 = "http://www.weather.com.cn/data/cityinfo/";//+10119 01 01.html";
        private const string uWeather_1 = "http://www.weather.com.cn/data/sk/";//+10119 01 01.html";
        //全局配置信息
        public Config m_config = new Config();

        //保存省市区代码
        Config.pca m_pca = new Config.pca();

        Dictionary<string, string> m_pros;
        Dictionary<string, string> m_cites;
        Dictionary<string, string> m_areas;

        //获取天气数据
        private void GetWeather(string code)
        {
            if (code == null) return;
            string data = string.Empty;
            char[] tr = { '}' };

            Common.getJson(uWeather_1 + code + ".html", ref data);

            if (data.StartsWith("<!DOCTYPE"))
            {
                MessageBox.Show("没有该地区的天气数据！");
            }
            else
            {
                data = data.Remove(0, 15);
                data = data.TrimEnd(tr);
                data = data + "}";
                m_weather = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                updateInfo();
                //获取预报信息
                Common.getJson(uWeather_3 + code + ".html", ref data);
                data = data.Remove(0, 15);
                data = data.TrimEnd(tr);
                data = data + "}";
                m_forcast = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                updateInfo(1);
            }

        }

        //缓存省市区数据
        private void getProvinceList()
        {
            string _pro = string.Empty;
            Common.getJson(uProvince, ref _pro);
            //省份
            m_pros = JsonConvert.DeserializeObject<Dictionary<string, string>>(_pro);
            provinceList.ItemsSource = (m_pros.Values);
        }

        //关闭弹出层
        private void btnClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //save
            using (FileStream fs = new FileStream(sConfigFile, FileMode.Create))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Config));
                xs.Serialize(fs, m_config);
            }
            SettingLayer.Visibility = Visibility.Hidden;
            popMsg("配置已保存");
        }

        //选择省份，获取城市列表
        private void provinceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = provinceList.SelectedIndex;
            if (i < 0) return;
            string _jCities = string.Empty;
            m_pca.pid = m_pros.Keys.ToArray<string>()[i];
            m_config.pName = provinceList.SelectedValue.ToString();

            Common.getJson(uCity + m_pca.pid + ".html", ref _jCities);
            m_cites = JsonConvert.DeserializeObject<Dictionary<string, string>>(_jCities);
            cityList.ItemsSource = m_cites.Values;
        }

        //选择城市，获取区域列表
        private void cityList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = cityList.SelectedIndex;
            if (i < 0) return;

            string _jAreas = string.Empty;

            m_pca.cid = m_cites.Keys.ToArray<string>()[i];
            m_config.cName = cityList.SelectedValue.ToString();

            Common.getJson(uArea + m_pca.pid + m_pca.cid + ".html", ref _jAreas);
            m_areas = JsonConvert.DeserializeObject<Dictionary<string, string>>(_jAreas);
            areaList.ItemsSource = m_areas.Values;
        }

        //选择区域
        private void areaList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int i = areaList.SelectedIndex;
            if (i < 0) return;
            m_pca.aid = m_areas.Keys.ToArray<string>()[i];
            m_config.aName = areaList.SelectedValue.ToString();
            m_config.m_pca = m_pca;
            m_config.lastCode = m_pca.combine();

            GetWeather(m_config.lastCode);

        }

        public Dictionary<string, string> m_weather;
        public Dictionary<string, string> m_forcast;

        //消息弹窗
        void popMsg(string msg, int msec = 1500)
        {
            popMsg obj = new popMsg();
            GridMain.Children.Add(obj);
            obj.show(msg, msec);
        }

        //该函数需要优化
        public void updateInfo(int type = 0)
        {
            if (type == 0)
            {
                //实时天气
                if (m_weather == null || m_weather["temp"] == "暂无实况")
                {
                    popMsg("本区暂无实况");
                    return;
                }

                titleName.Content = m_weather["city"];
                temp.Content = m_weather["temp"] + "℃";
                windsd.Content = m_weather["WD"] + m_weather["WS"] + " 湿度" + m_weather["SD"];
                time.Content = string.Format("{0}月{1}日 {2}", DateTime.Now.Month, DateTime.Now.Day, m_weather["time"]);
            }
            else
            {
                //预报天气
                //string[] days = { "今天", "周一", "周二", "周三", "周四", "周五", "周六", "周日" };
                var alc = new byte[,] { 
                    { 84, 27 ,134},
                    {186,151,197},
                    {166,207,229},
                    {12,180,234},
                    {230,28,100}
                };

                forecastList.clear();
                for (int i = 1; i <= 5; ++i)
                {
                    DropDown.ListItem it = new DropDown.ListItem();
                    it.w = "未来" + i + "天";
                    it.dur = m_forcast["temp" + (i + 1)];
                    it.imgsrc = m_forcast["img" + ((i + 1) * 2)];
                    it.brush = new SolidColorBrush(Color.FromRgb(alc[i - 1, 0], alc[i - 1, 1], alc[i - 1, 2]));

                    if (it.imgsrc == "99")
                        it.imgsrc = m_forcast["img" + ((i + 1) * 2 - 1)];

                    forecastList.addItem(it);
                }
                lblWeather.Content = m_forcast["weather1"];
                lblTips.Content = "穿衣指数：" + m_forcast["index_d"];
                lbl_1.Content = "紫外线：" + m_forcast["index_uv"];
                lbl_2.Content = "洗车：" + m_forcast["index_xc"];
                lbl_3.Content = "旅游：" + m_forcast["index_tr"];
                lbl_4.Content = "舒适指数：" + m_forcast["index_co"];

                mainImg.Source = new BitmapImage(new Uri(string.Format(@"res\wth\{0}.png", m_forcast["img1"]), UriKind.Relative));
            }
        }

        string getPM25()
        {
            string htmldata;
            HttpWebRequest req = WebRequest.Create(uPM25) as HttpWebRequest;
            using (HttpWebResponse resp = req.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(resp.GetResponseStream(), Encoding.UTF8))
                {
                    htmldata = reader.ReadToEnd();
                }
            }
            return null;

        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            updateInfo();
            updateInfo(1);

            Image im = (sender as Image);

            RotateTransform rt = new RotateTransform();
            rt.CenterX = im.Width / 2;
            rt.CenterY = im.Height / 2;
            popMsg("刷新天气信息中", 500);

            for (double i = 1; i <= 360; i += 5)
            {
                rt.Angle = i;
                im.RenderTransform = rt;
                System.Threading.Thread.Sleep(8);
                DispatcherHelper.DoEvents();
            }

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SettingLayer.Visibility = Visibility.Hidden;

            getProvinceList();

            if (File.Exists(sConfigFile))
            {
                using (FileStream fs = new FileStream(sConfigFile, FileMode.Open))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(Config));
                    m_config = (Config)(xml.Deserialize(fs));
                }
            }
            else
            {
                m_pca.pid = "10127";
                m_pca.cid = "01";
                m_pca.aid = "01";
                m_config.m_pca = m_pca;
                m_config.lastCode = m_pca.combine();
                m_config.pName = "四川";
                m_config.cName = "成都";
                m_config.aName = "成都";
            }

            provinceList.Text = m_config.pName;
            cityList.Text = m_config.cName;
            areaList.Text = m_config.aName;

            GetWeather(m_config.lastCode);

        }

        private void btnSet_MouseDown(object sender, MouseButtonEventArgs e)
        {
            SettingLayer.Visibility = Visibility.Visible;
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

        }

    }
}
