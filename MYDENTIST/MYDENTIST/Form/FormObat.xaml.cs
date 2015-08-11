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
    /// Interaction logic for FormObat.xaml
    /// </summary>
    public partial class FormObat : UserControl
    {

        private cds_MYSQLKonektor koneksi;
        private ParameterData param;

        public FormObat()
        {
            InitializeComponent();
            ShowDataTabel();

        }

        void ShowDataTabel()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgObat.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_obat", null).DefaultView;

            ((DataGridTextColumn)dgObat.Columns[0]).Binding = new Binding("id_obat");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
            ((DataGridTextColumn)dgObat.Columns[2]).Binding = new Binding("nama_obat");
            ((DataGridTextColumn)dgObat.Columns[3]).Binding = new Binding("jenis_obat");
            ((DataGridTextColumn)dgObat.Columns[4]).Binding = new Binding("hargabeli_obat");
            ((DataGridTextColumn)dgObat.Columns[4]).Binding.StringFormat = "{0:C2}";
            ((DataGridTextColumn)dgObat.Columns[5]).Binding = new Binding("hargajual_obat");
            ((DataGridTextColumn)dgObat.Columns[5]).Binding.StringFormat = "{0:C2}";
            ((DataGridTextColumn)dgObat.Columns[6]).Binding = new Binding("stok_obat");
            ((DataGridTextColumn)dgObat.Columns[7]).Binding = new Binding("keterangan_obat");


            //@Bahar : Harus ditutup !!!
            koneksi.Dispose();
        }

        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {
            PopUpObat popobat = new PopUpObat();
            popobat.AddItemCallback = new AddItemDelegateObat(this.AddItemCallbackPopUpObat);
            popobat.ShowDialog();
        }

        private void AddItemCallbackPopUpObat()
        {
            ShowDataTabel();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            if (dgObat.SelectedCells.Count > 0)
            {
                PopUpObat popObat = new PopUpObat(GetIndexObat(row));
                popObat.AddItemCallback = new AddItemDelegateObat(this.AddItemCallbackPopUpObat);
                popObat.ShowDialog();

            }
        }

        private string GetIndexObat(DataGridRow row)
        {
            DataRowView v = (DataRowView)dgObat.Items[row.GetIndex()];
            return (string)v[0].ToString();
        }

        private void txtPencarian_TextChanged(object sender, TextChangedEventArgs e)
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgObat.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_obat WHERE " +
                                  "mydentist.tbl_obat.nama_obat LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_obat.jenis_obat LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_obat.hargabeli_obat LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_obat.hargabeli_obat LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_obat.stok_obat LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_obat.keterangan_obat LIKE '%" + txtPencarian.Text + "%'", null).DefaultView;

            ((DataGridTextColumn)dgObat.Columns[0]).Binding = new Binding("id_obat");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
            ((DataGridTextColumn)dgObat.Columns[2]).Binding = new Binding("nama_obat");
            ((DataGridTextColumn)dgObat.Columns[3]).Binding = new Binding("jenis_obat");
            ((DataGridTextColumn)dgObat.Columns[4]).Binding = new Binding("hargabeli_obat");
            ((DataGridTextColumn)dgObat.Columns[4]).Binding.StringFormat = "{0:C2}";
            ((DataGridTextColumn)dgObat.Columns[5]).Binding = new Binding("hargajual_obat");
            ((DataGridTextColumn)dgObat.Columns[5]).Binding.StringFormat = "{0:C2}";
            ((DataGridTextColumn)dgObat.Columns[6]).Binding = new Binding("stok_obat");
            ((DataGridTextColumn)dgObat.Columns[7]).Binding = new Binding("keterangan_obat");



            //@Bahar : Harus ditutup !!!
            koneksi.Dispose();
        }

        private void btnHapus_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            if (dgObat.SelectedCells.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Hapus Data Obat?", "Konfirmasi", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                    koneksi.SendQuery("DELETE FROM mydentist.tbl_obat WHERE mydentist.tbl_obat.id_obat = " + GetIndexObat(row), null);
                    koneksi.Commit(true);

                    ShowDataTabel();

                    koneksi.Dispose();
                }

            }
        }

    }
}
