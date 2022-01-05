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
                btnCancelTeam.IsEnabled = false;
                btnSaveTeam.IsEnabled = false;
            }
            else if (estat == Estat.ALTA)
            {
                btnClearFilter.IsEnabled = false;
                btnDeleteTeam.IsEnabled = false;
                btnCancelTeam.IsEnabled = false;
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
            canviEstat(Estat.MODIFICACIO);
            validarDadesFormulari();
            cambiarCordenadesMapa();
        }

        private void txtLongMap_TextChanged(object sender, TextChangedEventArgs e)
        {
            canviEstat(Estat.MODIFICACIO);
            validarDadesFormulari();
            cambiarCordenadesMapa();
        }

        private void cambiarCordenadesMapa()
        {
            double lat = -200;
            double lng = -200;

            if (txtLatMap.Text != "" && txtLongMap.Text != "")
            {
                lat = campDoubleValid(lat, txtLatMap);
                lng = campDoubleValid(lng, txtLongMap);
            } 
            else
            {
                if (txtLatMap.Text == "") campTextBoxValid(false, txtLatMap);
                if (txtLongMap.Text == "") campTextBoxValid(false, txtLongMap);
            }

            if (Team.validaLatitud(lat) && Team.validaLatitud(lng)) {
                generarGeoPositionMapa(lat, lng);
            }
            
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

                canviEstat(Estat.MODIFICACIO);
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

                canviEstat(Estat.MODIFICACIO);
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

                canviEstat(Estat.MODIFICACIO);
                validarDadesFormulari();
            }   
        }

        private void validarDadesFormulari()
        {
            if (formulariValid())
            {
                bool hiHaCanvis = false;

                if (dgrTeams.SelectedItem != null)
                {
                    Team teamEditat = (Team)dgrTeams.SelectedItem;
                    hiHaCanvis = !(teamEditat.TeamCaption.Equals(txtNameTeam.Text) &&
                                   teamEditat.TeamShortCaption.Equals(txtShortNameTeam.Text) && 
                                   teamEditat.DivisionCaption.Equals((Division)cbxDivision.SelectedItem) &&
                                   teamEditat.TeamLogo.Equals(((SvgImageSource)imgLogoTeam.Source).UriSource.AbsoluteUri) &&
                                   teamEditat.ArenaCaption.Equals(txtArena.Text) &&
                                   teamEditat.ArenaLogo.Equals(((BitmapImage)imgLogoArena.Source).UriSource.AbsoluteUri) &&
                                   teamEditat.ArenaAbout.Equals(txtAbout.Text) &&
                                   teamEditat.ArenaCapacity.Equals(Int32.Parse(txtCapacity.Text)) &&
                                   teamEditat.ArenaPhoto.Equals(((BitmapImage)imgPhotoArena.Source).UriSource.AbsoluteUri) &&
                                   teamEditat.ArenaLat.Equals(Double.Parse(txtLatMap.Text)) &&
                                   teamEditat.ArenaLong.Equals(Double.Parse(txtLongMap.Text)) &&
                                   lsvPlayers.Items.Equals(PlayerDB.GetLlistaPlayers(teamEditat.TeamId)));
                }
                if (estat == Estat.MODIFICACIO && hiHaCanvis || estat == Estat.ALTA)
                {
                    btnCancelTeam.IsEnabled = true;
                    btnSaveTeam.IsEnabled = true;
                }
            }
            else
            {
                btnCancelTeam.IsEnabled = true;
                btnSaveTeam.IsEnabled = false;
            }
        }

        private bool formulariValid()
        {
            bool formulariValid = false;
            int capacity = -1;
            double lat = 0;
            double lng = 0;

            campTextBoxValid(Team.validaText(txtNameTeam.Text), txtNameTeam);
            campTextBoxValid(Team.validaShortCaption(txtShortNameTeam.Text), txtShortNameTeam);
            campTextBoxValid(Team.validaText(txtArena.Text), txtArena);
            campTextBoxValid(Team.validaText(txtAbout.Text), txtAbout);
            capacity = campIntegerValid(capacity, txtCapacity);
            lat = campDoubleValid(lat, txtLatMap);
            lng = campDoubleValid(lng, txtLongMap);

            if (Team.validaText(txtNameTeam.Text) && Team.validaShortCaption(txtShortNameTeam.Text) &&
                imgLogoTeam.Source != null && Team.validaText(txtArena.Text) &&
                imgLogoArena.Source != null && Team.validaText(txtAbout.Text) &&
                Team.validaCapacity(capacity) && imgPhotoArena.Source != null &&
                Team.validaLatitud(lat) && Team.validaLongitud(lng)
                )
            {
                formulariValid = true;
            }
            return formulariValid;
        }

        private int campIntegerValid(int valor, TextBox camp)
        {
            try
            {
                valor = Int32.Parse(camp.Text);
                campTextBoxValid(true, camp);

            }
            catch (Exception e)
            {
                campTextBoxValid(false, camp);
            }

            return valor;
        }

        private double campDoubleValid(double valor, TextBox camp)
        {
            try
            {
                valor = Double.Parse(camp.Text);
                campTextBoxValid(true, camp);

            }
            catch (Exception e)
            {
                campTextBoxValid(false, camp);
            }

            return valor;
        }

        private void campTextBoxValid(bool validacio, TextBox camp)
        {
            if (validacio)
            {
                camp.Background = new SolidColorBrush(Colors.Transparent);
            }
            else
            {
                camp.Background = new SolidColorBrush(Color.FromArgb(100,234,153,153));
            }
        }

        private void txtNameTeam_TextChanged(object sender, TextChangedEventArgs e)
        {
            canviEstat(Estat.MODIFICACIO);
            validarDadesFormulari();
        }

        private void txtShortNameTeam_TextChanged(object sender, TextChangedEventArgs e)
        {
            canviEstat(Estat.MODIFICACIO);
            validarDadesFormulari();
        }

        private void cbxDivision_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxDivision.SelectedItem != null)
            {
                canviEstat(Estat.MODIFICACIO);
                validarDadesFormulari();
            }
        }

        private void txtArena_TextChanged(object sender, TextChangedEventArgs e)
        {
            canviEstat(Estat.MODIFICACIO);
            validarDadesFormulari();
        }

        private void txtAbout_TextChanged(object sender, TextChangedEventArgs e)
        {
            canviEstat(Estat.MODIFICACIO);
            validarDadesFormulari();
        }

        private void txtCapacity_TextChanged(object sender, TextChangedEventArgs e)
        {
            canviEstat(Estat.MODIFICACIO);
            validarDadesFormulari();
        }

        private void Player_Eliminat(object sender, EventArgs e)
        {
            if (dgrTeams.SelectedItem != null && lsvPlayers.ItemsSource != null)
            {
                Team te = (Team)dgrTeams.SelectedItem;
                Player pl = (Player)lsvPlayers.ItemsSource;
                lsvPlayers.ItemsSource = PlayerDB.GetLlistaPlayers(te.TeamId);
            }
        }

        private void btnSaveTeam_Click(object sender, RoutedEventArgs e)
        {
            if (dgrTeams.SelectedItem != null)
            {
                Team te = (Team)dgrTeams.SelectedItem;
                if (!lsvPlayers.Items.Equals(PlayerDB.GetLlistaPlayers(te.TeamId)))
                {
                    lsvPlayers.ItemsSource = PlayerDB.GetLlistaPlayers(te.TeamId);
                }
            }
        }


        private async void btnAddPlayers_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog cpd = new ContentPlayerDialog {
                ElPlayer = null
            };
            ContentDialogResult result = await cpd.ShowAsync();
        }

        private void btnCancelTeam_Click(object sender, RoutedEventArgs e)
        {
            mostrarFormulari();
        }
    }
}
