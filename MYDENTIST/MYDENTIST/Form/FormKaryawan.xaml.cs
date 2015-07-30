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
using MYDENTIST.Class.DatabaseHelper;
using MYDENTIST.Form.PopUp;
using System.Data;

namespace MYDENTIST.Form
{
    /// <summary>
    /// Interaction logic for FormKaryawan.xaml
    /// </summary>
    public partial class FormKaryawan : UserControl
    {

        private cds_MYSQLKonektor koneksi;
        private ParameterData param;
        public FormKaryawan()
        {
            InitializeComponent();

            ShowDataTabel();
        }


        //@bahar : Nek iki rasah tak jelaske...ahahahaha
        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {
            PopUpKaryawan popKaryawan = new PopUpKaryawan();
            popKaryawan.AddItemCallback = new AddItemDelegateKaryawan(this.AddItemCallbackPopUpKaryawan);
            popKaryawan.ShowDialog();

           
        }


        //@Bahar : Fungsi Delegate Callback dari PopUPKaryawan
        //- Ketika Tombol simpan di PopUpKaryawan di clik, maka secara otomatis akan memanggil
        //  fungsi ini tanpa harus mengload baru FormKaryawan agar nilai pada FormKaryawan tidak berubah
        private void AddItemCallbackPopUpKaryawan()
        {
            ShowDataTabel();
        }


        //@Bahar : Manual binding data dari database ke kolom DataGrid
        void ShowDataTabel()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgUsers.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan", null).DefaultView;
            
            ((DataGridTextColumn)dgUsers.Columns[0]).Binding = new Binding("id_karyawan");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[2]).Binding = new Binding("nama_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[3]).Binding = new Binding("jenis_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[4]).Binding = new Binding("alamat_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[5]).Binding = new Binding("telp_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[6]).Binding = new Binding("tglmasuk_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[7]).Binding = new Binding("keterangan_karyawan");
            

            //@Bahar : Harus ditutup !!!
            koneksi.Dispose();
        }

        //@Bahar : Binding dinamik Click EditButton yang digenerate melalui DataGridTemplate
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            if (dgUsers.SelectedCells.Count > 0)
            {
                //MessageBox.Show(GetIndexKaryawan(row));

                PopUpKaryawan popKaryawan = new PopUpKaryawan(GetIndexKaryawan(row));
                popKaryawan.AddItemCallback = new AddItemDelegateKaryawan(this.AddItemCallbackPopUpKaryawan);
                popKaryawan.ShowDialog();
            }
        }

        //@Bahar : Binding dinamik Click HapusButton yang digenerate melalui DataGridTemplate
        private void btnHapus_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            if (dgUsers.SelectedCells.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Hapus Data Karyawan?", "Konfirmasi", MessageBoxButton.YesNo);

                
                if (result == MessageBoxResult.Yes)
                {
                    koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                    koneksi.SendQuery("DELETE FROM mydentist.tbl_karyawan WHERE mydentist.tbl_karyawan.id_karyawan = " + GetIndexKaryawan(row), null);
                    koneksi.Commit(true);

                    ShowDataTabel();

                    koneksi.Dispose();
                }

            }
        }


        //@Bahar : Ambil ID Karyawan dari DataGrid yang diselect
        private string GetIndexKaryawan(DataGridRow row)
        {
            DataRowView v = (DataRowView)dgUsers.Items[row.GetIndex()];
            return (string)v[0].ToString();
        }

        private void txtPencarian_TextChanged(object sender, TextChangedEventArgs e)
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgUsers.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan WHERE " +
                                  "mydentist.tbl_karyawan.nama_karyawan LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_karyawan.alamat_karyawan LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_karyawan.telp_karyawan LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_karyawan.keterangan_karyawan LIKE '%" + txtPencarian.Text + "%'", null).DefaultView;

            ((DataGridTextColumn)dgUsers.Columns[0]).Binding = new Binding("id_karyawan");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[2]).Binding = new Binding("nama_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[3]).Binding = new Binding("jenis_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[4]).Binding = new Binding("alamat_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[5]).Binding = new Binding("telp_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[6]).Binding = new Binding("tglmasuk_karyawan");
            ((DataGridTextColumn)dgUsers.Columns[7]).Binding = new Binding("keterangan_karyawan");


            //@Bahar : Harus ditutup !!!
            koneksi.Dispose();
        }
    }
}
