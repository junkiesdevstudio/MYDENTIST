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
    public delegate void AddItemDelegateAmbilDataTerapi(string persen, string nama, int biaya);
    public partial class FormCariTerapi : Window
    {
        public AddItemDelegateAmbilDataTerapi AddItemCallbackTerapi;
        private cds_MYSQLKonektor koneksi;

        public FormCariTerapi()
        {
            InitializeComponent();

            ShowDataTabel();
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

        private void btnAmbil_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            MessageBoxResult result = MessageBox.Show("Ambil Data Terapi?", "Konfirmasi", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                DataRowView v = (DataRowView)dgTerapi.Items[row.GetIndex()];
                string persen = (string)v[0].ToString();
                string nama = (string)v[1].ToString();
                int biaya = (int)v[3];

                AddItemCallbackTerapi(persen, nama, biaya);
                this.Close();
            }
        }
    }
}
