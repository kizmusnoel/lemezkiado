using System.Collections.ObjectModel;
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

        static List<Album> loaded = Album.LoadFromJson("albums.json");
        ObservableCollection<Album> albums = new ObservableCollection<Album>(loaded);


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
            if (srcBox.Text == "" || srcBox.Text == "Keresés albumok közt..." || srcBox.Text == "Search among albums...") albumsListBox.ItemsSource = albums;
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
            Album newAlbum = new Album();
            newAlbum.id = albums.Count + 1;
            if (albumNameTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem ajda meg az album nevét!\n";
                albumNameTXT.BorderBrush = Brushes.Red;
            }
            newAlbum.albumName = albumNameTXT.Text;
            if (artistNameTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az előadó nevét!\n";
                artistNameTXT.BorderBrush = Brushes.Red;
            }
            if (releaseDateTXT.Text.Trim() == "" || !int.TryParse(releaseDateTXT.Text, out int result1))
            {
                errorMSG += "Kérem adja meg a megjelenés évét!\n";
                releaseDateTXT.BorderBrush = Brushes.Red;
            }
            else if (int.TryParse(releaseDateTXT.Text, out result1))
            {
                newAlbum.releaseDate = result1;
            }
            if (priceTXT.Text.Trim() == "" || !float.TryParse(priceTXT.Text, out float result2))
            {
                errorMSG += "Kérem adja meg az album árát!\n";
                priceTXT.BorderBrush = Brushes.Red;
            }
            else if (float.TryParse(priceTXT.Text, out result2))
            {
                newAlbum.price = result2;
            }
            if (streamsTXT.Text.Trim() == "" || !int.TryParse(streamsTXT.Text, out int result3))
            {
                errorMSG += "Kérem adja meg a streamek számát!\n";
                streamsTXT.BorderBrush = Brushes.Red;
            }
            else if (int.TryParse(streamsTXT.Text, out result3))
            {
                newAlbum.streams = result3;
            }
            if (copiesSoldTXT.Text.Trim() == "" || !int.TryParse(copiesSoldTXT.Text, out int result4))
            {
                errorMSG += "Kérem adja meg az eladott másolatok számát!\n";
                copiesSoldTXT.BorderBrush = Brushes.Red;
            }
            else if (int.TryParse(copiesSoldTXT.Text, out result4))
            {
                newAlbum.copiesSold = result4;
            }
            if (genreTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az album műfaját!\n";
                genreTXT.BorderBrush = Brushes.Red;
            }
            newAlbum.genre = genreTXT.Text;
            if (rbExplicitNo.IsChecked == false && rbExplicitYes.IsChecked == false)
            {
                errorMSG += "Kérem válassza ki az album korhatárosságát!\n";
                rbExplicitNo.BorderBrush = Brushes.Red;
                rbExplicitYes.BorderBrush = Brushes.Red;
            }
            if (rbExplicitYes.IsChecked == true)
                newAlbum.explicitAlbum = true;
            else if (rbExplicitNo.IsChecked == true)
                newAlbum.explicitAlbum = false;
            if (trackListTXT.Text.Trim() == "")
            {
                errorMSG += "Kérem adja meg az albumon lévő zenék neveit!\n";
                trackListTXT.BorderBrush = Brushes.Red;
            }
            newAlbum.trackList = trackListTXT.Text.Split(',');

            if (errorMSG == "")
            {
                albums.Add(newAlbum);
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
                MessageBox.Show(errorMSG);
            }

        }

        private void albumsListBoxSelected(object sender, RoutedEventArgs e)
        {
            foreach (var item in albums)
            {
                if (item.id == Convert.ToInt32(albumsListBox.SelectedValue))
                {
                    if (language)
                    {
                        albumData.Content =
                        $"Price: {item.price} $\n" +
                        $"Streams: {item.streams}\n" +
                        $"Physical sales: {item.copiesSold}\n" +
                        $"Genre: {item.genre}\n";
                    }
                    else
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

                lbl1.Content = "Add New Album";
                lbl2.Content = "Album name: ";
                lbl3.Content = "Artist name: ";
                lbl4.Content = "Release year: ";
                lbl5.Content = "Price ($): ";
                lbl6.Content = "Streams: ";
                lbl7.Content = "Physical sales: ";
                lbl8.Content = "Genre: ";
                lbl9.Content = "Explicit: ";
                lbl10.Content = "Tracklist: *listbox gombbal*";
            }
            else
            {
                srcBox.Text = "Keresés albumok közt...";
                btn2.Content = "Keresés";
                btn1.Content = "Új album felvétele";
                updateBtn.Content = "Album hozzáadása";

                lbl1.Content = "Új album felvétele";
                lbl2.Content = "Album neve: ";
                lbl3.Content = "Előadó neve: ";
                lbl4.Content = "Megjelenés éve: ";
                lbl5.Content = "Ár ($): ";
                lbl6.Content = "Streamek: ";
                lbl7.Content = "Fizikai eladások: ";
                lbl8.Content = "Műfaj: ";
                lbl9.Content = "Explicit: ";
                lbl10.Content = "Számok listája: *listbox gombbal*";
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