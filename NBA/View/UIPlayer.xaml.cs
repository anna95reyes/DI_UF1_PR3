using NBA_BD;
using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace NBA.View
{
    public sealed partial class UIPlayer : UserControl
    {
        public UIPlayer()
        {
            this.InitializeComponent();
        }

        StorageFolder appDataFolder = ApplicationData.Current.LocalFolder;

        public Player ElPlayer
        {
            get { return (Player)GetValue(ElPlayerProperty); }
            set { SetValue(ElPlayerProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ElPlayer.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ElPlayerProperty =
            DependencyProperty.Register("ElPlayer", typeof(Player), typeof(UIPlayer), new PropertyMetadata(null, playerChangedCallbackStatic));

        private static void playerChangedCallbackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIPlayer p = (UIPlayer)d;
            p.playerChangedCallbackAsync(d,e);
        }

        private async Task playerChangedCallbackAsync(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (ElPlayer != null)
            {
                imgPlayer.Source = await ImageFromBytes(ElPlayer.PlayerPhoto);
                txbWeight.Text = ElPlayer.PlayerWeight.ToString();
                txbHeight.Text = ElPlayer.PlayerHeight.ToString();
                txbBirthday.Text = ElPlayer.PlayerBithday.ToString("dd.MM.yyyy");
                txbStartYear.Text = ElPlayer.PlayerCareerStartYear.ToString();
                if (ElPlayer.College.Name != null)
                {
                    tbxTitleCollege.Text = "Collage: ";
                    txbCollege.Text = ElPlayer.College.ToString();
                }
                else
                {
                    tbxTitleCollege.Text = "";
                }
                txbNumber.Text = ElPlayer.PlayerCurrentNumber.ToString();
                txbFirstName.Text = ElPlayer.PlayerFirstName;
                txbLastName.Text = ElPlayer.PlayerLastName;
                txbPosition.Text = ElPlayer.getPlayerPosition(ElPlayer.PlayerPosition);
                generarImatgeSvg(imgFlag, rutaImgFlagsAppData(CountryDB.GetShortCountry(ElPlayer.Country.Name)), 480, 640);
            }
        }

        public async static Task<BitmapImage> ImageFromBytes(byte[] bytes)
        {
            BitmapImage image = new BitmapImage();
            if (bytes != null)
            {
                using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
                {
                    await stream.WriteAsync(bytes.AsBuffer());
                    stream.Seek(0);
                    await image.SetSourceAsync(stream);
                }
            }
            return image;
        }

        private void generarImatgeSvg(Image img, String url, int height, int width)
        {
            SvgImageSource sis = new SvgImageSource(new Uri(url));
            sis.RasterizePixelHeight = height;
            sis.RasterizePixelWidth = width;
            img.Source = sis;
        }

        private String rutaImgFlagsAppData(string img)
        {
            Uri uri = new Uri(Path.Combine(appDataFolder.Path, "nba_images"));
            uri = new Uri(Path.Combine(uri.AbsoluteUri, "flags"));
            uri = new Uri(Path.Combine(uri.AbsoluteUri, img + ".svg"));

            return uri.AbsoluteUri;
        }

        private void btnDeletePlayer_Click(object sender, RoutedEventArgs e)
        {
            DisplayDeletePlayerDialog();
        }

        private async void btnEditPlayer_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog cpd = new ContentPlayerDialog
            {
                ElPlayer = ElPlayer
            };
            ContentDialogResult result = await cpd.ShowAsync();
        }

        private async void DisplayDeletePlayerDialog()
        {
            ContentDialog deleteFileDialog = new ContentDialog
            {
                Title = "Delete player permanently?",
                Content = "If you delete this player, you won't be able to recover it. Do you want to delete it?",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            ContentDialogResult result = await deleteFileDialog.ShowAsync();

            
            if (result == ContentDialogResult.Primary)
            {
                PlayerDB.delete(ElPlayer.PlayerId);
            }
        }
    }
}
