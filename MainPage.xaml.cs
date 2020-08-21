using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Pour plus d'informations sur le modèle d'élément Page vierge, consultez la page https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SaisonCSS
{
    /// <summary>
    /// Une page vide peut être utilisée seule ou constituer une page de destination au sein d'un frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Dictionary<string, string> editDictionary = new Dictionary<string, string>();
        public MainPage()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.BackgroundColor = Windows.UI.Color.FromArgb(255, 69, 69, 144);
            titleBar.ButtonBackgroundColor = Windows.UI.Color.FromArgb(255, 69, 69, 144);
            titleBar.ButtonForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonHoverBackgroundColor = Windows.UI.Color.FromArgb(255, 79, 79, 164);
            titleBar.ButtonHoverForegroundColor = Windows.UI.Colors.White;
            titleBar.ButtonPressedBackgroundColor = Windows.UI.Color.FromArgb(255, 110, 110, 255);
            titleBar.ButtonPressedForegroundColor = Windows.UI.Colors.White;
            this.InitializeComponent();
            if (globalEdit.Children.Contains(editAccent))
            {
                globalEdit.Children.Remove(editAccent);
            }
        }

        private async void generatePressed(object sender, RoutedEventArgs e)
        {
            editDictionary.Add("accentColor", accentColorPicker.Color.ToString().Replace("#FF","#"));
            Generator generator = new Generator(listView.Items,editDictionary);
        }

        private void editClick(object sender, RoutedEventArgs e)
        {
            if (!globalEdit.Children.Contains(editAccent))
            {
                globalEdit.Children.Add(editAccent);
            }
        }

        private void closeEdit(object sender, RoutedEventArgs e)
        {
            if (globalEdit.Children.Contains(editAccent))
            {
                globalEdit.Children.Remove(editAccent);
            }
        }
    }
}
