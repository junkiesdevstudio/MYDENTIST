using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using MYDENTIST.Form.AmbilData;
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

namespace MYDENTIST.Form.PopUpData
{
    public delegate void AddItemDelegatePopUpDataAppointment();
    public partial class PopUpDataAppointment : Window
    {
        public AddItemDelegatePopUpDataAppointment AddItemCallback;

        private cds_MYSQLKonektor koneksi;


        private string IDPasien;
        private string IDDokter;

        public PopUpDataAppointment()
        {
            InitializeComponent();


            datePick.SelectedDate = DateTime.Today;

            DataDokter();

            
        }

        void DataDokter()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            cmbNamaDokter.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan WHERE mydentist.tbl_karyawan.jenis_karyawan = 'Dokter'", null).DefaultView;
            cmbNamaDokter.DisplayMemberPath = "nama_karyawan";
        
        }

        private void btnBatal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSimpan_Click(object sender, RoutedEventArgs e)
        {
            AddItemCallback();
        }

        private void btnCariRM_Click(object sender, RoutedEventArgs e)
        {
            FormCariPasien formCariPasien = new FormCariPasien();
            formCariPasien.AddItemCallback = new AddItemDelegateAmbilDataPasien(this.AddItemCallbackAmbilDataPasien);
            formCariPasien.ShowDialog();
        }
        private void AddItemCallbackAmbilDataPasien(string idPasien, string noRM, string namapasien)
        {
            //ShowDataTabel();
            //MessageBox.Show("Ambil");
            IDPasien = idPasien;
            txtNoRm.Text = noRM;
            txtNamaPasien.Text = namapasien;

        }

        
    }
}
