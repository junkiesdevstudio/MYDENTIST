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
    /// opone
    public partial class FormMain : Window
    {
        public FormMain()
        {
            InitializeComponent();
        }

        private void btnKaryawan_Click(object sender, RoutedEventArgs e)
        {
            FormKaryawan inputGrid = new FormKaryawan();
            UIPanel.Children.Add(inputGrid);
        }
    }
}
