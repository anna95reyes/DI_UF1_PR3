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
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento del cuadro de diálogo de contenido está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace NBA.View
{
    public sealed partial class ContentPlayerDialog : ContentDialog
    {
        public ContentPlayerDialog()
        {
            this.InitializeComponent();
        }

        private Player elPlayer;

        public Player ElPlayer
        {
            get { return elPlayer; }
            set { elPlayer = value; }
        }

        StorageFolder appDataFolder = ApplicationData.Current.LocalFolder;

        public enum Estat
        {
            MODIFICACIO,
            ALTA
        }

        Estat estat;

        private void canviEstat(Estat estatNou)
        {
            estat = estatNou;
            if (estat == Estat.MODIFICACIO)
            {
                btnCancel.IsEnabled = true;
                btnSave.IsEnabled = false;
            }
            else if (estat == Estat.ALTA)
            {
                btnCancel.IsEnabled = true;
                btnSave.IsEnabled = false;
            }
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            cbxCollageName.ItemsSource = CollegeDB.GetLlistaCollege();
            cbxCountryName.ItemsSource = CountryDB.GetLlistaCountry();
            if (elPlayer == null)
            {
                netejarFormulari();
                canviEstat(Estat.ALTA);
            } 
            else
            {
                mostrarFormulari();
                canviEstat(Estat.MODIFICACIO);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            dialogPlayer.Hide();
        }

        private async void mostrarFormulari()
        {
            txtPlayerCurrentNumber.Text = ElPlayer.PlayerCurrentNumber.ToString();
            txtPlayerFirstName.Text = ElPlayer.PlayerFirstName.ToString();
            txtPlayerLastName.Text = ElPlayer.PlayerLastName.ToString();
            imgPlayerPhoto.Source = await ImageFromBytes(ElPlayer.PlayerPhoto);
            cbxCollageName.SelectedItem = ElPlayer.College;
            txtPlayerCareerStartYear.Text = ElPlayer.PlayerCareerStartYear.ToString();
            cbxCountryName.SelectedItem = ElPlayer.Country;
            txtPlayerHeight.Text = ElPlayer.PlayerHeight.ToString();
            txtPlayerWeight.Text = ElPlayer.PlayerWeight.ToString();
            cdpPlayerBithday.Date = ElPlayer.PlayerBithday;
            setPlayerPositions(ElPlayer.PlayerPosition);
        }

        private void netejarFormulari()
        {
            txtPlayerCurrentNumber.Text = "";
            txtPlayerFirstName.Text = "";
            txtPlayerLastName.Text = "";
            imgPlayerPhoto = null;
            cbxCollageName.SelectedItem = null;
            txtPlayerCareerStartYear.Text = "";
            cbxCountryName.SelectedItem = null;
            imgFlagCountryShortName = null;
            txtPlayerHeight.Text = "";
            txtPlayerWeight.Text = "";
            cdpPlayerBithday.Date = null;
            ckbPlayerPositionGuard.IsChecked = false;
            ckbPlayerPositionForeward.IsChecked = false;
            ckbPlayerPositionCenter.IsChecked = false;
        }

        public void setPlayerPositions(int position)
        {
            ckbPlayerPositionGuard.IsChecked = false;
            ckbPlayerPositionForeward.IsChecked = false;
            ckbPlayerPositionCenter.IsChecked = false;

            switch (position)
            {
                case 1:
                    ckbPlayerPositionCenter.IsChecked = true;
                    break;
                case 2:
                    ckbPlayerPositionGuard.IsChecked = true;
                    break;
                case 3:
                    ckbPlayerPositionCenter.IsChecked = true;
                    ckbPlayerPositionGuard.IsChecked = true;
                    break;
                case 4:
                    ckbPlayerPositionForeward.IsChecked = true;
                    break;
                case 5:
                    ckbPlayerPositionCenter.IsChecked = true;
                    ckbPlayerPositionForeward.IsChecked = true;
                    break;
                case 6:
                    ckbPlayerPositionGuard.IsChecked = true;
                    ckbPlayerPositionForeward.IsChecked = true;
                    break;
                case 7:
                    ckbPlayerPositionGuard.IsChecked = true;
                    ckbPlayerPositionForeward.IsChecked = true;
                    ckbPlayerPositionCenter.IsChecked = true;
                    break;
            }

        }

        public int getPlayerPositions()
        {
            int position = 0;

            if (ckbPlayerPositionGuard.IsChecked == true)
            {
                position = position + Int32.Parse((String)ckbPlayerPositionGuard.Tag);
            }
            else if (ckbPlayerPositionForeward.IsChecked == true)
            {
                position = position + Int32.Parse((String)ckbPlayerPositionForeward.Tag);
            }
            else if (ckbPlayerPositionCenter.IsChecked == true)
            {
                position = position + Int32.Parse((String)ckbPlayerPositionCenter.Tag);
            }

            return position;
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

        /*
        public async static Task<byte[]> ImageToBytes(BitmapImage image)
        {

        }
        */

        private async void btnPlayerPhoto_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fp = new FileOpenPicker();
            fp.FileTypeFilter.Add(".jpg");
            fp.FileTypeFilter.Add(".png");

            StorageFile sf = await fp.PickSingleFileAsync();
            if (sf != null)
            {
                // Cerca la carpeta de dades de l'aplicació, dins de ApplicationData
                var folder = ApplicationData.Current.LocalFolder;
                // Dins de la carpeta de dades, creem una nova carpeta "icons"
                var iconsFolder = await folder.CreateFolderAsync("icons", CreationCollisionOption.OpenIfExists);
                // Creem un nom usant la data i hora, de forma que no poguem repetir noms.
                string name = (DateTime.Now).ToString("yyyyMMddhhmmss") + "_" + sf.Name;
                // Copiar l'arxiu triat a la carpeta indicada, usant el nom que hem muntat
                StorageFile copiedFile = await sf.CopyAsync(iconsFolder, name);
                // Crear una imatge en memòria (BitmapImage) a partir de l'arxiu copiat a ApplicationData
                BitmapImage tmpBitmap = new BitmapImage(new Uri(copiedFile.Path));

                // ..... YOUR CODE HERE ...........
                imgPlayerPhoto.Source = tmpBitmap;
                validarDadesFormulari();
            }
        }

        private String rutaImgFlagsAppData(string img)
        {
            Uri uri = new Uri(Path.Combine(appDataFolder.Path, "nba_images"));
            uri = new Uri(Path.Combine(uri.AbsoluteUri, "flags"));
            uri = new Uri(Path.Combine(uri.AbsoluteUri, img + ".svg"));

            return uri.AbsoluteUri;
        }

        private void generarImatgeSvg(Image img, String url, int height, int width)
        {
            SvgImageSource sis = new SvgImageSource(new Uri(url));
            sis.RasterizePixelHeight = height;
            sis.RasterizePixelWidth = width;
            img.Source = sis;
        }

        private void cbxCountryName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxCountryName.SelectedItem != null)
            {
                Country country = (Country)cbxCountryName.SelectedItem;
                generarImatgeSvg(imgFlagCountryShortName, rutaImgFlagsAppData(CountryDB.GetShortCountry(country.Name)), 480, 640);
            }
        }

        private void validarDadesFormulari()
        {
            if (formulariValid())
            {
                bool hiHaCanvis = false;
                if (ElPlayer != null)
                {
                    DateTime dateTime = ElPlayer.PlayerBithday;
                    DateTimeOffset? dto = (DateTimeOffset?)cdpPlayerBithday.Date;
                    byte[] bytes = ElPlayer.PlayerPhoto;
                    /*
                    if (imgPlayerPhoto.Source != null) {
                        Task<byte[]> ipp = ImageToBytes((BitmapImage)imgPlayerPhoto.Source);
                        bytes = ipp.Result.ToArray();
                    }*/

                    if (dto != null)
                    {
                        dateTime = new DateTime(dto.Value.Ticks);
                    }

                    hiHaCanvis = !(
                        ElPlayer.PlayerCurrentNumber.Equals(Int32.Parse(txtPlayerCurrentNumber.Text)) &&
                        ElPlayer.PlayerFirstName.Equals(txtPlayerFirstName.Text) &&
                        ElPlayer.PlayerLastName.Equals(txtPlayerLastName.Text) &&
                        ElPlayer.PlayerPhoto.Equals(bytes) &&
                        ElPlayer.College.Equals((College)cbxCollageName.SelectedItem) &&
                        ElPlayer.PlayerCareerStartYear.Equals(Int32.Parse(txtPlayerCareerStartYear.Text)) &&
                        ElPlayer.Country.Equals((Country)cbxCountryName.SelectedItem) &&
                        ElPlayer.PlayerHeight.Equals(Int32.Parse(txtPlayerHeight.Text)) &&
                        ElPlayer.PlayerWeight.Equals(float.Parse(txtPlayerWeight.Text)) &&
                        ElPlayer.PlayerBithday.Equals(dateTime) &&
                        ElPlayer.PlayerPosition.Equals(getPlayerPositions())
                        );
                }
                if (estat == Estat.MODIFICACIO && hiHaCanvis || estat == Estat.ALTA)
                {
                    btnCancel.IsEnabled = true;
                    btnSave.IsEnabled = true;
                }
            }
            else
            {
                btnCancel.IsEnabled = true;
                btnSave.IsEnabled = false;
            }
        }

        private bool formulariValid()
        {
            bool formulariValid = false;
            if (ElPlayer != null && Player.validaNumber(campIntegerValid(txtPlayerCurrentNumber.Text)) &&
                Player.validaText(txtPlayerFirstName.Text) && Player.validaText(txtPlayerLastName.Text) &&
                imgPlayerPhoto != null && Player.validaYear(campIntegerValid(txtPlayerCareerStartYear.Text)) &&
                cbxCountryName.SelectedItem != null && Player.validaHeight(campIntegerValid(txtPlayerHeight.Text)) &&
                Player.validaWeight(campFloatValid(txtPlayerWeight.Text)) && cdpPlayerBithday.Date != null &&
                Player.validaPosition(getPlayerPositions())
                )
            {
                formulariValid = true;
            }
            return formulariValid;
        }

        private int campIntegerValid(String camp)
        {
            int valor = -1;
            try
            {
                valor = Int32.Parse(camp);

            }
            catch (Exception e){}

            return valor;
        }

        private float campFloatValid(String camp)
        {
            float valor = -1;
            try
            {
                valor = float.Parse(camp);

            }
            catch (Exception e) { }

            return valor;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (estat == Estat.ALTA)
            {
                DateTime dateTime = new DateTime();
                DateTimeOffset? dto = (DateTimeOffset?)cdpPlayerBithday.Date;
                if (dto != null)
                {
                    dateTime = new DateTime(dto.Value.Ticks);
                }


                //TODO: s'ha de cambiar ElPlayer.PlayerPhoto per la funcio que converteix la foto en byte[]
                Player p = new Player(ElPlayer.PlayerId, 0, Int32.Parse(txtPlayerCurrentNumber.Text),txtPlayerFirstName.Text,
                                      txtPlayerLastName.Text,ElPlayer.PlayerPhoto, (College)cbxCollageName.SelectedItem,
                                      Int32.Parse(txtPlayerCareerStartYear.Text), (Country)cbxCountryName.SelectedItem,
                                      Int32.Parse(txtPlayerHeight.Text), float.Parse(txtPlayerWeight.Text),
                                      dateTime, getPlayerPositions());
                PlayerDB.insert(p);
            }
            else
            {

            }
        }

        private void txtPlayerCurrentNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        

        private void txtPlayerFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void txtPlayerLastName_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void cbxCollageName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void txtPlayerCareerStartYear_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void txtPlayerHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void txtPlayerWeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void cdpPlayerBithday_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            validarDadesFormulari();
        }

        private void ckbPlayerPositionGuard_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void ckbPlayerPositionForeward_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void ckbPlayerPositionCenter_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        
    }
}
