using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
using WolfSchafGrasSimulation.Class;

namespace WolfSchafGrasSimulation.Applications.mainWindow
{
    public partial class ClearableTextBox : UserControl
    {
        public ClearableTextBox()
        {
            InitializeComponent();
        }

        private string placeholder;

        public string Placeholder
        {
            get { return placeholder; }

            set
            {
                placeholder = value;
                tbPlaceholder.Text = placeholder;
            }
        }

        public string Text
        {
            get
            {
                return this.txtInput.Text;
            }
            set
            {
                this.txtInput.Text = value; 
            }
        }

        private void txtInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            tbPlaceholder.Visibility = string.IsNullOrEmpty(txtInput.Text) ? Visibility.Visible : Visibility.Hidden;            
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtInput.Clear();
            txtInput.Focus();
        }

        private void txtInput_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(IsTextNumeric(e.Text))
            {
                e.Handled = true;
            }
        }

        private bool IsTextNumeric(string text)
        {
            Regex regex = new Regex("[^0-9]+");
            return regex.IsMatch(text);
        }

        private void txtInput_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                if(IsTextNumeric(text))
                {
                    e.CancelCommand();
                }
            }

            else
            {
                e.CancelCommand();
            }
        }
    }
}
