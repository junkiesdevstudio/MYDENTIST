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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MYDENTIST.Form
{
    /// <summary>
    /// Interaction logic for FormTerapi.xaml
    /// </summary>
    public partial class FormTerapi : UserControl
    {
        private cds_MYSQLKonektor koneksi;
        private ParameterData param;

        public FormTerapi()
        {
            InitializeComponent();
            ShowDataTabel();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            if (dgTerapi.SelectedCells.Count > 0)
            {
                PopUpTerapi popTerapi = new PopUpTerapi(GetIndexpasien(row));
                popTerapi.AddItemCallback = new AddItemDelegateTerapi(this.AddItemCallbackPopUpTerapi);
                popTerapi.ShowDialog();

            }
        }

        private void btnHapus_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            if (dgTerapi.SelectedCells.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Hapus Data Karyawan?", "Konfirmasi", MessageBoxButton.YesNo);


                if (result == MessageBoxResult.Yes)
                {
                    koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                    koneksi.SendQuery("DELETE FROM mydentist.tbl_terapi WHERE mydentist.tbl_terapi.id_terapi = " + GetIndexpasien(row), null);
                    koneksi.Commit(true);

                    ShowDataTabel();

                    koneksi.Dispose();
                }

            }
        }

        private string GetIndexpasien(DataGridRow row)
        {
            DataRowView v = (DataRowView)dgTerapi.Items[row.GetIndex()];
            return (string)v[0].ToString();
        }


        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {
            PopUpTerapi poppasien = new PopUpTerapi();
            poppasien.AddItemCallback = new AddItemDelegateTerapi(this.AddItemCallbackPopUpTerapi);
            poppasien.ShowDialog();
        }

        private void AddItemCallbackPopUpTerapi()
        {
            ShowDataTabel();
        }

        private void txtPencarian_TextChanged(object sender, TextChangedEventArgs e)
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgTerapi.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_terapi WHERE " +
                                  "mydentist.tbl_terapi.nama_terapi LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_terapi.jenis_terapi LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_terapi.biaya_terapi LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_terapi.keterangan_terapi LIKE '%" + txtPencarian.Text + "%'", null).DefaultView;

            ((DataGridTextColumn)dgTerapi.Columns[0]).Binding = new Binding("id_terapi");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_karyawan");
            ((DataGridTextColumn)dgTerapi.Columns[2]).Binding = new Binding("nama_terapi");
            ((DataGridTextColumn)dgTerapi.Columns[3]).Binding = new Binding("jenis_terapi");
            ((DataGridTextColumn)dgTerapi.Columns[3]).Binding.StringFormat = "{0} %";
            ((DataGridTextColumn)dgTerapi.Columns[4]).Binding = new Binding("biaya_terapi");
            ((DataGridTextColumn)dgTerapi.Columns[4]).Binding.StringFormat = "{0:C2}";

            ((DataGridTextColumn)dgTerapi.Columns[5]).Binding = new Binding("keterangan_terapi");


            //@Bahar : Harus ditutup !!!
            koneksi.Dispose();
        }

        void ShowDataTabel()
        {

            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgTerapi.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_terapi", null).DefaultView;

            ((DataGridTextColumn)dgTerapi.Columns[0]).Binding = new Binding("id_terapi");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_karyawan");
            ((DataGridTextColumn)dgTerapi.Columns[2]).Binding = new Binding("nama_terapi");
            ((DataGridTextColumn)dgTerapi.Columns[3]).Binding = new Binding("jenis_terapi");
            ((DataGridTextColumn)dgTerapi.Columns[3]).Binding.StringFormat = "{0} %";
            ((DataGridTextColumn)dgTerapi.Columns[4]).Binding = new Binding("biaya_terapi");
            ((DataGridTextColumn)dgTerapi.Columns[4]).Binding.StringFormat = "{0:C2}";

            ((DataGridTextColumn)dgTerapi.Columns[5]).Binding = new Binding("keterangan_terapi");


            //@Bahar : Harus ditutup !!!
            koneksi.Dispose();
        }
    }
}
