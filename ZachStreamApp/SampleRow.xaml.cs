using System;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ZachStreamApp
{
    /// <summary>
    /// Interaction logic for SampleRow.xaml
    /// </summary>
    public partial class SampleRow : INotifyPropertyChanged
    {
        internal delegate void DeleteRowHandler(string fileName);

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Event passed back to main window signaling it to create a new file entry
        /// </summary>
        internal event DeleteRowHandler DeleteRowEvent;

        #region Private Fields
        /// <summary>
        /// File name associated with the row. Do not include .txt in the name.
        /// </summary>
        private string fileName;
        private int value;
        private MainWindow mainWindow;
        #endregion Private Fields

        #region Properties
        public string FileName
        {
            get
            {
                return this.fileName;
            }

            set
            {
                this.fileName = value;
                this.OnPropertyChanged("FileName");
            }
        }

        public int Value
        {
            get
            {
                return this.value;
            }

            set
            {
                this.value = value;
                this.OnPropertyChanged("Value");
            }
        }
        #endregion Properties

        #region Constructor
        public SampleRow(string fileName, int value)
        {
            this.FileName = fileName;
            this.Value = value;
            this.mainWindow = Application.Current.MainWindow as MainWindow;

            this.DataContext = this;
            this.InitializeComponent();
        }
        #endregion Constructor

        #region Event Handlers
        public void OnPropertyChanged(string PropertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
        
        private void DecrementButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Value--;
        }

        private void IncrementButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Value++;
        }

        /// <summary>
        /// Updates the current 
        /// </summary>
        /// <param name="sender">Not Used</param>
        /// <param name="e">Not Used</param>
        private void UpdateButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.UpdateFile();
        }

        /// <summary>
        /// Delete button's on click.
        /// Delete the entry from the list as well as the file from disk
        /// </summary>
        /// <param name="sender">Not Used</param>
        /// <param name="e">Not Used</param>
        private void DeleteButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this file and entry?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                string fullFilePath = Utils.GetFullFilePath(this.mainWindow.FilePath, this.FileName);

                // Delete the entry and file
                try
                {
                    File.Delete(fullFilePath);

                    // Send event notification to delete row from list
                    if (this.DeleteRowEvent != null)
                    {
                        this.DeleteRowEvent(this.fileName);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format("Error: {0}", ex.Message));
                }
            }
        }

        /// <summary>
        /// Whenever the textbox value is changed, update the file
        /// </summary>
        /// <param name="sender">Not Used</param>
        /// <param name="e">Not Used</param>
        private void TextFileValueTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            this.UpdateFile();
        }
        #endregion Event Handlers

        /// <summary>
        /// Updates the file with the current value in textbox
        /// </summary>
        private void UpdateFile()
        {
            string fullFilePath = string.Empty;

            if (this.mainWindow != null)
            {
                fullFilePath = Utils.GetFullFilePath(this.mainWindow.FilePath, this.FileName);
            }
            
            // Update value in the text file
            try
            {
                File.WriteAllText(fullFilePath, this.Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error: {0}", ex.Message));
            }
        }
    }
}
