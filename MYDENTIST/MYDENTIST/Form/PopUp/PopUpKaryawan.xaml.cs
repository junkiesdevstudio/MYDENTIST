using MYDENTIST.Class.DatabaseHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MYDENTIST.Form.PopUp
{
    /// <summary>
    /// Interaction logic for PopUpKaryawan.xaml
    /// </summary>
    /// 
    public delegate void AddItemDelegate();

    public partial class PopUpKaryawan : Window
    {
        public AddItemDelegate AddItemCallback;

        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;

        public PopUpKaryawan()
        {
            InitializeComponent();

            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            cmbJenis.Items.Add("Dokter");
            cmbJenis.Items.Add("Perawat");
            datePick.SelectedDate = DateTime.Today; 
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString("localhost", "root", "", 3306), true, System.Data.IsolationLevel.Serializable);
        }

        private void btnBatal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSimpan_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                param = new ParameterData[] { new ParameterData("nama_karyawan", txtNama.Text),
                                          new ParameterData("jenis_karyawan", cmbJenis.SelectedItem),
                                          new ParameterData("alamat_karyawan", txtAlamat.Text),
                                          new ParameterData("telp_karyawan", txtTelp.Text), 
                                          new ParameterData("tglmasuk_karyawan", datePick.Text), 
                                          new ParameterData("keterangan_karyawan", txtKeterangan.Text)};

                koneksi.InsertRow("mydentist", "tbl_karyawan", true, param);
                koneksi.Commit(true);

                AddItemCallback();

                MessageBox.Show("Data karyawan berhasil ditambah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);
                koneksi.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Terjadi kesalahan!", "Informasi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }
    }
}
