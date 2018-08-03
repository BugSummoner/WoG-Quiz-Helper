using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.RegularExpressions;

namespace WoG_Quiz_Helper
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Title = "WoG Quiz Helper";
        }

        private void weaponName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox weaponNameTB = sender as TextBox;
            List<FileInfo> matches = FindMatches(weaponNameTB.Text);
            Uri imageUri = FindAccurateMatch(matches, weaponNameTB.Text);
            LoadImage(imageUri);


        }

        private List<FileInfo> FindMatches(string name)
        {
            List<FileInfo> matches = new List<FileInfo>();

            name = name.ToLower();
            name = name.Replace("_", "");
            name = name.Replace("-", "");
            name = name.Replace(" ", "");
            name = name.Replace("/", "");
            name = name.Trim();

            DirectoryInfo pics = new DirectoryInfo("Pics");

            foreach (var item in pics.GetFiles())
            {
                string itemName = item.Name;
                itemName = itemName.ToLower();
                itemName = itemName.Replace("_", "");
                itemName = itemName.Replace("-", "");
                itemName = itemName.Replace(" ", "");
                itemName = itemName.Replace(item.Extension, "");
                itemName = itemName.Trim();

                //Machtes raussuchen und dann prüfen ob einer 100% hat und anzeigen, ansonsten erste match 

                if (Regex.IsMatch(itemName, name) & name != "" & name != ".")
                {
                    matches.Add(item);
                }

            }

            return matches;
        }

        private Uri FindAccurateMatch(List<FileInfo> matches, string name)
        {

            Uri uriPath = null;
            DirectoryInfo pics = new DirectoryInfo("Pics");

            if (matches.Count == 0)
            {
                uriPath = new Uri(pics.GetFiles("wog_logo.png").First().FullName);
            }
            else if (matches.Count == 1)
            {
                uriPath = new Uri(matches[0].FullName);
            }
            else
            {
                name = name.ToLower();
                name = name.Replace("_", "");
                name = name.Replace("-", "");
                name = name.Replace(" ", "");
                //name = name.Replace("/", "");
                name = name.Trim();

                foreach (var item in matches)
                {
                    string itemName = item.Name;
                    itemName = itemName.ToLower();
                    itemName = itemName.Replace("_", "");
                    itemName = itemName.Replace("-", "");
                    itemName = itemName.Replace(" ", "");
                    itemName = itemName.Replace(item.Extension, "");
                    itemName = itemName.Trim();

                    if (itemName == name)
                    {
                        uriPath = new Uri(item.FullName);
                    }
                }
            }

            if (uriPath == null)
            {
                uriPath = new Uri(matches[0].FullName);
            }

            return uriPath;
        }

        private void LoadImage(Uri imageUri)
        {
            weaponPic.Source = new BitmapImage(imageUri);
        }
    }
}
