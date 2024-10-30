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
using System.Windows.Media.Imaging;

namespace VisualShow_Client.View
{

    public partial class Page_Urgence : Page
    {
        public Page_Urgence(string alertType)
        {
            InitializeComponent();

            //image en fonction du type d'alerte
            string imagePath = alertType switch
            {
                "FireAlert" => "pack://application:,,,/Ressources/Images/Incendie.jpg",
                "IntruderAlert" => "pack://application:,,,/Ressources/Images/Intrusion.jpg",
                "GeneralEmergency" => "pack://application:,,,/Ressources/Images/Générale.jpg",
            };
                EmergencyImage.Source = new BitmapImage(new Uri(imagePath));
        }
    }
}