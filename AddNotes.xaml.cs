using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static NotesApp.MainWindow;

namespace NotesApp
{
    public partial class AddNotes : Window
    {
        public AddNotes()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            if (TodoPicker.SelectedDate == null)
            {
                MessageBox.Show("Вы не выбрали дату");
            }
            else
            {
                DialogResult = true;
                Close();
            }
        }
    }
}
