using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
            p.playerChangedCallback(d,e);
        }

        private void playerChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (ElPlayer != null)
            {
                imgPlayer.Source = generarImatgePlayer(ElPlayer.PlayerPhoto);
                txbWeight.Text = ElPlayer.PlayerWeight.ToString();
                txbHeight.Text = ElPlayer.PlayerHeight.ToString();
                txbBirthday.Text = ElPlayer.PlayerBithday.ToString("dd.MM.yyyy");
                txbStartYear.Text = ElPlayer.PlayerCareerStartYear.ToString();
                if (ElPlayer.CollageName != null)
                {
                    tbxTitleCollage.Text = "Collage: ";
                    txbCollage.Text = ElPlayer.CollageName;
                }
                else
                {
                    tbxTitleCollage.Text = "";
                }
                txbNumber.Text = ElPlayer.PlayerCurrentNumber.ToString();
                txbFirstName.Text = ElPlayer.PlayerFirstName;
                txbLastName.Text = ElPlayer.PlayerLastName;
                txbPosition.Text = ElPlayer.PlayerPosition;
                generarImatgeSvg(imgFlag, rutaImgFlagsAppData(ElPlayer.CountryShortName), 480, 640);
            }
        }

        

        private BitmapImage generarImatgePlayer(byte[] playerPhoto)
        {
            BitmapImage image = new BitmapImage();
            using (InMemoryRandomAccessStream stream = new InMemoryRandomAccessStream())
            {
                stream.AsStreamForWrite().Write(playerPhoto, 0, playerPhoto.Length);
                image.SetSource(stream);
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
            //ms-appx:///Assets/nba_images/flags/us.svg

            Uri uri = new Uri(Path.Combine(appDataFolder.Path, "nba_images"));
            uri = new Uri(Path.Combine(uri.AbsoluteUri, "flags"));
            uri = new Uri(Path.Combine(uri.AbsoluteUri, img + ".svg"));

            Debug.WriteLine(uri.AbsoluteUri);

            return uri.AbsoluteUri;
        }
    }
}
