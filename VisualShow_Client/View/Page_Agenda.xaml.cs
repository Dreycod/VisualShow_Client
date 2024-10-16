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
using System.Windows.Controls;
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
using Microsoft.Extensions.Logging;
using VisualShow_Admin.Model;
using VisualShow_Admin.Controller;

namespace VisualShow_Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Agenda.xaml
    /// </summary>
    public partial class Page_Agenda : UserControl
    {
        public ObservableCollection<Event> Events { get; set; }

        public Page_Agenda()
        {
            InitializeComponent();
            DataContext = this;
            LoadGoogleCalendarEvents(DateTime.Today);
        }
        private void LoadGoogleCalendarEvents(DateTime selectedDate)
        {
            var service = GoogleCalendarAuth.GetCalendarService();
            var events = service.Events.List("primary").Execute().Items;

            var eventList = new List<Event>();

            foreach (var item in events)
            {
                var startDateTime = item.Start?.DateTime;
                var startDate = startDateTime?.Date; // Get only the date part

                if (startDate == selectedDate.Date)
                {
                    eventList.Add(new Event
                    {
                        Summary = item.Summary,
                        UpdatedDateTimeOffset = item.Start.DateTime,
                        EndTimeUnspecified = item.EndTimeUnspecified
                    });
                }
            }

            EventsListView.ItemsSource = eventList;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime selectedDate = Calendar_Events.SelectedDate.GetValueOrDefault();
            LoadGoogleCalendarEvents(selectedDate);
        }
        private void Menu_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
