using System.Globalization; // Add this at the top with other using directives
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
    /// 
    public partial class MainWindow : Window
    {
        static bool language = false; // false = HU, true = EN

        List<Album> albums = Album.LoadFromJson("albums.json");


        public MainWindow()
        {
            InitializeComponent();
            srcMenu.Visibility = Visibility.Visible;
            newAlbumMenu.Visibility = Visibility.Collapsed;
            albumsListBox.ItemsSource = albums;
            btn1.Background = "#6F732F".ToBrush();
            btn2.Background = "#264027".ToBrush();
        }

        private void srcBtnClick(object sender, RoutedEventArgs e)
        {
            srcMenu.Visibility = Visibility.Visible;
            newAlbumMenu.Visibility = Visibility.Collapsed;
            btn1.Background = "#6F732F".ToBrush();
            btn2.Background = "#264027".ToBrush();
        }

     
        private void newAlbumBtnClick(object sender, RoutedEventArgs e)
        {
            newAlbumMenu.Visibility = Visibility.Visible;
            srcMenu.Visibility = Visibility.Collapsed;
            btn1.Background = "#264027".ToBrush();
            btn2.Background = "#6F732F".ToBrush();
            
        }

        private void srcBoxGotFocus(object sender, RoutedEventArgs e)
        {
            if (srcBox.Text == "Keresés albumok közt..." || srcBox.Text == "Search among albums...") srcBox.Text = "";
            srcBox.Opacity = 0.9;
        }

        private void srcBoxLostFocus(object sender, RoutedEventArgs e)
        {
            srcBox.Opacity = 0.6;
            if (srcBox.Text == "")
            {
                if (language) srcBox.Text = "Search among albums...";
                else
                    srcBox.Text = "Keresés albumok közt...";
            }
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
            if (srcBox.Text == "" || srcBox.Text == "Keresés albumok közt..." ||srcBox.Text == "Search among albums...") albumsListBox.ItemsSource = albums;
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
                albumNameTXT.BorderBrush = Brushes.Red;

            }
            if (artistNameTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az előadó nevét!";
                artistNameTXT.BorderBrush = Brushes.Red;

            }
            if (releaseDateTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg a megjelenés évét!";
                releaseDateTXT.BorderBrush = Brushes.Red;

            }
            if (priceTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az album árát!";
                priceTXT.BorderBrush = Brushes.Red;

            }
            if (streamsTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg a streamek számát!";
                streamsTXT.BorderBrush = Brushes.Red;

            }
            if (copiesSoldTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az eladott másolatok számát!";
                copiesSoldTXT.BorderBrush = Brushes.Red;

            }
            if (genreTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az album műfaját!";
                genreTXT.BorderBrush = Brushes.Red;

            }
            if (rbExplicitNo.IsChecked == false && rbExplicitYes.IsChecked == false)
            {
                errorMSG += "Kérem válassza ki az album korhatárosságát!";
                rbExplicitNo.BorderBrush = Brushes.Red;
                rbExplicitYes.BorderBrush = Brushes.Red;

            }
            if (trackListTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az albumon lévő zenék neveit!";
                trackListTXT.BorderBrush = Brushes.Red;

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
            else
            {
                
            }

        }

        private void albumsListBoxSelected(object sender, RoutedEventArgs e)
        {
            foreach (var item in albums)
            {
                if (item.id == Convert.ToInt32(albumsListBox.SelectedValue)) {
                    if (language)
                    {
                        albumData.Content =
                        $"Price: {item.price} $\n" +
                        $"Streams: {item.streams}\n" +
                        $"Physical sales: {item.copiesSold}\n" +
                        $"Genre: {item.genre}\n";
                    } else
                    {

                        albumData.Content =
                            $"Ár: {item.price} $\n" +
                            $"Streamelések: {item.streams}\n" +
                            $"Fizikai eladások: {item.copiesSold}\n" +
                            $"Műfaj: {item.genre}\n";
                    }
                    albumSongs.ItemsSource = item.trackList;
                    albumSongs.Visibility = Visibility.Visible;
                }
                
            }
        }

        private void btnMouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Opacity = 1.4;
        }

        private void btnMouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            btn.Opacity = 1;
        }

        private void langBtnClick(object sender, RoutedEventArgs e)
        {
            language = !language;
            albumsListBoxSelected(null, null);
            if (language)
            {
                srcBox.Text = "Search among albums...";
                btn2.Content = "Search";
                btn1.Content = "Add New Album";
                updateBtn.Content = "Add Album";
            }
            else
            {
                srcBox.Text = "Keresés albumok közt...";
                btn2.Content = "Keresés";
                btn1.Content = "Új album felvétele";
                updateBtn.Content = "Album hozzáadása";
            }
        }
    }

    public static class BrushExtensions
    {
        public static Brush ToBrush(this string hex)
        {
            return (SolidColorBrush)(new BrushConverter().ConvertFromString(hex));
        }
    }
}