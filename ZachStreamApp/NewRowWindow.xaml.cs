using System.Windows;

namespace ZachStreamApp
{
    using System.Windows.Input;

    /// <summary>
    /// Interaction logic for NewRowWindow.xaml
    /// </summary>
    public partial class NewRowWindow : Window
    {
        internal delegate void CreateNewFileHandler(string fileName, int value);

        /// <summary>
        /// Event passed back to main window signaling it to create a new file entry
        /// </summary>
        internal event CreateNewFileHandler CreateNewFileEvent;

        public NewRowWindow()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Cancel button closes the window without taking action 
        /// </summary>
        /// <param name="sender">Not Used</param>
        /// <param name="e">Not Used</param>
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// If Ok button is clicked signal 
        /// </summary>
        /// <param name="sender">Not Used</param>
        /// <param name="e">Not Used</param>
        private void OkButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (this.IsValidInput())
            {
                if (this.CreateNewFileEvent != null)
                {
                    this.CreateNewFileEvent(
                        this.FileNameTextBox.Text,
                        Utils.ParseInputInteger(this.InitialValueTextBox.Text));
                }

                this.Close();
            }
        }

        /// <summary>
        /// Make sure the text boxes entries are valid
        /// If not, display appropriate message box
        /// </summary>
        /// <returns>True if input is valid, false otherwise</returns>
        private bool IsValidInput()
        {
            if (this.FileNameTextBox.Text == string.Empty)
            {
                MessageBox.Show("File name is empty");
                return false;
            }

            if (this.InitialValueTextBox.Text == string.Empty)
            {
                MessageBox.Show("Initial value is empty");
                return false;
            }
            else if (!Utils.IsInputInteger(this.InitialValueTextBox.Text))
            {
                MessageBox.Show("Initial value must be a valid integer number.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Check key pressed.
        /// If Enter key is pressed, simulate "Ok" button click.
        /// If Esc key is pressed, simulate "Cancel" button click.
        /// </summary>
        /// <param name="sender">Not Used</param>
        /// <param name="e">Not Used</param>
        private void NewRowWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.OkButton_OnClick(this, null);
            }
            else if (e.Key == Key.Escape)
            {
                this.CancelButton_OnClick(this, null);
            }
        }
    }
}
