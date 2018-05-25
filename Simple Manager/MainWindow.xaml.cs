        #region Namespaces Used

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
using Simple_Manager.DataModels;
using Simple_Manager.UserControls;
using Microsoft.VisualBasic;
namespace Simple_Manager
#endregion
 
        #region Code shared by both StackPanels
{
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {        
        

        /// <summary>
        /// Methods loaded on startup
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            LoadDrives();
            RefreshFilesList_left(leftDriveSelector.Text);
            RefreshFilesList_right(rightDriveSelector.Text);

        }

        private void LoadDrives()
        {            
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            foreach (DriveInfo dr in allDrives)
            {
                leftDriveSelector.Items.Add(dr.Name);
                leftDriveSelector.SelectedIndex = 0;
                rightDriveSelector.Items.Add(dr.Name);           
                rightDriveSelector.SelectedIndex = 0;
            }
        }

        private void DiskElementView_chechedChange()
        {
        }

        private void DiskElementViewFileDeleted(DiskElement diskElement)
        {
            RefreshFilesList_left(leftCurrentPath.Text);
            RefreshFilesList_right(rightCurrentPath.Text);
        }


        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// Search button interaction logic
        /// </summary>      
        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MyDirectory myDirectory2 = new MyDirectory(rightCurrentPath.Text);
                List<DiskElement> subElements2 = myDirectory2.GetSubDiskElements();
                rightStackPanel.Children.Clear();
                foreach (DiskElement diskElement in subElements2)
                {
                    DiskElementView diskElementView = new DiskElementView(diskElement);

                    if (Convert.ToString(diskElementView.name.Content).Contains(searchBox.Text))
                    { rightStackPanel.Children.Add(diskElementView); }

                    diskElementView.fileDeleted += DiskElementViewFileDeleted;
                    diskElementView.chechedChange += DiskElementView_chechedChange;
                    diskElementView.openSubRight += DiskElementView_openSubRight;
                    diskElementView.previewContentLeft += DiskElementViewPreviewContent_onLeft;
                }

                MyDirectory myDirectory1 = new MyDirectory(leftCurrentPath.Text);
                List<DiskElement> subElements1 = myDirectory1.GetSubDiskElements();
                leftStackPanel.Children.Clear();
                foreach (DiskElement diskElement in subElements1)
                {
                    DiskElementView diskElementView = new DiskElementView(diskElement);

                    if (Convert.ToString(diskElementView.name.Content).Contains(searchBox.Text))
                    { leftStackPanel.Children.Add(diskElementView); }

                    diskElementView.fileDeleted += DiskElementViewFileDeleted;
                    diskElementView.chechedChange += DiskElementView_chechedChange;
                    diskElementView.openSubRight += DiskElementView_openSubLeft;
                    diskElementView.previewContentRight += DiskElementViewPreviewContent_onRight;
                }
            }
            catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: " + Environment.NewLine + Convert.ToString(ex)); }
        }
        private void searchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        #endregion

        #region Left StackPanel Code

        /// <summary>
        /// Beginning of left panel  
        /// </summary>



        /// <summary>
        /// Method for auto refreshing left panel  
        /// </summary>
        private void RefreshFilesList_left(string text)
        {
          try  { MyDirectory myDirectory = new MyDirectory(text);
            leftCurrentPath.Text = myDirectory.Path;

            List<DiskElement> subElements = myDirectory.GetSubDiskElements();
            leftStackPanel.Children.Clear();
            foreach (DiskElement diskElement in subElements)
            {
                DiskElementView diskElementView = new DiskElementView(diskElement);
                leftStackPanel.Children.Add(diskElementView);

                diskElementView.fileDeleted += DiskElementViewFileDeleted;
                diskElementView.chechedChange += DiskElementView_chechedChange;
                diskElementView.openSubLeft += DiskElementView_openSubLeft;
                diskElementView.previewContentRight += DiskElementViewPreviewContent_onRight;
                } }
            catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: " + Environment.NewLine + Convert.ToString(ex)); }

        }


        /// <summary>
        /// Method for opening a subdirectory on left panel
        /// </summary>
        private void DiskElementView_openSubLeft(DiskElement diskElement)
        {
            try  { MyDirectory myDirectory = new MyDirectory(diskElement.Path);             
                    leftCurrentPath.Text = diskElement.Path;
                    List<DiskElement> subElements = myDirectory.GetSubDiskElements();
                    leftStackPanel.Children.Clear();

                    foreach (DiskElement subdiskElement in subElements)
                    {
                        DiskElementView diskElementView = new DiskElementView(subdiskElement);
                        leftStackPanel.Children.Add(diskElementView);

                        diskElementView.fileDeleted += DiskElementViewFileDeleted;
                        diskElementView.chechedChange += DiskElementView_chechedChange;
                        diskElementView.openSubLeft += DiskElementView_openSubLeft;
                        diskElementView.previewContentRight += DiskElementViewPreviewContent_onRight;
                    }             
                 }
            catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: " + Environment.NewLine + Convert.ToString(ex)); }
        }

        private void leftHomeButton_Click(object sender, RoutedEventArgs e)
        {
            RefreshFilesList_left(leftDriveSelector.Text); 
            
        }
        private void leftGoToPath_Click(object sender, RoutedEventArgs e)
        {
            RefreshFilesList_left(leftCurrentPath.Text);
        }
        private void left_currentPath_TextChanged(object sender, TextChangedEventArgs e)
        {
        }


        /// <summary>
        /// Method for sorting items in left panel by name 
        /// </summary>
        private void left_sortByName_Click(object sender, RoutedEventArgs e)
        {            
           try { 
                MyDirectory myDirectory = new MyDirectory(leftCurrentPath.Text);                

                List<DiskElement> subElements = myDirectory.GetSubDiskElements_byName();
                    leftStackPanel.Children.Clear();
                    foreach (DiskElement diskElement in subElements)
                {
                    DiskElementView diskElementView = new DiskElementView(diskElement);

                    leftStackPanel.Children.Add(diskElementView);

                    diskElementView.fileDeleted += DiskElementViewFileDeleted;
                    diskElementView.chechedChange += DiskElementView_chechedChange;
                    diskElementView.openSubLeft += DiskElementView_openSubLeft;
                    diskElementView.previewContentRight += DiskElementViewPreviewContent_onRight;


                }
            }
            catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: " + Environment.NewLine + Convert.ToString(ex)); }
        }

        /// <summary>
        /// Button sorting items in left panel by creation time 
        /// </summary>
        private void left_sortByDate_Click(object sender, RoutedEventArgs e)
        {
            {
                try { 
                MyDirectory myDirectory = new MyDirectory(leftCurrentPath.Text);                
                List<DiskElement> subElements = myDirectory.GetSubDiskElements_byDate();
                leftStackPanel.Children.Clear();
                foreach (DiskElement diskElement in subElements)
                {
                    DiskElementView diskElementView = new DiskElementView(diskElement);

                    leftStackPanel.Children.Add(diskElementView);

                    diskElementView.fileDeleted += DiskElementViewFileDeleted;
                    diskElementView.chechedChange += DiskElementView_chechedChange;
                    diskElementView.openSubLeft += DiskElementView_openSubLeft;
                    diskElementView.previewContentRight += DiskElementViewPreviewContent_onRight;

                    }
                }
                catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: " + Environment.NewLine + Convert.ToString(ex)); }

            }
        }
        /// <summary>
        /// Button changing location to parent directory
        /// </summary>
        private void left_parentDir_Click(object sender, RoutedEventArgs e)
        {
            try { RefreshFilesList_left(Directory.GetParent(leftCurrentPath.Text).FullName); }
            catch { MessageBox.Show("Brak Folderu Nadrzędnego!"); }
        }

        /// <summary>
        /// Method for previewing simple image and text files
        /// </summary>
        private void DiskElementViewPreviewContent_onLeft(DiskElement diskElement)
        {
            var converter = new System.Windows.Media.BrushConverter();

            DiskElementView diskElementView = new DiskElementView(diskElement);
           if( (Convert.ToString(diskElementView.preview_button.Tag)== "image"))
           { leftImgView.Source = new BitmapImage(new Uri(diskElement.Path));
                RefreshFilesList_left(leftCurrentPath.Text);
                leftStackPanel.Children.Insert(0,leftImgView);
                leftScroll.ScrollToTop();
            }
            if ((Convert.ToString(diskElementView.preview_button.Tag) == "text"))
            {
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(System.IO.File.ReadAllText(Convert.ToString(diskElement.Path)));
                FlowDocument document = new FlowDocument(paragraph);
                leftReader.Document = document;
                leftReader.Document.Background = (Brush)converter.ConvertFromString("white");
                RefreshFilesList_left(leftCurrentPath.Text);
                leftStackPanel.Children.Insert(0,leftReader);
                leftScroll.ScrollToTop();
            }

            /// <summary>
            /// Midsection button for copying selected items
            /// </summary>
        }
        private void leftCopyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (UIElement diskView in leftStackPanel.Children)
                {
                    if (((DiskElementView)diskView).checkBox.IsChecked.Value && Convert.ToString(((DiskElementView)diskView).additionalInfo.Content) == "<DIR>")
                    {
                        MyDirectory myDir = new MyDirectory(leftCurrentPath.Text);
                        myDir.DirectoryCopy((Convert.ToString(((DiskElementView)diskView).path.Content)), rightCurrentPath.Text + "\\" + (Convert.ToString(((DiskElementView)diskView).name.Content)), true);                        
                    }
                }

                foreach (UIElement diskView in leftStackPanel.Children)
                {
                    if (((DiskElementView)diskView).checkBox.IsChecked.Value && Convert.ToString(((DiskElementView)diskView).additionalInfo.Content) != "<DIR>")
                    {
                        System.IO.File.Copy(Convert.ToString(((DiskElementView)diskView).path.Content), rightCurrentPath.Text + "\\" + ((DiskElementView)diskView).name.Content, true);                        
                    }
                }
               RefreshFilesList_right(rightCurrentPath.Text);
               RefreshFilesList_left(leftCurrentPath.Text);

            }
            catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD:" + Environment.NewLine + Convert.ToString(ex)); }
        }

        /// <summary>
        /// Midsection button for creating a new directory 
        /// </summary>
        private void leftAddDir_Click(object sender, RoutedEventArgs e)
        {
            string newDir = "";
            newDir = Microsoft.VisualBasic.Interaction.InputBox("Folder zostanie utworzony w lokalizacji:" + Environment.NewLine + leftCurrentPath.Text + "\\", "Podaj nazwę folderu", newDir);
            System.IO.Directory.CreateDirectory(leftCurrentPath.Text + '\\' + newDir);
            RefreshFilesList_left(leftCurrentPath.Text);
            RefreshFilesList_right(rightCurrentPath.Text);
        }

        /// <summary>
        /// Midsection button for deleting selected items
        /// </summary>
        private void deleteSelectedOnLeft_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Czy napewno chcesz usunąć wybrane elementy?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    foreach (UIElement diskView in leftStackPanel.Children)

                    {
                        if (((DiskElementView)diskView).checkBox.IsChecked.Value && Convert.ToString(((DiskElementView)diskView).additionalInfo.Content) == "<DIR>")
                        {
                            System.IO.DirectoryInfo dir = new DirectoryInfo(Convert.ToString(((DiskElementView)diskView).path.Content));
                            dir.Delete(true);
                        }
                    }

                    foreach (UIElement diskView in leftStackPanel.Children)
                    {
                        if (((DiskElementView)diskView).checkBox.IsChecked.Value && Convert.ToString(((DiskElementView)diskView).additionalInfo.Content) != "<DIR>")
                        {
                            System.IO.File.Delete(Convert.ToString(((DiskElementView)diskView).path.Content));

                        }
                    }
                    RefreshFilesList_left(leftCurrentPath.Text);
                    if (Directory.Exists(rightCurrentPath.Text))
                    { RefreshFilesList_right(rightCurrentPath.Text); }
                    else { RefreshFilesList_right(rightDriveSelector.Text); }
                }
            }
            catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD:" + Environment.NewLine + Convert.ToString(ex)); }
        }

        #endregion

        #region Right StackPanel Code

        /// <summary>
        ///  Beginning of right panel - mostly a mirror image of left panel code
        /// </summary>


        /// <summary>
        /// Method for auto refreshing right panel  
        /// </summary>
        private void RefreshFilesList_right(string text)
        {
            try
            { MyDirectory myDirectory = new MyDirectory(text);
            rightCurrentPath.Text = myDirectory.Path;

            List<DiskElement> subElements = myDirectory.GetSubDiskElements();
            rightStackPanel.Children.Clear();
                foreach (DiskElement diskElement in subElements)
                {
                    DiskElementView diskElementView = new DiskElementView(diskElement);
                    rightStackPanel.Children.Add(diskElementView);

                    diskElementView.fileDeleted += DiskElementViewFileDeleted;
                    diskElementView.chechedChange += DiskElementView_chechedChange;
                    diskElementView.openSubRight += DiskElementView_openSubRight;
                    diskElementView.previewContentLeft += DiskElementViewPreviewContent_onLeft;
                }                             

            }
            catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: " + Environment.NewLine + Convert.ToString(ex)); }
        }

        /// <summary>
        /// Method for opening a subdirectory on right panel
        /// </summary>
        private void DiskElementView_openSubRight(DiskElement diskElement)
        {
            try
            {
                MyDirectory myDirectory = new MyDirectory(diskElement.Path);

                rightCurrentPath.Text = diskElement.Path;

                List<DiskElement> subElements = myDirectory.GetSubDiskElements();
                rightStackPanel.Children.Clear();
                foreach (DiskElement subdiskElement in subElements)
                {
                    DiskElementView diskElementView = new DiskElementView(subdiskElement);

                    rightStackPanel.Children.Add(diskElementView);

                    diskElementView.fileDeleted += DiskElementViewFileDeleted;
                    diskElementView.chechedChange += DiskElementView_chechedChange;
                    diskElementView.openSubRight += DiskElementView_openSubRight;
                    diskElementView.previewContentLeft += DiskElementViewPreviewContent_onLeft;
                }
            }
            catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: " + Environment.NewLine + Convert.ToString(ex)); }
        }

        /// <summary>
        /// Button for changing location to parent directory
        /// </summary>
        private void right_parentDir_Click(object sender, RoutedEventArgs e)
        {
            try { RefreshFilesList_right(Directory.GetParent(rightCurrentPath.Text).FullName); }
            catch { MessageBox.Show("Brak Folderu Nadrzędnego!"); }
        }     
        
        /// <summary>
        /// Button sorting items in right panel by name
        /// </summary>
        private void right_sortByName_Click(object sender, RoutedEventArgs e)
        {
            {
                try
                {
                    MyDirectory myDirectory = new MyDirectory(rightCurrentPath.Text);

                    List<DiskElement> subElements = myDirectory.GetSubDiskElements_byName();
                    rightStackPanel.Children.Clear();
                    foreach (DiskElement diskElement in subElements)
                    {
                        DiskElementView diskElementView = new DiskElementView(diskElement);
                       rightStackPanel.Children.Add(diskElementView);

                        diskElementView.fileDeleted += DiskElementViewFileDeleted;
                        diskElementView.chechedChange += DiskElementView_chechedChange;                        
                        diskElementView.openSubRight += DiskElementView_openSubRight;
                        diskElementView.previewContentLeft += DiskElementViewPreviewContent_onLeft;

                    }
                }
                catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: " + Environment.NewLine + Convert.ToString(ex)); }
            }
        }

        /// <summary>
        /// Button sorting items in right panel by creation time
        /// </summary>
        private void right_sortByDate_Click(object sender, RoutedEventArgs e)
        {            
                try
                {
                    MyDirectory myDirectory = new MyDirectory(rightCurrentPath.Text);
                    List<DiskElement> subElements = myDirectory.GetSubDiskElements_byDate();
                    rightStackPanel.Children.Clear();
                    foreach (DiskElement diskElement in subElements)
                    {
                        DiskElementView diskElementView = new DiskElementView(diskElement);

                       rightStackPanel.Children.Add(diskElementView);

                        diskElementView.fileDeleted += DiskElementViewFileDeleted;
                        diskElementView.chechedChange += DiskElementView_chechedChange;                        
                        diskElementView.openSubRight += DiskElementView_openSubRight;
                        diskElementView.previewContentLeft += DiskElementViewPreviewContent_onLeft;

                }
            }
                catch (Exception ex) { MessageBox.Show("WYSTĄPIŁ BŁĄD: "+Environment.NewLine + Convert.ToString(ex)); }

        }
       
        /// <summary>
        /// Button for changing location based on drive selected
        /// </summary>
        private void right_HomeButton_Click(object sender, RoutedEventArgs e)
        {
             RefreshFilesList_right(rightDriveSelector.Text);            
        }
        /// <summary>
        /// Button for changing location based on path typed in a textbox
        /// </summary>
        private void rightGoToPath_Click(object sender, RoutedEventArgs e)
        {
            RefreshFilesList_right(rightCurrentPath.Text);
        }
        private void right_currentPath_TextChanged(object sender, TextChangedEventArgs e)
        {  }
        private void right_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        { }

        /// <summary>
        /// Midsection button for copying selected items
        /// </summary>
        private void rightCopyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (UIElement diskView in rightStackPanel.Children)
                {
                    if (((DiskElementView)diskView).checkBox.IsChecked.Value && Convert.ToString(((DiskElementView)diskView).additionalInfo.Content) == "<DIR>")
                    {
                        MyDirectory myDir = new MyDirectory(rightCurrentPath.Text);
                        myDir.DirectoryCopy((Convert.ToString(((DiskElementView)diskView).path.Content)), leftCurrentPath.Text + "\\" + (Convert.ToString(((DiskElementView)diskView).name.Content)), true);                      
                    }
                }

                foreach (UIElement diskView in rightStackPanel.Children)
                {
                    if (((DiskElementView)diskView).checkBox.IsChecked.Value && Convert.ToString(((DiskElementView)diskView).additionalInfo.Content) != "<DIR>")
                    {
                        System.IO.File.Copy(Convert.ToString(((DiskElementView)diskView).path.Content), leftCurrentPath.Text + "\\" + ((DiskElementView)diskView).name.Content, true);                       
                    }
                }
                RefreshFilesList_right(rightCurrentPath.Text);
                RefreshFilesList_left(leftCurrentPath.Text);
            }
            catch (Exception ex) {MessageBox.Show("WYSTĄPIŁ BŁĄD:" + Environment.NewLine + Convert.ToString(ex)); }
        }
        /// <summary>
        /// Midsection button for creation a new directory
        /// </summary>
        private void rightAddDir_Click(object sender, RoutedEventArgs e)
        {
            string newDir = "";
            newDir = Microsoft.VisualBasic.Interaction.InputBox("Folder zostanie utworzony w lokalizacji:"+ Environment.NewLine + rightCurrentPath.Text+"\\", "Podaj nazwę folderu", newDir);
            System.IO.Directory.CreateDirectory(rightCurrentPath.Text + '\\' + newDir);
            RefreshFilesList_left(leftCurrentPath.Text);
            RefreshFilesList_right(rightCurrentPath.Text);
        }

        /// <summary>
        /// Method for previewing simple image and text files 
        /// </summary>
        private void DiskElementViewPreviewContent_onRight(DiskElement diskElement)
        {
            DiskElementView diskElementView = new DiskElementView(diskElement);
            if ((Convert.ToString(diskElementView.preview_button.Tag) == "image"))
            {
                rightImgView.Source = new BitmapImage(new Uri( diskElement.Path));
                RefreshFilesList_right(rightCurrentPath.Text);
                rightStackPanel.Children.Insert(0,rightImgView);
                rightScroll.ScrollToTop();               
            }

            if ((Convert.ToString(diskElementView.preview_button.Tag) == "text"))
            {
                var converter = new System.Windows.Media.BrushConverter();
                Paragraph paragraph = new Paragraph();
                paragraph.Inlines.Add(System.IO.File.ReadAllText(Convert.ToString(diskElement.Path)));               
                FlowDocument document = new FlowDocument(paragraph);
            
                rightReader.Document = document;
                rightReader.Document.Background = (Brush)converter.ConvertFromString("white");
                RefreshFilesList_right(rightCurrentPath.Text);
                rightStackPanel.Children.Insert(0,rightReader);
                rightScroll.ScrollToTop();
            }
        }
        /// <summary>
        /// Midsection button for deleting selected items 
        /// </summary>  
        private void deleteSelectedOnRight_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("Czy napewno chcesz usunąć wybrane elementy?", "Potwierdzenie usunięcia", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    foreach (UIElement diskView in rightStackPanel.Children)
                    {
                        if (((DiskElementView)diskView).checkBox.IsChecked.Value && Convert.ToString(((DiskElementView)diskView).additionalInfo.Content) == "<DIR>")
                        {
                            System.IO.DirectoryInfo dir = new DirectoryInfo(Convert.ToString(((DiskElementView)diskView).path.Content));
                            dir.Delete(true);
                        }
                    }
                    foreach (UIElement diskView in rightStackPanel.Children)
                    {
                        if (((DiskElementView)diskView).checkBox.IsChecked.Value && Convert.ToString(((DiskElementView)diskView).additionalInfo.Content) != "<DIR>")
                        {
                            System.IO.File.Delete(Convert.ToString(((DiskElementView)diskView).path.Content));
                        }
                    }
                    RefreshFilesList_right(rightCurrentPath.Text);
                    if (Directory.Exists(leftCurrentPath.Text))
                    { RefreshFilesList_left(leftCurrentPath.Text); }
                    else { RefreshFilesList_left(leftDriveSelector.Text); }
                }
            }
            catch (Exception ex)  { MessageBox.Show("WYSTĄPIŁ BŁĄD:" + Environment.NewLine + Convert.ToString(ex)); }            
        }
                
    }   
}
#endregion
