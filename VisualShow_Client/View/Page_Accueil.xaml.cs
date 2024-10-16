using System;
using System.IO;
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
using System.Windows.Threading;
using VisualShow_Client.Controller;
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
        API_Quotes api_quotes;


        public Page_Accueil()
        {
            InitializeComponent();
            apimanager = new APIManager();
            api_quotes = new API_Quotes();
            InitializeQuote();
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

        private async void InitializeQuote() 
        {
            string QuotesCategoryFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\..\\Ressources", "QuotesCategory.txt");
            string[] lines = File.ReadAllLines(QuotesCategoryFile);
            Random rand = new Random();
            int randomIndex = rand.Next(0, lines.Length);
            string randomCategory = lines[randomIndex];
            MessageBox.Show(randomCategory);

            List<QuotesRoot> quotesList = await api_quotes.GetQuotesAsync(randomCategory);
            if (quotesList != null && quotesList.Count > 0)
            {
                Random quoteRand = new Random();
                int quoteIndex = quoteRand.Next(0, quotesList.Count);
                QuotesRoot selectedQuote = quotesList[quoteIndex];

                TB_DailyQuote.Text = selectedQuote.quote;
                TB_QuoteAuthor.Text = "- " + selectedQuote.author;
            }
            else
            {
                Console.WriteLine("No quotes found for this category.");
            }
        }

        private async void UpdateUI(string cityUpdate)
        {

            Image[] weatherImages = { Image_1, Image_2, Image_3, Image_4, Image_5, Image_6 };
            TextBlock[] hourTexts = { Heure_1, Heure_2, Heure_3, Heure_4, Heure_5, Heure_6 };
            TextBlock[] tempTexts = { Temp_1, Temp_2, Temp_3, Temp_4, Temp_5, Temp_6 };

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

                // Calculate the future time by adding i+1 hours to 'now'
                DateTime futureTime = now.AddHours(i + 1);
                int currentHour = futureTime.Hour;

                // Format hourKey as 1H00, 2H00, 10H00, etc.
                if (currentHour < 10)
                {
                    hourKey = futureTime.ToString("H") + "H00"; // Single digit for hours < 10 (e.g., 1H00, 2H00)
                }
                else
                {
                    hourKey = futureTime.ToString("HH") + "H00"; // Two digits for hours >= 10 (e.g., 10H00, 22H00)
                }

                // Format displayTime as 1:00, 10:00, 22:00, etc.
                displayTime = $"{currentHour}:00"; // No leading zero for hours < 10

                PropertyInfo propInfo = typeof(HourlyData).GetProperty($"_{hourKey}");

                if (propInfo != null)
                {
                    var hourlyData = propInfo.GetValue(TodayForecast.hourly_data);

                    if (hourlyData != null)
                    {
                        var iconProperty = hourlyData.GetType().GetProperty("ICON");
                        var tmpProperty = hourlyData.GetType().GetProperty("TMP2m");

                        if (iconProperty != null && tmpProperty != null)
                        {
                            string iconValue = (string)iconProperty.GetValue(hourlyData);
                            string tmpValue = (string)tmpProperty.GetValue(hourlyData);

                            // Set the text for hour and temperature
                            hourTexts[i].Text = displayTime; // Display the time in 1:00, 2:00, 10:00, etc.
                            string bigIconValue = iconValue.Replace(".png", "-big.png");

                            // Use CultureInfo.InvariantCulture to parse the temperature correctly
                            tempTexts[i].Text = Math.Round(double.Parse(tmpValue, CultureInfo.InvariantCulture)).ToString() + "°C";

                            // Set the weather image source
                            weatherImages[i].Source = new BitmapImage(new Uri(bigIconValue, UriKind.RelativeOrAbsolute));
                        }
                    }
                }
            }


        }
    }
}
