using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public delegate void AddItemDelegateTerapi();
    public partial class PopUpTerapi : Window
    {

        public AddItemDelegateTerapi AddItemCallback;
        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;
        private bool isEdit = false;
        private decimal biayaAngka;
        public PopUpTerapi()
        {
            InitializeComponent();

            isEdit = false;
            this.Title = "Tambah Data Terapi";
            btnSimpan.Content = "Simpan";

            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
        }

        public PopUpTerapi(string IdTerapi)
        {
            InitializeComponent();
            isEdit = true;

            this.Title = "Ubah Data Terapi";
            btnSimpan.Content = "Update";
            txtID.Text = IdTerapi;

            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            FetchEditData();
        
        }

        private void btnSimpan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isEdit) SimpanNew();
                else EditUpdate();

            }
            catch (Exception ex)
            {
                koneksi.Dispose();
                MessageBox.Show("Terjadi kesalahan!", "Informasi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnBatal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void FetchEditData()
        {
            DataTable Datatable = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_terapi WHERE mydentist.tbl_terapi.id_terapi = " + txtID.Text, null);
            foreach (DataRow row in Datatable.Rows)
            {
                //MessageBox.Show(row["nama_karyawan"].ToString());
                txtNama.Text = row["nama_terapi"].ToString();
                txtJenis.Text = row["jenis_terapi"].ToString();
                txtBiaya.Text = row["biaya_terapi"].ToString();  
                txtKeterangan.Text = row["keterangan_terapi"].ToString();
            }
        }

        private void txtBiaya_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value = txtBiaya.Text.Replace("Rp", "").Replace(".", "").Replace(",", "");
                    //.Replace("Rp", "").Replace(",", "").TrimStart('0');
            
            decimal ul;
            //Check we are indeed handling a number
            if (decimal.TryParse(value, out ul))
            {
                biayaAngka = ul;
                //ul /=  100;
                //Unsub the event so we don't enter a loop
                txtBiaya.TextChanged -= txtBiaya_TextChanged;
                //Format the text as currency
                txtBiaya.Text = string.Format(CultureInfo.CreateSpecificCulture("id-ID"), "{0:C0}", ul);
                txtBiaya.TextChanged += txtBiaya_TextChanged;
                txtBiaya.Select(txtBiaya.Text.Length, 0);
            }
            //bool goodToGo = TextisValid(txtBiaya.Text);
            
            //if (!goodToGo)
            //{
            //    txtBiaya.Text = "Rp0";
            //    txtBiaya.Select(txtBiaya.Text.Length, 0);
            //}

            //Console.WriteLine(ul);
       }

        private bool TextisValid(string text)
        {
            Regex money = new Regex(@"^\$(\d{1,3}(\,\d{3})*|(\d+))(\.\d{2})?$");
            return money.IsMatch(text);
        }

        void SimpanNew()
        {
            //@Bahar : ParameterData dalam bentuk Array (Menyesuakian Database)
            param = new ParameterData[] { new ParameterData("nama_terapi", txtNama.Text),
                                          new ParameterData("jenis_terapi",  txtJenis.Text),
                                          new ParameterData("biaya_terapi", biayaAngka),
                                          new ParameterData("keterangan_terapi", txtKeterangan.Text)};

            koneksi.InsertRow(SettingHelper.database, "tbl_terapi", true, param);

            //@Bahar : Penting ketika melakukan fungsi InsertRow, kalau tidak dicommit data gk akan masuk ke database
            koneksi.Commit(true);

            //@Bahar : melaksanakan fungsi delegate
            AddItemCallback();

            MessageBox.Show("Data terapi berhasil ditambah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

            //@Bahar : Penting, habis melakukan koneksi harus ditutup koneksi.Dispose() !!
            //Jika tidak ditutup akan bertabrakan dengan koneksi lain yang aktif, alhasil Not Respond
            koneksi.Dispose();
        }

        void EditUpdate()
        {
            param = new ParameterData[] { new ParameterData("nama_terapi", txtNama.Text),
                                          new ParameterData("jenis_terapi",  txtJenis.Text),
                                          new ParameterData("biaya_terapi", biayaAngka),
                                          new ParameterData("keterangan_terapi", txtKeterangan.Text)};

            koneksi.UpdateRow(SettingHelper.database, "tbl_terapi", "id_terapi=" + txtID.Text, 0, param);
            koneksi.Commit(true);

            AddItemCallback();

            MessageBox.Show("Data terapi berhasil diubah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

            koneksi.Dispose();

        }
    }
}
