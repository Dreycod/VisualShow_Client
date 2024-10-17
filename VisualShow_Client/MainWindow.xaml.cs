using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using VisualShow_Admin.Controller;
using VisualShow_Admin.Model;
using VisualShow_Client.View;

namespace VisualShow_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Page_Accueil page_accueil;
        Page_Meteo page_meteo;
        Page_Offres page_offres;
        Page_Agenda page_agenda;
        Page_Dates page_dates;
        Page_Media page_media;
        DAO_Ecrans daoEcrans;
        public MainWindow()
        {
            InitializeComponent();
            page_accueil = new Page_Accueil();
            page_meteo = new Page_Meteo();
            page_offres = new Page_Offres();
            page_agenda = new Page_Agenda();
            page_dates = new Page_Dates();
            daoEcrans = new DAO_Ecrans();

            LoadEcran();
         
        }

        public async void LoadEcran()
        {
            var ecrans = await daoEcrans.GetEcrans();
            if (ecrans != null)
            {
                int count = ecrans.Count;
                
                for (int i = 0; i < count; i++)
                {
                    ComboBoxPages.Items.Add(ecrans[i].name);
                }
                
            }
        }

        public async void MethodAsync(int normalPage, int mainPage)
        {
            while (true)
            {
                await SwitchContent(page_accueil, mainPage);
                await SwitchContent(page_meteo, normalPage);
                await SwitchContent(page_offres, normalPage);
                await SwitchContent(page_agenda, normalPage);
                await SwitchContent(page_dates, normalPage);
                await SwitchContent(page_media, normalPage);
            }
        }

        private async Task SwitchContent(UIElement newContent, int delay)
        {
            UIElement oldContent = null;

            if (GridContainer.Children.Count > 0)
            {
                oldContent = GridContainer.Children[0];
            }

            Grid.SetZIndex(newContent, 1);
            newContent.RenderTransform = new TranslateTransform();
            GridContainer.Children.Add(newContent);

            if (oldContent != null)
            {
                var storyboardOut = (Storyboard)this.Resources["PageSlideOut"];
                var storyboardIn = (Storyboard)this.Resources["PageSlideIn"];

                oldContent.RenderTransform = new TranslateTransform();
                Storyboard.SetTarget(storyboardOut, oldContent);
                storyboardOut.Begin();

                Storyboard.SetTarget(storyboardIn, newContent);
                storyboardIn.Begin();

                await Task.Delay(1500);
                GridContainer.Children.Remove(oldContent);
            }

            await Task.Delay(delay);
        }

        private void ComboBoxPages_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxPages.Visibility = Visibility.Hidden;
            int mainPage = 5000; // 30000
            int normalPage = 5000; // 20000
            page_media = new Page_Media(ComboBoxPages.SelectedItem.ToString());

            MethodAsync(normalPage, mainPage);
        }
    }
}
