using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SaisonCSS
{
    class Generator
    {
        private string stringToWrite = "";
        private ItemCollection selectedList;
        private Dictionary<string, string> editDictionary;


        public Generator(ItemCollection items,Dictionary<string, string> editDictionary)
        {
            this.selectedList = items;
            this.editDictionary = editDictionary;
            getProperties();
        }

        private async void getProperties()
        {
            string jsonString = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/saisoncss.JSON")));
            var properties = JsonObject.Parse(jsonString);

            stringToWrite += properties["startDocument"].GetString();

            foreach (StackPanel stack in selectedList.OfType<StackPanel>())                                     //Will look in the ListView for StackPanels
            {
                foreach (CheckBox property in stack.Children.OfType<CheckBox>())                                //Will look in the StackPanels for Checkboxes
                {
                    switch (property.Name)                                                                      //Switch between properties and if they are checked, add the appropriate CSS code
                    {
                        case "accentColor":
                            if (property.IsChecked == true)
                            {
                                stringToWrite += "--accent-color:" + editDictionary["accentColor"] + ";}";      //Write the user-edited value stored in the Dictionary or the default color
                            }
                            else
                            {
                                stringToWrite += properties[property.Name].GetString();     
                            }
                            break;
                        default:
                            if (property.IsChecked == true)
                            {
                                stringToWrite += properties[property.Name].GetString() + "\n";
                            }
                            break;
                    }
                }
            }

            ContentDialog dialog = new ContentDialog { Title = "Success", Content = "Generation completed.", CloseButtonText = "Ok" };
        
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            saveFile(folder);
            dialog.ShowAsync();
        }
        private async void saveFile(StorageFolder folder)
        {
            string filename = "test.css";
            StorageFile f = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(f, stringToWrite.ToString());
        }
    }
}
