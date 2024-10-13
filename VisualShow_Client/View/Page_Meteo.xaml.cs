using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using MQTTnet.Protocol;
using MQTTnet.Server;
using VisualShow_Client.Controller;
using WeatherView.Service;

namespace VisualShow_Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Meteo.xaml
    /// </summary>
    public partial class Page_Meteo : UserControl
    {
       

        //variables dans lesquelles ont stocker les valeurs obtenues avec mqtt
        

        int seconds;

        DispatcherTimer timer;
        DAO_MQTT dao_mqtt;
        public Page_Meteo()
        {
            InitializeComponent();
            dao_mqtt = new DAO_MQTT();
            timer = new DispatcherTimer();
            UpdateUI();
            Initialize_Timer();
        }
        public void Initialize_Timer()
        {
            timer.Tick += new EventHandler(Timer_Tick);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }
        public void Timer_Tick(object sender, EventArgs e)
        {
            seconds++;
            Debug.WriteLine(seconds);
            if (seconds == 10)
            {
                seconds = 0;
                UpdateUI();
                Debug.WriteLine("Updating MQTT data");

            }
        }

        public void UpdateUI()
        {
           List<string> mqttData = dao_mqtt.GetData();
            MessageBox.Show(mqttData[1]);
           if (mqttData.Count == 0)
           {
                MessageBox.Show("nil");
                return;
           }
            TB_Humidity.Text = mqttData[0];
            TB_Temperature.Text = mqttData[1];
            TB_Decibels.Text = mqttData[2];
            TB_AirQuality.Text = mqttData[3];
            if (mqttData[4] != "")
            {
                Popup_Emergency.IsOpen = true;
                TB_Emergency.Text = mqttData[4];
            }

            MessageBox.Show("worked");
        }
        public void Menu_Click(object sender, RoutedEventArgs e) { }
    }
}

