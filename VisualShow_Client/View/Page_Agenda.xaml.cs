using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VisualShow_Client.Controller;
using Google.Apis.Calendar.v3.Data;
using System.Collections.ObjectModel;
using Google.Apis.Calendar.v3;
using System.Reflection;
using VisualShow_Admin.Model;
using VisualShow_Admin.Controller;
using WpfAgendaDatabase.Service.DAO;
using System.Diagnostics;
using System.Windows.Threading;

namespace VisualShow_Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Agenda.xaml
    /// </summary>
    public partial class Page_Agenda : UserControl
    {
        public ObservableCollection<Event> Events { get; set; } = new ObservableCollection<Event>();
        DispatcherTimer timer;
        private int seconds; // Keeps track of elapsed seconds

        public Page_Agenda()
        {
            InitializeComponent();
            this.DataContext = this;
            Initialize_Timer();
            DateTime today = DateTime.Now;

            // Load events for today and the next 3 months upon startup
            LoadEventsAsync(today, today.AddMonths(3));
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
            seconds += 1;
            Debug.WriteLine(seconds);

            // Check for events every 60 seconds
            if (seconds % 60 == 0)
            {
                DateTime today = DateTime.Now;
                LoadEventsAsync(today, today.AddMonths(3)); // Load events for today to 3 months ahead
            }

            Debug.WriteLine("Checking air quality...");
        }

        private async void LoadEventsAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                var service = await DAO_GoogleCalendar.GetCalendarServiceAsync();
                var request = service.Events.List("primary");
                request.TimeMin = startDate;
                request.TimeMax = endDate;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                var events = await request.ExecuteAsync();

                // Clear existing events before adding new ones
                Events.Clear();

                foreach (var eventItem in events.Items)
                {
                    Events.Add(eventItem);
                }

                EventsListView.ItemsSource = Events;
            }
            catch (Exception ex)
            {
                // Handle the error, e.g., by displaying a message
                MessageBox.Show($"Erreur lors du chargement des événements: {ex.Message}");
            }
        }
        public void Menu_Click(object sender, EventArgs e) { }
    }
}
