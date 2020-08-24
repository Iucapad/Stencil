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
        private string styleToWrite = "", scriptToWrite = "";
        private ItemCollection selectedList;
        private Dictionary<string, string> editDictionary;

        public Generator(ItemCollection items,Dictionary<string, string> editDictionary)
        {
            this.selectedList = items;
            this.editDictionary = editDictionary;
            GetProperties();
        }
        private async void GetProperties()
        {
            string jsonString = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/saisoncss.JSON")));
            var properties = JsonObject.Parse(jsonString);
            string jsonString2 = await FileIO.ReadTextAsync(await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/saisonjs.JSON")));
            var scriptJson = JsonObject.Parse(jsonString2);

            styleToWrite += properties["startDocument"].GetString();

            foreach (StackPanel stack in selectedList.OfType<StackPanel>())                                     //Will look in the ListView for StackPanels
            {
                foreach (CheckBox property in stack.Children.OfType<CheckBox>())                                //Will look in the StackPanels for Checkboxes
                {
                    switch (property.Name)                                                                      //Switch between properties and if they are checked, add the appropriate CSS code
                    {
                        case "accentColor":
                            if (property.IsChecked == true)
                            {
                                styleToWrite += "--accent-color:" + editDictionary["accentColor"] + ";--accent-soft:"+ editDictionary["accentColor"]+"66;}";      //Write the user-edited value stored in the Dictionary or the default color
                            }
                            else
                            {
                                styleToWrite += properties[property.Name].GetString();     
                            }
                            break;
                        case "uiTabs":
                            styleToWrite += properties[property.Name].GetString();
                            styleToWrite += properties["uiTabs_"+editDictionary["uiTabs"]].GetString();
                            scriptToWrite += scriptJson[property.Name].GetString() + "\n";
                            break;
                        default:
                            if (property.IsChecked == true)
                            {
                                styleToWrite += properties[property.Name].GetString() + "\n";
                                if (scriptJson.ContainsKey(property.Name))
                                {
                                    scriptToWrite += scriptJson[property.Name].GetString() + "\n";
                                }                                
                            }
                            break;
                    }
                }
            }
            editDictionary.Clear();
            ContentDialog dialog = new ContentDialog { Title = "Success", Content = "Generation completed.", CloseButtonText = "Ok" };
        
            FolderPicker folderPicker = new FolderPicker();
            folderPicker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            folderPicker.FileTypeFilter.Add("*");
            StorageFolder folder = await folderPicker.PickSingleFolderAsync();
            SaveFile(folder);
            dialog.ShowAsync();
        }
        private async void SaveFile(StorageFolder folder)
        {
            string filename = "saisoncss.css";
            //string scripts = "saisoncss.js";
            StorageFile styleFile = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
            await FileIO.WriteTextAsync(styleFile, styleToWrite.ToString());
            //StorageFile scriptFile = await folder.CreateFileAsync(scripts, CreationCollisionOption.ReplaceExisting);
            //await FileIO.WriteTextAsync(scriptFile, scriptToWrite.ToString());
        }
    }
}
