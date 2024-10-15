using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
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
using WeatherView.Service;

namespace VisualShow_Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Accueil.xaml
    /// </summary>
    public partial class Page_Accueil : UserControl
    {
        int seconds = 0;
        DispatcherTimer timer;
        APIManager apimanager;


        public Page_Accueil()
        {
            InitializeComponent();
            apimanager = new APIManager();
            
            UpdateUI("Annecy");
            timer = new DispatcherTimer();
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
            if (seconds % 2 == 0)
            {
                TB_CurrentTime.Text = DateTime.Now.ToString("HH:mm");
            }
            if (seconds >= 60)
            {
                seconds = 0;
                UpdateUI("Annecy");
                Debug.WriteLine("Resetting to 0");
            }
        }
        private async void UpdateUI(string cityUpdate)
        {

            Image[] weatherImages = { Image_1, Image_2, Image_3, Image_4, Image_5, Image_6 };
            TextBlock[] hourTexts = { Heure_1, Heure_2, Heure_3, Heure_4, Heure_5, Heure_6 };
            TextBlock[] tempTexts = { Temp_1, Temp_2, Temp_3, Temp_4, Temp_5, Temp_6 };

            try
            {
                Root root = await apimanager.DataGrabber(cityUpdate);

                CurrentCondition currentcondition = root.current_condition;
                FcstDay0 TodayForecast = root.fcst_day_0;

                HourlyData TodayForecasT = root.fcst_day_1.hourly_data;

                DateTime now = DateTime.Now;

                Uri weatherNow = new Uri(currentcondition.icon_big, UriKind.Absolute);


                TB_CurrentTime.Text = now.ToString("HH:mm");
                TB_MainTemp.Text = currentcondition.tmp.ToString() + "°C";
                Today_Image.Source = new BitmapImage(weatherNow);
                TB_TodayDate.Text = now.ToString("dd MMMM yyyy");

                string dayInFrench = now.ToString("dddd", new CultureInfo("fr-FR"));
                string capitalizedDay = char.ToUpper(dayInFrench[0]) + dayInFrench.Substring(1);

                TB_TodayDay.Text = capitalizedDay;

                for (int i = 0; i < 6; i++)
                {
                    string hourKey;
                    string displayTime;

                    // Get the hour for the current time + i hours
                    int currentHour = now.AddHours(i + 1).Hour;

                    // Construct the hourKey for property access (XH00 or XXH00)
                    hourKey = $"{currentHour}H00"; // No need for leading zero logic since format is consistent

                    // Construct display time (H:00 for single digits, HH:00 for double digits)
                    displayTime = $"{currentHour}:00"; // This works for both single and double digits

                    // Try to get the property from the HourlyData class using the hourKey
                    PropertyInfo propInfo = typeof(HourlyData).GetProperty($"_{hourKey}");

                    if (propInfo != null)
                    {
                        // Get the hourly data for the current hour
                        var hourlyData = propInfo.GetValue(TodayForecast.hourly_data);

                        if (hourlyData != null)
                        {
                            // Retrieve the ICON and TMP2m properties
                            var iconProperty = hourlyData.GetType().GetProperty("ICON");
                            var tmpProperty = hourlyData.GetType().GetProperty("TMP2m");

                            if (iconProperty != null && tmpProperty != null)
                            {
                                // Get the icon and temperature values
                                string iconValue = (string)iconProperty.GetValue(hourlyData);
                                string tmpValue = (string)tmpProperty.GetValue(hourlyData);

                                // Modify the iconValue to get the big version
                                string bigIconValue = iconValue.Replace(".png", "-big.png");

                                // Set the text for the display
                                hourTexts[i].Text = displayTime; // Use the displayTime format
                                tempTexts[i].Text = Math.Round(double.Parse(tmpValue)).ToString() + "°C";
                                weatherImages[i].Source = new BitmapImage(new Uri(bigIconValue, UriKind.RelativeOrAbsolute));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {            }
        }
    }
}
