using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace VisualShow_Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Media.xaml
    /// </summary>
    public partial class Page_Media : UserControl
    {
        DAO_FTP daoFtp = new DAO_FTP();
        public Page_Media()
        {
            InitializeComponent();
            LoadImages();
        }

        private async void LoadImages()
        {
            string ftpDirectory = "KM103"; // Set your FTP directory here
            string localDirectory = Path.Combine(Path.GetTempPath(), "DownloadedImages");

            // Download images from FTP to local directory
            await daoFtp.DownloadImagesFromDirectoryAsync(ftpDirectory, localDirectory);

            // Get the list of local image file paths
            var imageFiles = Directory.GetFiles(localDirectory, "*.jpg"); // Add other formats if necessary
            List<string> imagePaths = new List<string>();

            // Convert the local file paths to URIs for binding
            foreach (var file in imageFiles)
            {
                imagePaths.Add(new Uri(file).AbsoluteUri); // Convert to URI format
            }

            // Bind the image paths to the ListView
            ImageListView.ItemsSource = imagePaths;
        }

        private void Menu_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
