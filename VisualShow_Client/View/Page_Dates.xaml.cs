using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using VisualShow_Admin.Controller;
using VisualShow_Admin.Model;

namespace VisualShow_Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Dates.xaml
    /// </summary>
    public partial class Page_Dates : UserControl
    {
        private DispatcherTimer scrollTimer;
        private DAO_Events dao_events = new DAO_Events();

        public Page_Dates()
        {
            InitializeComponent();
            UpdatePage_Date();
            StartAutoScroll();
        }

        private void StartAutoScroll()
        {
            scrollTimer = new DispatcherTimer();
            scrollTimer.Interval = TimeSpan.FromSeconds(1); // Adjust the interval if necessary
            scrollTimer.Tick += ScrollToBottom;
            scrollTimer.Start();
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            scrollTimer.Stop();
        }

        private void ScrollToBottom(object sender, EventArgs e)
        {
            // Automatically scroll to the last item if there are items
            if (LB_Agenda.Items.Count > 0)
            {
                LB_Agenda.ScrollIntoView(LB_Agenda.Items[LB_Agenda.Items.Count - 1]);
            }
        }

        private async void UpdatePage_Date()
        {
            var events = await dao_events.GetEvents();
            LB_Agenda.ItemsSource = events;

            // Scroll to the bottom after items are updated
            if (events.Count > 0)
            {
                LB_Agenda.ScrollIntoView(LB_Agenda.Items[LB_Agenda.Items.Count - 1]);
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {
            // Handle menu click
        }
    }
}
