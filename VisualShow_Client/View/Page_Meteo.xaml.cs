using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using VisualShow_Client.Controller;
using WeatherView.Service;

namespace VisualShow_Client.View
{

    /// <summary>
    /// Logique d'interaction pour Page_Meteo.xaml
    /// </summary>
    public partial class Page_Meteo : UserControl
    {
       public string emergencyMQTTMessage = "";

        //variables dans lesquelles ont stocker les valeurs obtenues avec mqtt
        DispatcherTimer timer;
        DAO_MQTT dao_mqtt;
        private bool isPopupCooldownActive;
        int seconds = 0;

        public Page_Meteo()
        {
            InitializeComponent();
            dao_mqtt = new DAO_MQTT();
            timer = new DispatcherTimer();
            Initialize_Timer();
            InitializeMQTT();
            UpdateUI();
            
        }

        public async void InitializeMQTT()
        {
            await dao_mqtt.InitializeAsync();
        }

        public void Initialize_Timer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            
            UpdateUI();
            seconds++;
            if (seconds == 30)
            {
                seconds = 0;
                emergencyMQTTMessage = "";
            }
        }

        public void UpdateUI()
        {
            List<string> mqttData = new List<string>();
            mqttData = dao_mqtt.GetData();

            if (mqttData.Count == 0)
            {
                return;
            }

            TB_HumidityValue.Text = mqttData[0] + "%";
            TB_TemperatureValue.Text = mqttData[1] + "°C";
            TB_DecibelsValue.Text = mqttData[2] + " dB";

            string airQualityValueStr = mqttData[3];
            if (mqttData[4] != "")
            {
                emergencyMQTTMessage = mqttData[4];
            }

            TB_AirQualityValue.Text = airQualityValueStr + JudgeAirQuality(airQualityValueStr);
            CheckAirQuality(airQualityValueStr);
        }

        private void CheckAirQuality(string airQuality)
        {
            if (double.TryParse(airQuality, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double airQualityValue))
            {
                if (airQualityValue > 45 && !isPopupCooldownActive)
                {
                    ShowWarningPopup("Faite attention, la qualité d'air est basse et c'est fortement recommandé de ouvrir les fenêtres");
                    isPopupCooldownActive = true;
                }
            }
            else
            {
                return;
            }
        }

        private void ShowWarningPopup(string message)
        {
            TB_Emergency.Text = message;

            Popup_Emergency.PlacementTarget = Application.Current.MainWindow;
            Popup_Emergency.IsOpen = true;

            DispatcherTimer durationTimer = new DispatcherTimer();
            durationTimer.Interval = TimeSpan.FromSeconds(10);
            durationTimer.Tick += (s, e) =>
            {
                Popup_Emergency.IsOpen = false;
                durationTimer.Stop();

                DispatcherTimer cooldownTimer = new DispatcherTimer();
                cooldownTimer.Interval = TimeSpan.FromSeconds(30);
                cooldownTimer.Tick += (s2, e2) =>
                {
                    isPopupCooldownActive = false;
                    cooldownTimer.Stop();
                };
                cooldownTimer.Start();
            };
            durationTimer.Start();
        }

        public string JudgeAirQuality(string airQuality)
        {
            if (double.TryParse(airQuality, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double airQualityValue))
            {
                if (airQualityValue <= 12) // Very Good
                    return " (Very Good)";
                else if (airQualityValue <= 25) // Good
                    return " (Good)";
                else if (airQualityValue <= 50) // Moderate
                    return " (Moderate)";
                else if (airQualityValue <= 75) // Bad
                    return " (Bad)";
                else // Very Bad
                    return " (Very Bad)";
            }
            else
            {
                // Handle the case where parsing fails
                return " (Invalid Data)";
            }
        }

        public void Menu_Click(object sender, RoutedEventArgs e) { }
    }
}
