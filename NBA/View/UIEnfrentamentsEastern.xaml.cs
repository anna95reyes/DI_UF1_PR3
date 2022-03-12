using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// La plantilla de elemento Control de usuario está documentada en https://go.microsoft.com/fwlink/?LinkId=234236

namespace NBA.View
{
    public sealed partial class UIEnfrentamentsEastern : UserControl
    {
        public UIEnfrentamentsEastern()
        {
            this.InitializeComponent();
        }

        public event EventHandler Click;

        public Boolean EsGuanyador
        {
            get { return (Boolean)GetValue(EsGuanyadorProperty); }
            set { SetValue(EsGuanyadorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EsGuanyador.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EsGuanyadorProperty =
            DependencyProperty.Register("EsGuanyador", typeof(Boolean), typeof(UIEnfrentamentsEastern), new PropertyMetadata(false, EnfrentamentsEasternChangedCallbackStatic));


        public String LogoTeam
        {
            get { return (String)GetValue(LogoTeamProperty); }
            set { SetValue(LogoTeamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LogoTeam.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogoTeamProperty =
            DependencyProperty.Register("LogoTeam", typeof(String), typeof(UIEnfrentamentsEastern), new PropertyMetadata(null, EnfrentamentsEasternChangedCallbackStatic));




        public int SeedTeam
        {
            get { return (int)GetValue(SeedTeamProperty); }
            set { SetValue(SeedTeamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SeedTeam.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeedTeamProperty =
            DependencyProperty.Register("SeedTeam", typeof(int), typeof(UIEnfrentamentsEastern), new PropertyMetadata(0, EnfrentamentsEasternChangedCallbackStatic));




        public int WinsTeam
        {
            get { return (int)GetValue(WinsTeamProperty); }
            set { SetValue(WinsTeamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WinsTeam.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WinsTeamProperty =
            DependencyProperty.Register("WinsTeam", typeof(int), typeof(UIEnfrentamentsEastern), new PropertyMetadata(0, EnfrentamentsEasternChangedCallbackStatic));

        public int Round
        {
            get { return (int)GetValue(RoundProperty); }
            set { SetValue(RoundProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Round.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RoundProperty =
            DependencyProperty.Register("Round", typeof(int), typeof(UIEnfrentamentsEastern), new PropertyMetadata(0));



        public int Enfrentament
        {
            get { return (int)GetValue(EnfrentamentProperty); }
            set { SetValue(EnfrentamentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Enfrentament.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnfrentamentProperty =
            DependencyProperty.Register("Enfrentament", typeof(int), typeof(UIEnfrentamentsEastern), new PropertyMetadata(0));



        private static void EnfrentamentsEasternChangedCallbackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIEnfrentamentsEastern ee = (UIEnfrentamentsEastern)d;
            ee.EnfrentamentsEasternChangedCallback(d, e);
        }

        private void EnfrentamentsEasternChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (LogoTeam != null && SeedTeam != 0)
            {
                txbSeedEquip.Text = SeedTeam.ToString();
                imgLogoTeam.UriSource = new Uri(LogoTeam);
                txbPuntuacioEquip.Text = WinsTeam.ToString();

                if (EsGuanyador)
                {
                    brdEquip.Background = new SolidColorBrush(Color.FromArgb(100, 234, 153, 153));
                    txbPuntuacioEquip.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    brdEquip.Background = new SolidColorBrush(Color.FromArgb(100, 125, 198, 125));
                    txbPuntuacioEquip.Foreground = new SolidColorBrush(Colors.White);
                }
            }
            else
            {
                txbSeedEquip.Text = "";
                imgLogoTeam.UriSource = null;
                txbPuntuacioEquip.Text = "";
                brdEquip.Background = new SolidColorBrush(Colors.Transparent);
            }


        }

        private void btnUI_Click(object sender, RoutedEventArgs e)
        {
            Click?.Invoke(this, new EventArgs());
        }

    }
}
