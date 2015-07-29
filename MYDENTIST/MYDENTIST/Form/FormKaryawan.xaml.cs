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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MYDENTIST.Class;

namespace MYDENTIST.Form
{
    /// <summary>
    /// Interaction logic for FormKaryawan.xaml
    /// </summary>
    public partial class FormKaryawan : UserControl
    {
        public FormKaryawan()
        {
            InitializeComponent();
            List<cds_Karyawan> users = new List<cds_Karyawan>();
            users.Add(new cds_Karyawan() { IdKaryawan = 0, NamaKaryawan = "Sammy Doe", JenisKaryawan = "Dokter", AlamatKaryawan = "Sorowajan" });
            users.Add(new cds_Karyawan() { IdKaryawan = 0, NamaKaryawan = "Sammy Doe", JenisKaryawan = "Dokter", AlamatKaryawan = "Sorowajan" });
            dgUsers.ItemsSource = users;
        }
    }
}
