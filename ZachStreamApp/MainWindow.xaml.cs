using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Forms;

using MessageBox = System.Windows.MessageBox;
using Application = System.Windows.Application;

namespace ZachStreamApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
    {
        /// <summary>
        /// File path used if the user wants to save the current session
        /// </summary>
        private string SaveFilePath;
        private string filePath;
        #region Properties

        public string FilePath
        {
            get
            {
                return filePath;
            }

            set
            {
                filePath = value;
                this.OnPropertyChanged("FilePath");
            }
        }

        #endregion Properties

        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            InitializeComponent();

            Application.Current.MainWindow = this;

            // Set file path to empty string at the start
            FilePath = this.SaveFilePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }


        private void AddNewRowButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (FilePath.Equals(string.Empty))
            {
                MessageBox.Show("Please set the file path in \"Settings\" -> \"File Location\" first.");
                return;
            }

            NewRowWindow newRowWindow = new NewRowWindow
            {
                Owner = Window.GetWindow(this)
            };
            newRowWindow.CreateNewFileEvent += this.CreateNewTextFile;

            newRowWindow.ShowDialog();
        }

        /// <summary>
        /// Creates a new text file with the provided information
        /// </summary>
        /// <param name="fileName">name of the file to be created</param>
        /// <param name="value">value to put inside the file</param>
        private void CreateNewTextFile(string fileName, int value)
        {
            string fullFilePath = Utils.GetFullFilePath(FilePath, fileName);

            // Try to create the new file
            try
            {
                // Error out if file exists
                // TODO: Can change logic to overwrite the file
                if (File.Exists(fullFilePath))
                {
                    if (MessageBox.Show(
                            string.Format("File {0} already exists. Do you want to overwrite the file?", fullFilePath),
                            "Question",
                            MessageBoxButton.YesNo,
                            MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        // Want to overwrite the file so delete the existing one;
                        File.Delete(fullFilePath);
                    }
                    else
                    {
                        // Do no overwrite so stop right here and exit early 
                        // without adding new entry or creating file
                        return;
                    }
                }

                // Create the file.
                using (FileStream fs = File.Create(fullFilePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(value.ToString());

                    // Add the value to the file
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception e)
            {
                // Show error message and return early
                MessageBox.Show("Error during file creation: " + e.Message);
                return;
            }

            SampleRow newRow = new SampleRow(fileName, value);
            newRow.DeleteRowEvent += this.DeleteRow;

            // If file created successfully, add new row to main app view
            // TODO maybe change logic to binding
            this.RowsList.Items.Add(newRow);
        }

        /// <summary>
        /// Delete the row from our list
        /// </summary>
        /// <param name="name">Name of the file we want to delete</param>
        private void DeleteRow(string name)
        {
            // Go through our rows and delete the one with this file name
            foreach (object o in this.RowsList.Items)
            {
                SampleRow row = o as SampleRow;

                // TODO this case shouldn't happen but should log if it does
                if (row == null)
                {
                    continue;
                }

                if (row.FileName.Equals(name))
                {
                    this.RowsList.Items.Remove(row);

                    // We need to return at this point since enumeration cannot continue
                    // after an item has been removed. This is fine though since we should only
                    // have 1 item of that file name in the list
                    return;
                }
            }
        }

        /// <summary>
        /// When "File Path" menu option is clicked, open a file browser dialog to set the selected file path
        /// </summary>
        /// <param name="sender">Not Used</param>
        /// <param name="e">Not Used</param>
        private void FilePathMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                FilePath = fbd.SelectedPath;
            }
        }

        private void OpenMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            

        }

        private void SaveMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";

            // Show save file dialog box
            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Save document
                string filename = dlg.FileName;
            }
        }
        public void OnPropertyChanged(string PropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }
}
