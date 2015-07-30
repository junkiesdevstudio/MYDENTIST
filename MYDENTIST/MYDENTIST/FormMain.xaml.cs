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
using System.Windows.Shapes;
using MYDENTIST.Form;

namespace MYDENTIST
{

    public partial class FormMain : Window
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnKaryawan_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormKaryawan inputGrid = new FormKaryawan();
            UIPanel.Children.Add(inputGrid);
        }

        private void btnObat_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormObat inputGrid = new FormObat();
            UIPanel.Children.Add(inputGrid);
        }
    }
}
