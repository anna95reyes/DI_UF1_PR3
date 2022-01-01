using NBA.View;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using Windows.Devices.Geolocation;
using Windows.UI.Xaml.Controls.Maps;

// La plantilla de elemento Página en blanco está documentada en https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0xc0a

namespace NBA
{
    /// <summary>
    /// Página vacía que se puede usar de forma independiente o a la que se puede navegar dentro de un objeto Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            copiarImatgesAppDataAsync();
            frmPrincipal.Navigate(typeof(TeamEditorPage));
            nviTeamEditor.IsSelected = true;
        }

        private async void copiarImatgesAppDataAsync()
        {
            StorageFolder appInstalledFolder = Windows.ApplicationModel.Package.Current.InstalledLocation;
            StorageFolder assets = await appInstalledFolder.GetFolderAsync("Assets");
            StorageFolder appDataFolder = ApplicationData.Current.LocalFolder;
            IReadOnlyList<IStorageItem> files = await assets.GetItemsAsync();

            for (int i = 0; i < files.Count; i++)
            {
                copiarArxiusICarpetesAssets(files, i, appDataFolder);
            }   
        }

        private async void copiarArxiusICarpetesAssets(IReadOnlyList<IStorageItem> files, int index, StorageFolder appDataFolder)
        {
            if (files[index].IsOfType(StorageItemTypes.Folder))
            {
                StorageFolder folder = await appDataFolder.CreateFolderAsync(files[index].Name, CreationCollisionOption.OpenIfExists);
                IReadOnlyList<IStorageItem> f = await ((StorageFolder)files[index]).GetItemsAsync();
                for (int i = 0; i < f.Count; i++)
                {
                    if (files[index].Name != f[i].Name)
                    {
                        copiarArxiusICarpetesAssets(f, i, folder);
                    }
                }
            }
            else if (files[index].IsOfType(StorageItemTypes.File))
            {
                ((StorageFile)files[index]).CopyAsync(appDataFolder);
            }
        }

        private void NavigationView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (nviTeamEditor.Content.Equals(args.InvokedItem))
            {
                frmPrincipal.Navigate(typeof(TeamEditorPage));
            }
            else if (nviPlayOffBracket.Content.Equals(args.InvokedItem))
            {
                frmPrincipal.Navigate(typeof(PlayOffBracketPage));
            }
        }

    }
}
