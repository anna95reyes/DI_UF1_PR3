using NBA_BD;
using NBA_BD.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
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


        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            
        }

        private void ContentDialog_Loaded(object sender, RoutedEventArgs e)
        {
            cbxCollageName.ItemsSource = CollegeDB.GetLlistaCollege();
            cbxCountryName.ItemsSource = CountryDB.GetLlistaCountry();
            if (elPlayer == null)
            {
                int a = 0;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            dialogPlayer.Hide();
        }

    }
}
