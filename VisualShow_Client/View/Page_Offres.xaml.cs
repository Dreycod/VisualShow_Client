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

namespace VisualShow_Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Offres.xaml
    /// </summary>
    public partial class Page_Offres : UserControl
    {
        DAO_Events dao_events;

        public Page_Offres()
        {
            InitializeComponent();
            dao_events = new DAO_Events();
            LoadEvents();

        }
        public async void LoadEvents()
        {
            var events = await dao_events.GetEvents();
            LV_Offres.ItemsSource = events;
        }
        private void Menu_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
