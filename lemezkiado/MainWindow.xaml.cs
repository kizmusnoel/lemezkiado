using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lemezkiado
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<Album> albums = Album.LoadFromJson("albums.json");


        public MainWindow()
        {
            InitializeComponent();
            srcMenu.Visibility = Visibility.Visible;
            newAlbumMenu.Visibility = Visibility.Collapsed;
            albumsListBox.ItemsSource = albums;

        }

        private void srcBtnClick(object sender, RoutedEventArgs e)
        {
            srcMenu.Visibility = Visibility.Visible;
            newAlbumMenu.Visibility = Visibility.Collapsed;

        }

     
        private void newAlbumBtnClick(object sender, RoutedEventArgs e)
        {
            newAlbumMenu.Visibility = Visibility.Visible;
            srcMenu.Visibility = Visibility.Collapsed;

        }

        private void srcBoxGotFocus(object sender, RoutedEventArgs e)
        {
            srcBox.Text = "";
            srcBox.Opacity = 0.9;
        }

        private void srcBoxLostFocus(object sender, RoutedEventArgs e)
        {
            srcBox.Opacity = 0.6;
        }

        public static bool AlbumHasText(Album album, string search)
        {
            if (album.albumName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                album.artistName.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                Convert.ToString(album.releaseDate).Contains(search, StringComparison.OrdinalIgnoreCase) ||
                album.genre.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                album.trackList.Any(item =>
                item.Contains(search, StringComparison.OrdinalIgnoreCase))) return true;

            return false;
        }

        private void srcBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            List<Album> filteredAlbums = new List<Album>();
            if (albumsListBox == null) return;
            if (srcBox.Text == "") albumsListBox.ItemsSource = albums;
            else
            {

                foreach (var item in albums)
                {

                    if (AlbumHasText(item, srcBox.Text)) filteredAlbums.Add(item);
                }
                albumsListBox.ItemsSource = filteredAlbums;
            }


        }

        private void updateBtnClick(object sender, RoutedEventArgs e)
        {
            var errorMSG = "";
            if (albumNameTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem ajda meg az album nevét!";

            }
            if (artistNameTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az előadó nevét!";

            }
            if (releaseDateTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg a megjelenés évét!";

            }
            if (priceTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az album árát!";

            }
            if (streamsTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg a streamek számát!";

            }
            if (copiesSoldTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az eladott másolatok számát!";

            }
            if (genreTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az album műfaját!";

            }
            if (rbExplicitNo.IsChecked == false && rbExplicitYes.IsChecked == false)
            {
                errorMSG += "Kérem válassza ki az album korhatárosságát!";

            }
            if (trackListTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az albumon lévő zenék neveit!";

            }

            if (errorMSG == "")
            {
                string jsonString = JsonSerializer.Serialize(albums, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                using (StreamWriter sw = new StreamWriter("albums.json"))
                {
                    sw.Write(jsonString);
                }
                MessageBox.Show("Sikeres felvétel!");
                albumNameTXT.Text = "";
                artistNameTXT.Text = "";
                releaseDateTXT.Text = "";
                priceTXT.Text = "";
                streamsTXT.Text = "";
                copiesSoldTXT.Text = "";
                genreTXT.Text = "";
                rbExplicitNo.IsChecked = false;
                rbExplicitYes.IsChecked = false;
                trackListTXT.Text = "";
            }

        }

        private void albumsListBoxSelected(object sender, RoutedEventArgs e)
        {
            foreach (var item in albums)
            {
                if (item.id == Convert.ToInt32(albumsListBox.SelectedValue)) {
                    albumData.Content =
                        $"Ár: {item.price} $\n" +
                        $"Streamelések: {item.streams}\n" +
                        $"Fizikai eladások: {item.copiesSold}\n" +
                        $"Műfaj: {item.genre}\n";
                    albumSongs.ItemsSource = item.trackList;
                }
                
            }
        }
    }
}