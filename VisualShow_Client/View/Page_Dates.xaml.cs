using System;
using System.Collections.Generic;
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
using VisualShow_Admin.Controller;
using VisualShow_Admin.Model;

namespace VisualShow_Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Dates.xaml
    /// </summary>
    public partial class Page_Dates : UserControl
    {
        DAO_Events dao_events = new DAO_Events();
        public Page_Dates()
        {
            InitializeComponent();
            UpdatePage_Date();
        }
        private async void UpdatePage_Date()
        {            
            var events = dao_events.GetEvents();
            for (int i = 0; i < 2; i++)
            {
                LV_Offres.Items.Add(new { name = events[0].name, date = "Event 1" });
            }
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
