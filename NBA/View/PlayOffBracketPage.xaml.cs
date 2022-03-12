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
        ObservableCollection<PlayOff> plays;

        public PlayOffBracketPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            cbxNbaSeason.ItemsSource = SeasonDB.GetLlistaSeasons();
            cbxNbaSeason.SelectedIndex = 0;
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

        private void cbxNbaSeason_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbxNbaSeason.SelectedItem != null)
            {
                Season s = (Season)cbxNbaSeason.SelectedItem;

                plays = PlayOffDB.GetLlistaSeasons(s.Caption, 1, "Western");
                if (plays.Count == 4)
                {
                    Enfrentament(uiWesternRound1Enf1Local, uiWesternRound1Enf1Visitant, plays[0], 1, 0);
                    Enfrentament(uiWesternRound1Enf2Local, uiWesternRound1Enf2Visitant, plays[1], 1, 1);
                    Enfrentament(uiWesternRound1Enf3Local, uiWesternRound1Enf3Visitant, plays[2], 1, 2);
                    Enfrentament(uiWesternRound1Enf4Local, uiWesternRound1Enf4Visitant, plays[3], 1, 3);
                } 
                else
                {
                    Enfrentament(uiWesternRound1Enf1Local, uiWesternRound1Enf1Visitant, null, 0, 0);
                    Enfrentament(uiWesternRound1Enf2Local, uiWesternRound1Enf2Visitant, null, 0, 0);
                    Enfrentament(uiWesternRound1Enf3Local, uiWesternRound1Enf3Visitant, null, 0, 0);
                    Enfrentament(uiWesternRound1Enf4Local, uiWesternRound1Enf4Visitant, null, 0, 0);
                }

                plays = PlayOffDB.GetLlistaSeasons(s.Caption, 2, "Western");
                if (plays.Count == 2)
                {
                    Enfrentament(uiWesternRound2Enf1Local, uiWesternRound2Enf1Visitant, plays[0], 2, 0);
                    Enfrentament(uiWesternRound2Enf2Local, uiWesternRound2Enf2Visitant, plays[1], 2, 1);
                }
                else
                {
                    Enfrentament(uiWesternRound2Enf1Local, uiWesternRound2Enf1Visitant, null, 0, 0);
                    Enfrentament(uiWesternRound2Enf2Local, uiWesternRound2Enf2Visitant, null, 0, 0);
                }

                plays = PlayOffDB.GetLlistaSeasons(s.Caption, 3, "Western");
                if (plays.Count == 1)
                {
                    Enfrentament(uiWesternRound3Enf1Local, uiWesternRound3Enf1Visitant, plays[0], 3, 0);
                }
                else
                {
                    Enfrentament(uiWesternRound3Enf1Local, uiWesternRound3Enf1Visitant, null, 0, 0);
                }

                plays = PlayOffDB.GetLlistaSeasons(s.Caption, 4, "");
                if (plays.Count == 1)
                {
                    Enfrentament(uiFinalsLocal, uiFinalsVisitant, plays[0], 4, 0);
                } 
                else
                {
                    Enfrentament(uiFinalsLocal, uiFinalsVisitant, null, 0, 0);
                }

                plays = PlayOffDB.GetLlistaSeasons(s.Caption, 3, "Eastern");
                if (plays.Count == 1)
                {
                    Enfrentament(uiEasternRound3Enf1Local, uiEasternRound3Enf1Visitant, plays[0], 3, 0);
                }
                else
                {
                    Enfrentament(uiEasternRound3Enf1Local, uiEasternRound3Enf1Visitant, null, 0, 0);
                }

                plays = PlayOffDB.GetLlistaSeasons(s.Caption, 2, "Eastern");
                if (plays.Count == 2)
                {
                    Enfrentament(uiEasternRound2Enf1Local, uiEasternRound2Enf1Visitant, plays[0], 2, 0);
                    Enfrentament(uiEasternRound2Enf2Local, uiEasternRound2Enf2Visitant, plays[1], 2, 1);
                } 
                else
                {
                    Enfrentament(uiEasternRound2Enf1Local, uiEasternRound2Enf1Visitant, null, 0, 0);
                    Enfrentament(uiEasternRound2Enf2Local, uiEasternRound2Enf2Visitant, null, 0, 0);
                }

                plays = PlayOffDB.GetLlistaSeasons(s.Caption, 1, "Eastern");
                if (plays.Count == 4)
                {
                    Enfrentament(uiEasternRound1Enf1Local, uiEasternRound1Enf1Visitant, plays[0], 1, 0);
                    Enfrentament(uiEasternRound1Enf2Local, uiEasternRound1Enf2Visitant, plays[1], 1, 1);
                    Enfrentament(uiEasternRound1Enf3Local, uiEasternRound1Enf3Visitant, plays[2], 1, 2);
                    Enfrentament(uiEasternRound1Enf4Local, uiEasternRound1Enf4Visitant, plays[3], 1, 3);
                } 
                else
                {
                    Enfrentament(uiEasternRound1Enf1Local, uiEasternRound1Enf1Visitant, null, 0, 0);
                    Enfrentament(uiEasternRound1Enf2Local, uiEasternRound1Enf2Visitant, null, 0, 0);
                    Enfrentament(uiEasternRound1Enf3Local, uiEasternRound1Enf3Visitant, null, 0, 0);
                    Enfrentament(uiEasternRound1Enf4Local, uiEasternRound1Enf4Visitant, null, 0, 0);
                }

                plays = null;
            }
        }

        

        private void EnfrontamentSeleccionat(Season s, PlayOff play, int round)
        {
            if (play != null)
            {

                UIEnfrentamentsSeleccionat(uiWestern, play.FirstTeamWins > play.SecondTeamWins, play.FirstTeamName,
                    play.SeedFirstTeam, rutaImgAppData(play.FirstTeamLogo), play.FirstTeamWins);

                txbVS.Text = "VS";

                UIEnfrentamentsSeleccionat(uiEastern, play.SecondTeamWins > play.FirstTeamWins, play.SecondTeamName,
                    play.SeedSecondTeam, rutaImgAppData(play.SecondTeamLogo), play.SecondTeamWins);


                games = GameDB.GetLlistGames(s.Caption, round, play.FirstTeamName, play.SecondTeamName);
                for (int i = 0; i < games.Count; i++)
                {
                    games[i].HomeTeam = rutaImgAppData(games[i].HomeTeam);
                    games[i].AwayTeam = rutaImgAppData(games[i].AwayTeam);
                }
                dgrGames.ItemsSource = games;

            }
            else
            {

                UIEnfrentamentsSeleccionat(uiWestern, false, null, 0, null, 0);

                txbVS.Text = "";

                UIEnfrentamentsSeleccionat(uiEastern, false, null, 0, null, 0);

                dgrGames.ItemsSource = null;
            }
        }

        private void UIEnfrentamentsSeleccionat(UIPlayOffEquip ui, bool esGuanyador, string teamName, int teamSeed, string teamLogo, int teamWins)
        {
            ui.EsGuanyador = esGuanyador;
            ui.NomTeam = teamName;
            ui.SeedTeam = teamSeed;
            ui.LogoTeam = teamLogo;
            ui.WinsTeam = teamWins;
        }

        private void UIEnfrentaments(UIEnfrentamentsWestern ui, bool esGuanyador, int teamSeed, string teamLogo, int teamWins, int round, int enfrentament)
        {
            ui.EsGuanyador = esGuanyador;
            ui.SeedTeam = teamSeed;
            ui.LogoTeam = teamLogo;
            ui.WinsTeam = teamWins;
            ui.Round = round;
            ui.Enfrentament = enfrentament;
        }

        private void UIEnfrentaments(UIEnfrentamentsEastern ui, bool esGuanyador, int teamSeed, string teamLogo, int teamWins, int round, int enfrentament)
        {
            ui.EsGuanyador = esGuanyador;
            ui.SeedTeam = teamSeed;
            ui.LogoTeam = teamLogo;
            ui.WinsTeam = teamWins;
            ui.Round = round;
            ui.Enfrentament = enfrentament;
        }

        private void UIEnfrentaments(UIEnfrentamentsFinals ui, bool esGuanyador, int teamSeed, string teamLogo, int teamWins, int round, int enfrentament)
        {
            ui.EsGuanyador = esGuanyador;
            ui.SeedTeam = teamSeed;
            ui.LogoTeam = teamLogo;
            ui.WinsTeam = teamWins;
            ui.Round = round;
            ui.Enfrentament = enfrentament;
        }

        private void Enfrentament(UIEnfrentamentsWestern uiLocal, UIEnfrentamentsWestern uiVisitant, PlayOff play, int round, int enfrentament)
        {
            if (play != null)
            {
                UIEnfrentaments(uiLocal, play.FirstTeamWins > play.SecondTeamWins,
                                            play.SeedFirstTeam, rutaImgAppData(play.FirstTeamLogo), play.FirstTeamWins,
                                            round, enfrentament);
                UIEnfrentaments(uiVisitant, play.SecondTeamWins > play.FirstTeamWins,
                            play.SeedSecondTeam, rutaImgAppData(play.SecondTeamLogo), play.SecondTeamWins,
                            round, enfrentament);
            }
            else
            {
                UIEnfrentaments(uiLocal, false, 0, null, 0, 0, 0);
                UIEnfrentaments(uiVisitant, false, 0, null, 0, 0, 0);
            }
        }
    

        private void Enfrentament(UIEnfrentamentsEastern uiLocal, UIEnfrentamentsEastern uiVisitant, PlayOff play, int round, int enfrentament)
        {
            if (play != null)
            {
                UIEnfrentaments(uiLocal, play.FirstTeamWins > play.SecondTeamWins,
                                            play.SeedFirstTeam, rutaImgAppData(play.FirstTeamLogo), play.FirstTeamWins,
                                            round, enfrentament);
                UIEnfrentaments(uiVisitant, play.SecondTeamWins > play.FirstTeamWins,
                            play.SeedSecondTeam, rutaImgAppData(play.SecondTeamLogo), play.SecondTeamWins,
                                            round, enfrentament);
            }
            else
            {
                UIEnfrentaments(uiLocal, false, 0, null, 0, 0, 0);
                UIEnfrentaments(uiVisitant, false, 0, null, 0, 0, 0);
            }
        }
    

        private void Enfrentament(UIEnfrentamentsFinals uiLocal, UIEnfrentamentsFinals uiVisitant, PlayOff play, int round, int enfrentament)
        {
            if (play != null)
            {
                UIEnfrentaments(uiLocal, play.FirstTeamWins > play.SecondTeamWins,
                                                play.SeedFirstTeam, rutaImgAppData(play.FirstTeamLogo), play.FirstTeamWins,
                                            round, enfrentament);
                UIEnfrentaments(uiVisitant, play.SecondTeamWins > play.FirstTeamWins,
                                play.SeedSecondTeam, rutaImgAppData(play.SecondTeamLogo), play.SecondTeamWins,
                                            round, enfrentament);
            }
            else
            {
                UIEnfrentaments(uiLocal, false, 0, null, 0, 0, 0);
                UIEnfrentaments(uiVisitant, false, 0, null, 0, 0, 0);
            }
        }

        private void uiEnfrentament_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("SENDER: " + sender.ToString());
            int round = 0;
            int enfrentament = 0;
            String conference = null;

            if (sender.ToString().Equals("NBA.View.UIEnfrentamentsFinals"))
            {
                UIEnfrentamentsFinals ui = (UIEnfrentamentsFinals)sender;
                Debug.WriteLine("UI: " + sender.GetType().FullName);
                conference = "";
                round = ui.Round;
                enfrentament = ui.Enfrentament;
            }
            else if (sender.ToString().Equals("NBA.View.UIEnfrentamentsEastern"))
            {
                UIEnfrentamentsEastern ui = (UIEnfrentamentsEastern)sender;
                conference = "Eastern";
                round = ui.Round;
                enfrentament = ui.Enfrentament;
            }
            else if (sender.ToString().Equals("NBA.View.UIEnfrentamentsWestern"))
            {
                UIEnfrentamentsWestern ui = (UIEnfrentamentsWestern)sender;
                conference = "Western";
                round = ui.Round;
                enfrentament = ui.Enfrentament;
            }

            if (cbxNbaSeason.SelectedItem != null)
            {
                Season s = (Season)cbxNbaSeason.SelectedItem;
                plays = PlayOffDB.GetLlistaSeasons(s.Caption, round, conference);
                if (plays.Count > 0 && enfrentament < plays.Count)
                {
                    EnfrontamentSeleccionat(s, plays[enfrentament], round);
                }
            }
        }


    }
}
