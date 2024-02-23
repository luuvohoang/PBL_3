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

    public partial class DeleteDialog : Window
    {
        public string InputText { get; set; }

        public string Response
        {
            get { return inputTextBox.Text; }
            set { inputTextBox.Text = value; }
        }

        public DeleteDialog(string demand) 
        {
            InitializeComponent();
            this.Title = demand;
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            InputText = inputTextBox.Text;
            DialogResult = true;
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
