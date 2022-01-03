using NBA_BD;
using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace NBA.View
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class TeamEditorPage : Page
    {
        public enum Estat
        {
            VIEW,
            MODIFICACIO,
            ALTA
        }

        StorageFolder appDataFolder = ApplicationData.Current.LocalFolder;
        ObservableCollection<Team> teams;
        ObservableCollection<Division> divisions;
        Estat estat = Estat.VIEW;

        public TeamEditorPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            mapArena.Loaded += mapArena_Loaded;
            divisions = DivisionDB.GetLlistaDivision();
            cbxFilterDivision.ItemsSource = divisions;
            loadTeams();
            canviEstat(Estat.VIEW);
        }

        private void canviEstat(Estat estatNou)
        {
            estat = estatNou;
            if (estat == Estat.VIEW)
            {
                btnClearFilter.IsEnabled = false;
                btnDeleteTeam.IsEnabled = false;
                btnCancelTeam.IsEnabled = false;
                btnSaveTeam.IsEnabled = false;
            }
            else if (estat == Estat.MODIFICACIO)
            {
                btnClearFilter.IsEnabled = false;
                btnDeleteTeam.IsEnabled = false;
                btnCancelTeam.IsEnabled = true;
                btnSaveTeam.IsEnabled = false;
            }
            else if (estat == Estat.ALTA)
            {
                btnClearFilter.IsEnabled = false;
                btnDeleteTeam.IsEnabled = false;
                btnCancelTeam.IsEnabled = true;
                btnSaveTeam.IsEnabled = false;
            }
        }

        private void loadTeams()
        {
            netejarFormulari();
            teams = TeamDB.GetLlistaTeams((Division)cbxFilterDivision.SelectedItem, txtFilterName.Text);
            for (int i = 0; i < teams.Count; i++)
            {
                teams[i].TeamLogo = rutaImgAppData(teams[i].TeamLogo);
                teams[i].ArenaLogo = rutaImgAppData(teams[i].ArenaLogo);
                teams[i].ArenaPhoto = rutaImgAppData(teams[i].ArenaPhoto);
            }
            dgrTeams.ItemsSource = teams;
            if (teams != null)
            {
                dgrTeams.SelectedIndex = 0;
            }
            cbxDivision.ItemsSource = divisions;
            btnClearFilter.IsEnabled = txtFilterName.Text != "" || cbxFilterDivision.SelectedItem != null;
        }

        private String rutaImgAppData(string ruta)
        {
            Uri uri = new Uri(Path.Combine(appDataFolder.Path, ruta));

            return uri.AbsoluteUri;
        }

        private async void mapArena_Loaded(object sender, RoutedEventArgs e)
        {
            if (dgrTeams.SelectedItem != null)
            {
                Team te = (Team)dgrTeams.SelectedItem;
                generarGeoPositionMapa(te.ArenaLat, te.ArenaLong);
            }
        }

        private async void generarGeoPositionMapa(double latitud, double longitud)
        {
            BasicGeoposition userGeoPosition = new BasicGeoposition()
            {
                Latitude = latitud,
                Longitude = longitud
            };

            Geopoint center = new Geopoint(userGeoPosition);
            await mapArena.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(center, 3000));
        }

        private void dgrTeams_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            mostrarFormulari();
            if (lsvPlayers.Items.Count == 0)
            {
                btnDeleteTeam.IsEnabled = true;
            }
        }

        private void mostrarFormulari()
        {
            if (dgrTeams.SelectedItem != null)
            {
                Team te = (Team)dgrTeams.SelectedItem;
                txtNameTeam.Text = te.TeamCaption;
                txtShortNameTeam.Text = te.TeamShortCaption;
                cbxDivision.SelectedItem = te.DivisionCaption;
                generarImatgeSvg(imgLogoTeam, te.TeamLogo, 400, 400);
                txtArena.Text = te.ArenaCaption;
                imgLogoArena.Source = new BitmapImage(new Uri(te.ArenaLogo));
                txtAbout.Text = te.ArenaAbout;
                txtCapacity.Text = te.ArenaCapacity.ToString();
                imgPhotoArena.Source = new BitmapImage(new Uri(te.ArenaPhoto));
                txtLatMap.Text = te.ArenaLat.ToString();
                txtLongMap.Text = te.ArenaLong.ToString();
                generarGeoPositionMapa(te.ArenaLat, te.ArenaLong);
                lsvPlayers.ItemsSource = PlayerDB.GetLlistaPlayers(te.TeamId);
            }
        }

        private void netejarFormulari()
        {
            txtNameTeam.Text = "";
            txtShortNameTeam.Text = "";
            cbxDivision.SelectedItem = null;
            imgLogoTeam.Source = null;
            txtArena.Text = "";
            imgLogoArena.Source = null;
            txtAbout.Text = "";
            txtCapacity.Text = "";
            imgPhotoArena.Source = null;
            txtLatMap.Text = "";
            txtLongMap.Text = "";
            generarGeoPositionMapa(0, 0);
            lsvPlayers.ItemsSource = null;
        }

        private void generarImatgeSvg(Image img, String url, int height, int width)
        {
            SvgImageSource sis = new SvgImageSource(new Uri(url));
            sis.RasterizePixelHeight = height;
            sis.RasterizePixelWidth = width;
            img.Source = sis;
        }

        private void cbxFilterDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            loadTeams();
        }

        private void txtFilterName_TextChanged(object sender, TextChangedEventArgs e)
        {
            loadTeams();
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            cbxFilterDivision.SelectedItem = null;
            txtFilterName.Text = "";
            btnClearFilter.IsEnabled = false;
        }

        private void txtLatMap_TextChanged(object sender, TextChangedEventArgs e)
        {
            cambiarCordenadesMapa();
        }

        private void txtLongMap_TextChanged(object sender, TextChangedEventArgs e)
        {
            cambiarCordenadesMapa();
        }

        private void cambiarCordenadesMapa()
        {
            double lat = 0;
            double lng = 0;
            if (txtLatMap.Text != "" && txtLongMap.Text != "")
            {
                lat = Double.Parse(txtLatMap.Text);
                lng = Double.Parse(txtLongMap.Text);
            }

            generarGeoPositionMapa(lat, lng);
        }

        private async void btnUpdateImgLogoTeam_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker fp = new FileOpenPicker();
            fp.FileTypeFilter.Add(".svg");

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
                //BitmapImage tmpBitmap = new BitmapImage(new Uri(copiedFile.Path));

                // ..... YOUR CODE HERE ...........
                generarImatgeSvg(imgLogoTeam, copiedFile.Path, 400, 400);

                validarDadesFormulari();
            }
        }

        private async void btnUpdateImgLogoArena_Click(object sender, RoutedEventArgs e)
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
                imgLogoArena.Source = tmpBitmap;

                validarDadesFormulari();
            }
        }

        private async void btnUpdateImgPhotoArena_Click(object sender, RoutedEventArgs e)
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
                imgPhotoArena.Source = tmpBitmap;

                validarDadesFormulari();
            }   
        }

        private void validarDadesFormulari()
        {
            if (formulariValid())
            {

            }
        }

        private bool formulariValid()
        {
            bool formulariValid = false;

            if (campTextBoxValid(Team.validaText(txtNameTeam.Text), txtNameTeam) &&
                campTextBoxValid(Team.validaShortCaption(txtShortNameTeam.Text), txtShortNameTeam) &&
                imgLogoTeam.Source != null &&
                campTextBoxValid(Team.validaText(txtArena.Text), txtArena) &&
                imgLogoArena.Source != null &&
                campTextBoxValid(Team.validaText(txtAbout.Text), txtAbout) &&
                campTextBoxValid(Team.validaCapacity(Int32.Parse(txtCapacity.Text)), txtCapacity) &&
                imgPhotoArena.Source != null &&
                campTextBoxValid(Team.validaLatitud(Double.Parse(txtLatMap.Text)), txtLatMap) &&
                campTextBoxValid(Team.validaLongitud(Double.Parse(txtLongMap.Text)), txtLongMap)
                )
            {
                formulariValid = true;
            }

            return formulariValid;
        }

        private bool campTextBoxValid(bool validacio, TextBox camp)
        {
            if (validacio)
            {
                camp.Background = new SolidColorBrush(Colors.Transparent);
                validacio = true;
            }
            else
            {
                camp.Background = new SolidColorBrush(Colors.DarkRed);
            }
            return validacio;
        }

        private void txtNameTeam_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void txtShortNameTeam_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void cbxDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxDivision.SelectedItem != null)
            {
                validarDadesFormulari();
            }
        }

        private void txtArena_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void txtAbout_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }

        private void txtCapacity_TextChanged(object sender, TextChangedEventArgs e)
        {
            validarDadesFormulari();
        }
    }
}
