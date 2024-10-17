using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace VisualShow_Client.View
{
    /// <summary>
    /// Logique d'interaction pour Page_Media.xaml
    /// </summary>
    public partial class Page_Media : UserControl
    {
        public ObservableCollection<ImageItem> Images { get; set; } = new ObservableCollection<ImageItem>();
        DispatcherTimer timer;

        string _ecran_name;

        public Page_Media(string ecran_name)
        {
            InitializeComponent();
            ImagesControl.ItemsSource = Images;
            _ecran_name = ecran_name;
            LoadFtpImages();
            timer = new DispatcherTimer();
        }
        public void Initialize_Timer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(60);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        public void Timer_Tick(object sender, EventArgs e)
        {
            LoadFtpImages();
        }

        private void LoadFtpImages()
        {
            string ftpUrl = "ftp://ftp-borne-arcade.alwaysdata.net/Images/" + _ecran_name + "/";
            string username = "borne-arcade";
            string password = "borne-testing";

            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = new NetworkCredential(username, password);

            try
            {
                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string fileName;
                    while ((fileName = reader.ReadLine()) != null)
                    {
                        string fileUrl = ftpUrl + fileName;
                        BitmapImage image = DownloadImage(fileUrl, username, password);

                        if (image != null)
                        {
                            Images.Add(new ImageItem { FileName = fileName, ImageSource = image });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        // Download an image from FTP
        private BitmapImage DownloadImage(string ftpUrl, string username, string password)
        {
            try
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(ftpUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new NetworkCredential(username, password);

                using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
                using (Stream responseStream = response.GetResponseStream())
                using (MemoryStream ms = new MemoryStream())
                {
                    responseStream.CopyTo(ms);
                    ms.Position = 0;

                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.StreamSource = ms;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.EndInit();

                    return image;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading image: {ex.Message}");
                return null;
            }
        }
        private void Menu_Click(object sender, RoutedEventArgs e)
        {}
    }

    // Class to hold the image source
    public class ImageItem
    {
        public string FileName { get; set; } // Propriété pour stocker le nom du fichier
        public BitmapImage ImageSource { get; set; }
    }
}
