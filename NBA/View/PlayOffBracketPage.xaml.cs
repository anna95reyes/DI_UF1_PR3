using NBA_BD;
using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
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
        public static readonly DependencyProperty SourceStringProperty = DependencyProperty.RegisterAttached("SourceString", typeof(string), typeof(PlayOffBracketPage), new PropertyMetadata("", OnSourceStringChanged));


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


        public static string GetSourceString(DependencyObject obj)
        {
            return obj.GetValue(SourceStringProperty).ToString();
        }

        public static void SetSourceString(DependencyObject obj, string value)
        {
            obj.SetValue(SourceStringProperty, value);
        }

        private static void OnSourceStringChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            WebView wv = d as WebView;
            
            if (wv != null)
            {

                Game g = (Game)wv.DataContext;
                String videoId = GetYouTubeId(g.Recap);

                String html =
                    @"
                        <html>
                            <head>
                                <style>
                                    .auto-resizable-iframe {
                                         max-width: 420px;
                                         margin: 0px auto;
                                    }

                                    .auto-resizable-iframe > div {
                                        position: relative;
                                        padding-bottom: 75%;
                                        height: 0px;
                                    }

                                    .auto-resizable-iframe iframe {
                                        position: absolute;
                                        top: 0px;
                                        left: 0px;
                                        width: 100%;
                                        height: 100%;
                                    }
                                </style>
                            </head>
                            <body>
                                <div class='auto-resizable-iframe'>
                                    <div>
                                        <iframe width='100' height='100' src='http://www.youtube.com/embed/" + videoId + @"?rel=0' display='block' frameborder='0' allowfullscreen></iframe>
                                    </div>
                                </div>
                            </body>
                        </html>
                    ";
                wv.NavigateToString(html);
            }

        }

        public static string GetYouTubeId(string url)
        {
            var regex = @"(?:youtube\.com\/(?:[^\/]+\/.+\/|(?:v|e(?:mbed)?|watch)\/|.*[?&amp;]v=)|youtu\.be\/)([^""&amp;?\/ ]{11})";

            var match = Regex.Match(url, regex);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            return url;
        }

    }
}
