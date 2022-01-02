using NBA_BD;
using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
        StorageFolder appDataFolder = ApplicationData.Current.LocalFolder;
        ObservableCollection<TeamEditor> teams;
        ObservableCollection<Division> divisions;

        public TeamEditorPage()
        {
            this.InitializeComponent();
            mapArena.Loaded += mapArena_Loaded;
            loadTeams();
        }

        private void loadTeams()
        {
            teams = TeamEditorDB.GetLlistaTeams();
            divisions = DivisionDB.GetLlistaDivision();
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
            cbxFilterDivision.ItemsSource = divisions;
            cbxDivision.ItemsSource = divisions;

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
                TeamEditor te = (TeamEditor)dgrTeams.SelectedItem;
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
            if (dgrTeams.SelectedItem != null)
            {
                TeamEditor te = (TeamEditor)dgrTeams.SelectedItem;
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

        private void generarImatgeSvg(Image img, String url, int height, int width)
        {
            SvgImageSource sis = new SvgImageSource(new Uri(url));
            sis.RasterizePixelHeight = height;
            sis.RasterizePixelWidth = width;
            img.Source = sis;
        }
    }
}
