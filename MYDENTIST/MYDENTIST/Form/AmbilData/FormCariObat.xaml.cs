using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
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
    public delegate void AddItemDelegateAmbilDataObat(int id, int persen, string nama, int biaya);
    public partial class FormCariObat : Window
    {
        public AddItemDelegateAmbilDataObat AddItemCallbackObat;
        private cds_MYSQLKonektor koneksi;


        public FormCariObat()
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


        private void btnAmbil_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            MessageBoxResult result = MessageBox.Show("Ambil Data Obat?", "Konfirmasi", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                DataRowView v = (DataRowView)dgObat.Items[row.GetIndex()];
                int id = (int)v[0];
                int persen = (int)v[0];
                string nama = (string)v[1].ToString();
                int biaya = (int)v[4];

               // MessageBox.Show((string)v[0].ToString());

                AddItemCallbackObat(id, persen, nama, biaya);
                this.Close();
            }

        }
    }
}
