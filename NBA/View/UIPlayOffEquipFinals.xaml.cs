﻿using System;
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
    public sealed partial class UIPlayOffEquipFinals : UserControl
    {
        public UIPlayOffEquipFinals()
        {
            this.InitializeComponent();
        }



        public Boolean EsGuanyador
        {
            get { return (Boolean)GetValue(EsGuanyadorProperty); }
            set { SetValue(EsGuanyadorProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EsGuanyador.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EsGuanyadorProperty =
            DependencyProperty.Register("EsGuanyador", typeof(Boolean), typeof(UIPlayOffEquipFinals), new PropertyMetadata(false, PlayOffEquipFinalsChangedCallbackStatic));



        public String NomTeam
        {
            get { return (String)GetValue(NomTeamProperty); }
            set { SetValue(NomTeamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for NomTeam.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty NomTeamProperty =
            DependencyProperty.Register("NomTeam", typeof(String), typeof(UIPlayOffEquipFinals), new PropertyMetadata(null, PlayOffEquipFinalsChangedCallbackStatic));




        public String LogoTeam
        {
            get { return (String)GetValue(LogoTeamProperty); }
            set { SetValue(LogoTeamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LogoTeam.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LogoTeamProperty =
            DependencyProperty.Register("LogoTeam", typeof(String), typeof(UIPlayOffEquipFinals), new PropertyMetadata(null, PlayOffEquipFinalsChangedCallbackStatic));




        public int SeedTeam
        {
            get { return (int)GetValue(SeedTeamProperty); }
            set { SetValue(SeedTeamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SeedTeam.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SeedTeamProperty =
            DependencyProperty.Register("SeedTeam", typeof(int), typeof(UIPlayOffEquipFinals), new PropertyMetadata(0, PlayOffEquipFinalsChangedCallbackStatic));




        public int WinsTeam
        {
            get { return (int)GetValue(WinsTeamProperty); }
            set { SetValue(WinsTeamProperty, value); }
        }

        // Using a DependencyProperty as the backing store for WinsTeam.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty WinsTeamProperty =
            DependencyProperty.Register("WinsTeam", typeof(int), typeof(UIPlayOffEquipFinals), new PropertyMetadata(0, PlayOffEquipFinalsChangedCallbackStatic));




        private static void PlayOffEquipFinalsChangedCallbackStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UIPlayOffEquipFinals poef = (UIPlayOffEquipFinals)d;
            poef.PlayOffEquipFinalsChangedCallback(d, e);
        }

        private void PlayOffEquipFinalsChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (NomTeam != null && LogoTeam != null && SeedTeam != 0)
            {
                txbNomEquip.Text = NomTeam;
                txbSeedEquip.Text = "("+SeedTeam.ToString()+")";
                imgLogoTeam.UriSource = new Uri(LogoTeam);
                txbPuntuacioEquip.Text = WinsTeam.ToString();

                if (EsGuanyador)
                {
                    brdEquip.Background = new SolidColorBrush(Color.FromArgb(100, 234, 153, 153));
                    txbPuntuacioEquip.Foreground = new SolidColorBrush(Colors.White);
                }
                else
                {
                    brdEquip.Background = new SolidColorBrush(Colors.Transparent);
                    txbPuntuacioEquip.Foreground = new SolidColorBrush(Colors.Black);
                }
            } 
            else
            {
                txbNomEquip.Text = "";
                txbSeedEquip.Text = "";
                imgLogoTeam.UriSource = null;
                txbPuntuacioEquip.Text = "";
            }


        }
    }
}
