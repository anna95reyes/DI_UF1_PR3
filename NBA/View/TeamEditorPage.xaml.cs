using NBA_BD;
using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=234238

namespace NBA.View
{
    /// <summary>
    /// Una página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class TeamEditorPage : Page
    {
        ObservableCollection<TeamEditor> teams;

        public TeamEditorPage()
        {
            this.InitializeComponent();

            loadTeams();

            mapArena.Loaded += mapArena_Loaded;
        }

        private void loadTeams()
        {
            teams = TeamEditorDB.GetLlistaTeams();
            dgrTeams.ItemsSource = teams;
        }

        private async void mapArena_Loaded(object sender, RoutedEventArgs e)
        {
            Geopoint center =
               new Geopoint(new BasicGeoposition()
               {
                   //Latitude = 11.66509,
                   //Longitude = 78.154587
                   Latitude = 42.366303,
                   Longitude = -71.062228
               });
            await mapArena.TrySetSceneAsync(MapScene.CreateFromLocationAndRadius(center, 3000));
        }
    }
}
