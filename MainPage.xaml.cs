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

namespace SaisonCSS
{
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

        private void generatePressed(object sender, RoutedEventArgs e)
        {
            editDictionary.Add("accentColor", accentColorPicker.Color.ToString().Replace("#FF","#"));
            Generator generator = new Generator(listView.Items,editDictionary);
        }

        private void editClick(object sender, RoutedEventArgs e)
        {
            if (globalEdit.Children.Count==0)                               //Check if no edit popup is present
            {
                if (sender.Equals(editAccentBtn))
                {
                    globalEdit.Children.Add(editAccent);
                }
                else if (false)
                {

                }
            }
            if (applicationPage.Children.Contains(toolbarBottom))           //Hide the toolbar at the bottom edge
            {
                applicationPage.Children.Remove(toolbarBottom);
            }
        }

        private void closeEdit(object sender, RoutedEventArgs e)
        {
            if (globalEdit.Children.Count != 0)                             //Check if any edit popup is present
            {
                if (sender.Equals(editAccentClose))
                {
                    globalEdit.Children.Remove(editAccent);
                }
                else if (false)
                {

                }
            }
            if (!applicationPage.Children.Contains(toolbarBottom))          //Show the toolbar at the bottom edge
            {
                applicationPage.Children.Add(toolbarBottom);
            }
        }
    }
}
