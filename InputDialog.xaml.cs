using System;
using System.Collections.Generic;
using System.IO.Packaging;
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
using System.Windows.Shapes;

namespace ComputerShop
{
 
    public partial class InputDialog : Window
    {
        //public string InputText {  get; set; }

        //public string Response
        //{
        //    get { return inputTextBox.Text; }
        //    set { inputTextBox.Text = value; }
        //}

        //public InputDialog(string demand)
        //{
        //    InitializeComponent();
        //    this.Title = demand;
        //}

        //private void okButton_Click(object sender, RoutedEventArgs e)
        //{
        //    InputText = inputTextBox.Text;
        //    DialogResult = true;
        //}

        //private void cancelButton_Click(object sender, RoutedEventArgs e)
        //{
        //    DialogResult = false;
        //}

        private List<TextBox> textBoxes = new List<TextBox>();

        public InputDialog(List<string> fields)
        {
            InitializeComponent();

            for (int i = 0; i < fields.Count; i++)
            {
                RowDefinition row = new RowDefinition { Height = GridLength.Auto };
                inputGrid.RowDefinitions.Add(row);

                Grid overlayGrid = new Grid();
                TextBox textBox = new TextBox { Name = fields[i], Margin = new Thickness(0, 0, 10, 0) };
                TextBlock placeholderTextBlock = new TextBlock { Text = fields[i], Margin = new Thickness(5, 13, 0, 0), Foreground = Brushes.Gray };

                textBox.TextChanged += (sender, e) =>
                {
                    placeholderTextBlock.Visibility = string.IsNullOrEmpty(textBox.Text) ? Visibility.Visible : Visibility.Hidden;
                };

                overlayGrid.Children.Add(textBox);
                overlayGrid.Children.Add(placeholderTextBlock);

                Grid.SetRow(overlayGrid, i);
                inputGrid.Children.Add(overlayGrid);

                textBoxes.Add(textBox);
            }
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        public List<string> GetResponses()
        {
            return textBoxes.Select(textBox => textBox.Text).ToList();
        }
    }
}
