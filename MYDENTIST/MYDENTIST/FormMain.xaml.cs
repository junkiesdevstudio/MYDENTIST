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
    /// <summary>
    /// Interaction logic for FormMain.xaml
    /// </summary>
    /// assssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssus
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

        private void btnPasien_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormPasien inputGrid = new FormPasien();
            UIPanel.Children.Add(inputGrid);
        }

        private void btnTerapi_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormTerapi inputGrid = new FormTerapi();
            UIPanel.Children.Add(inputGrid);
        }
    }
}
