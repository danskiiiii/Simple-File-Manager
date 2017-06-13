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
using System.Diagnostics;
using Simple_Manager.DataModels;

namespace Simple_Manager.UserControls
{
    /// <summary>
    /// Interaction logic for DiskElementView.xaml
    /// </summary>
    public partial class DiskElementView : UserControl
    {
        DiskElement diskElement;
        public DiskElementView(DiskElement diskElement)
        {
            this.diskElement = diskElement;

            InitializeComponent();

            /// <summary>
            /// Code for customizing UIElement depending on type (file or directory)
            /// </summary>
            var converter = new System.Windows.Media.BrushConverter();
            
            if (diskElement is MyDirectory)
            {               
               additionalInfo.Content = "<DIR>";
                open_button.Content = "Przejdź";
                open_button.Background = (Brush)converter.ConvertFromString("#ffffb3");
                preview_button.Visibility = Visibility.Collapsed;               
            }
            if (diskElement is MyFile)
            {
                string ext = ((MyFile)diskElement).Extension;
                additionalInfo.Content = "  "+ext;
                open_button.Content = "Uruchom";                
                open_button.Background = (Brush)converter.ConvertFromString("#80ff80");
                preview_button.Background = (Brush)converter.ConvertFromString("#ccffcc");
                preview_button.Visibility = Visibility.Collapsed;
                if (ext == ".jpg" || ext == ".png")
                {
                    preview_button.Visibility = Visibility.Visible;
                    preview_button.Tag = "image";
                }
                if (ext == ".txt" || ext == ".ini"|| ext==".tsv") 
                {
                    preview_button.Visibility = Visibility.Visible;
                    preview_button.Tag = "text";
                }

            }
            name.Content = diskElement.Name;
            path.Content = diskElement.Path;
            path.Visibility = Visibility.Collapsed;
            creationTime.Content =diskElement.CreationTime;
        }

     /// <summary>
     ///Code for changing location to a subdirectory of UIElement (if it's a directory type) or running process (if it's a file)
     /// </summary>

        public delegate void OpenSubLeft(DiskElement diskElement);
        public event OpenSubLeft openSubLeft;

        public delegate void OpenSubRight(DiskElement diskElement);
        public event OpenSubRight openSubRight;

        private void open_button_Click(object sender, RoutedEventArgs e)
        {

            if (diskElement is MyDirectory )
            {
                if (openSubLeft != null )
                {
                    openSubLeft.Invoke(diskElement);
                }
                if (openSubRight != null)
                {
                    openSubRight.Invoke(diskElement);
                }
            }
            if (diskElement is MyFile)
            {
                try
                { Process.Start(diskElement.Path); }
                catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: " + Environment.NewLine + Convert.ToString(ex)); }
            }
        }

        /// <summary>
        ///Code for deleting files and directories
        /// </summary>

        public delegate void FileDeletedEvent(DiskElement diskElement);
        public event FileDeletedEvent fileDeleted;

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
          try{              
              if (MessageBox.Show("Czy napewno chcesz usunąć "+diskElement.Name +"?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    if (diskElement is MyDirectory)
                    {
                        System.IO.DirectoryInfo dir = new DirectoryInfo(diskElement.Path);
                        dir.Delete(true);
                    }

                    if (diskElement is MyFile)
                    { System.IO.File.Delete(diskElement.Path); }

                    if (fileDeleted != null)
                    {
                        fileDeleted.Invoke(diskElement);
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: " + Environment.NewLine + Convert.ToString(ex)); }
        }

        /// <summary>
        ///Code for selecting UIElement items 
        /// </summary>
        public delegate void CheckedChangedEvent();
        public event CheckedChangedEvent chechedChange;

        private void checkBox_Checked(object sender, RoutedEventArgs e)
        {
        }

        private void checkBox_Click(object sender, RoutedEventArgs e)
        {
            if (chechedChange != null)
            {
                chechedChange.Invoke();

                var converter = new System.Windows.Media.BrushConverter();
                if(checkBox.IsChecked.Value )
                ucGrid.Background = (Brush)converter.ConvertFromString("#99bbff");
                if (! checkBox.IsChecked.Value)
                ucGrid.Background = (Brush)converter.ConvertFromString("#cce5ff");

            }
        }

        /// <summary>
        ///Code for previewing UIElement  
        /// </summary>
        public delegate void PreviewContentLeft(DiskElement diskElement);
        public event PreviewContentLeft previewContentLeft;

        public delegate void PreviewContentRight(DiskElement diskElement);
        public event PreviewContentRight previewContentRight;

        private void preview_button_Click(object sender, RoutedEventArgs e)
        {                       
                if (previewContentLeft != null)
                {
                    previewContentLeft.Invoke(diskElement);
                }
                if (previewContentRight != null)
                {
                    previewContentRight.Invoke(diskElement); 
                }
            
        }
    }
}

