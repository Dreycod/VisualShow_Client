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

namespace VisualShow_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            int normalPage = 5500;
            int mainPage = 10000;
        _: MethodAsync(normalPage, mainPage);
        }
        public async void MethodAsync(int normalPage, int mainPage)
        {
            while (true)
            {
                await SwitchContent(new View.Page_Accueil(), mainPage);
                await SwitchContent(new View.Page_Meteo(), normalPage);
                await SwitchContent(new View.Page_Offres(), normalPage);
                await SwitchContent(new View.Page_Agenda(), normalPage);
                await SwitchContent(new View.Page_Dates(), normalPage);
                await SwitchContent(new View.Page_Media(), normalPage);
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
    }
}