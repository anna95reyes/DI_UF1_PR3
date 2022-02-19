using NBA_BD;
using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
    public sealed partial class PlayOffBracketPage : Page
    {
        StorageFolder appDataFolder = ApplicationData.Current.LocalFolder;
        ObservableCollection<Game> games;
        
        public PlayOffBracketPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbxNbaSeason.ItemsSource = SeasonDB.GetLlistaSeasons();
            cbxNbaSeason.SelectedIndex = 0;

            games = GameDB.GetLlistGames();
            String html;
            for (int i = 0; i < games.Count; i++)
            {
                games[i].HomeTeam = rutaImgAppData(games[i].HomeTeam);
                games[i].AwayTeam = rutaImgAppData(games[i].AwayTeam);
            }
            dgrGames.ItemsSource = games;

        }

        private String rutaImgAppData(string ruta)
        {
            Uri uri = new Uri(Path.Combine(appDataFolder.Path, ruta));

            return uri.AbsoluteUri;
        }
    }
}
