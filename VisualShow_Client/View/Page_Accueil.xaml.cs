using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
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
            UpdateUI("Annecy");
            timer = new DispatcherTimer();
            apimanager = new APIManager();
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
            if (seconds >= 5)
            {
                seconds = 0;
                UpdateUI("Annecy");
                Debug.WriteLine("Resetting to 0");
            }
        }
        private async void UpdateUI(string cityUpdate)
        {
            try
            {
                Root root = await apimanager.DataGrabber(cityUpdate);

                CurrentCondition currentcondition = root.current_condition;
                FcstDay0 TodayForecast = root.fcst_day_0;

                HourlyData TodayForecasT = root.fcst_day_1.hourly_data;


                Uri weatherNow = new Uri(currentcondition.icon, UriKind.Absolute);

                TB_CurrentTime.Text = DateTime.Now.ToString("HH:mm");
                TB_MainTemp.Text = currentcondition.tmp.ToString() + "°C";

                string currentTimeString = "16:15";
                DateTime currentTime = DateTime.ParseExact(currentTimeString, "HH:mm", CultureInfo.InvariantCulture);

                // Get the hour part and calculate the next hour keys
                int nextHour = currentTime.Hour + 1; // Get the next hour
                string hourKey;

                for (int i = 0; i < 3; i++) // Get the next three hours
                {
                    // Calculate the next hour key
                    hourKey = $"_{(nextHour + i).ToString("D2")}H00"; // Format as _XXH00
                    Console.WriteLine($"Accessing data for: {hourKey}");

                    // Use reflection to access the hourly data dynamically
                    var hourlyDataProperty = typeof(HourlyData).GetProperty(hourKey);
                    if (hourlyDataProperty != null)
                    {
                        var hourData = (dynamic)hourlyDataProperty.GetValue(TodayForecast.hourly_data);
                        if (hourData != null)
                        {
                            string icon = hourData.ICON; // Access the ICON property
                            string temperature = hourData.Temperature; // Access the Temperature property
                            Console.WriteLine($"The icon for {hourKey} is: {icon}, Temperature: {temperature}");
                        }
                        else
                        {
                            Console.WriteLine($"No data found for {hourKey}.");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Property {hourKey} does not exist.");
                    }
                }

               // Uri weatherIn1H = new Uri(icon, UriKind.Absolute);

             //   Today_Image.Source = new BitmapImage(weatherIn1H);

                Heure_1.Text = apimanager.GetForecastForHour(1) + "00";
               // Image_1.Source = new BitmapImage(weatherIn1H);

                Heure_2.Text = apimanager.GetForecastForHour(2).ToString() + ":00";
                Heure_3.Text = apimanager.GetForecastForHour(3).ToString() + ":00";
                Heure_4.Text = apimanager.GetForecastForHour(4).ToString() + ":00";
                Heure_5.Text = apimanager.GetForecastForHour(5).ToString() + ":00";
                Heure_6.Text = apimanager.GetForecastForHour(6).ToString() + ":00";

                TB_TodayDate.Text = currentcondition.date.ToString();
                MessageBox.Show(currentcondition.date.ToString());
              
            }
            catch (Exception ex)
            {            }
        }
    }
}
