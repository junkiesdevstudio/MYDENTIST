using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using MYDENTIST.Form.PopUp;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace MYDENTIST.Form.AmbilData
{
    public delegate void AddItemDelegateAmbilDataPasien(string idPasien, string noRM, string namapasien);

    public partial class FormCariPasien : Window
    {
        public AddItemDelegateAmbilDataPasien AddItemCallback;


        private cds_MYSQLKonektor koneksi;

        public FormCariPasien()
        {
            InitializeComponent();

            ShowDataTabel();
        }


        void ShowDataTabel()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgPasien.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_pasien", null).DefaultView;

            ((DataGridTextColumn)dgPasien.Columns[0]).Binding = new Binding("id_pasien");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
            ((DataGridTextColumn)dgPasien.Columns[2]).Binding = new Binding("norm_pasien");
            ((DataGridTextColumn)dgPasien.Columns[3]).Binding = new Binding("nama_pasien");
            ((DataGridTextColumn)dgPasien.Columns[4]).Binding = new Binding("alamat_pasien");
            ((DataGridTextColumn)dgPasien.Columns[5]).Binding = new Binding("telp_pasien");
            ((DataGridTextColumn)dgPasien.Columns[6]).Binding = new Binding("keterangan_pasien");


            //@Bahar : Harus ditutup !!!
            koneksi.Dispose();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            if (dgPasien.SelectedCells.Count > 0)
            {
                PopUpPasien popPasien = new PopUpPasien(GetIndexpasien(row));
                popPasien.AddItemCallback = new AddItemDelegatePasien(this.AddItemCallbackPopUpPasien);
                popPasien.ShowDialog();

            }
        }
        private string GetIndexpasien(DataGridRow row)
        {
            DataRowView v = (DataRowView)dgPasien.Items[row.GetIndex()];
            return (string)v[0].ToString();
        }

        private void AddItemCallbackPopUpPasien()
        {
            ShowDataTabel();
        }

        private void btnAmbil_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            MessageBoxResult result = MessageBox.Show("Ambil Data Pasien?", "Konfirmasi", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                DataRowView v = (DataRowView)dgPasien.Items[row.GetIndex()];
                string idPasien = (string)v[0].ToString();
                string NoRM = (string)v[2].ToString();
                string NamaPasien = (string)v[3].ToString();

                AddItemCallback(idPasien, NoRM, NamaPasien);
                this.Close();
            }
        }

        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtPencarian_TextChanged(object sender, TextChangedEventArgs e)
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgPasien.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_pasien WHERE " +
                                  "mydentist.tbl_pasien.norm_pasien LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_pasien.nama_pasien LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_pasien.alamat_pasien LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_pasien.telp_pasien LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_pasien.keterangan_pasien LIKE '%" + txtPencarian.Text + "%'", null).DefaultView;

            ((DataGridTextColumn)dgPasien.Columns[0]).Binding = new Binding("id_pasien");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
            ((DataGridTextColumn)dgPasien.Columns[2]).Binding = new Binding("norm_pasien");
            ((DataGridTextColumn)dgPasien.Columns[3]).Binding = new Binding("nama_pasien");
            ((DataGridTextColumn)dgPasien.Columns[4]).Binding = new Binding("alamat_pasien");
            ((DataGridTextColumn)dgPasien.Columns[5]).Binding = new Binding("telp_pasien");
            ((DataGridTextColumn)dgPasien.Columns[6]).Binding = new Binding("keterangan_pasien");


            //@Bahar : Harus ditutup !!!
            koneksi.Dispose();
        }
    }
}
