using System.Text;
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
            albums.Sort();
            albumsListBox.ItemsSource = albums;

        }

        private void srcBtnClick(object sender, RoutedEventArgs e)
        {
            srcMenu.Visibility = Visibility.Visible;
            newAlbumMenu.Visibility = Visibility.Collapsed;

        }

        private void updateBtnClick(object sender, RoutedEventArgs e)
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
            srcBox.Opacity = 1;
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
    }
}