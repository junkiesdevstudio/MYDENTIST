using MYDENTIST.Class.DatabaseHelper;
using System;
using System.Collections.Generic;
using System.Data;
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


    //Untuk Form yang berdiri sendiri alia tidak menempel (PopUp) di MainForm, menggunakan Base class WINDOW (FormKaryawan.xaml) --> Add -> Window
    //Untuk Form yang menempel di MainForm, menggunakan Base class USERCONTROL (PopUpKaryawan.xaml)-> Add -> UserControl
    //Dikarenakan WPF tidak support / tidak digunakan untuk model MDIChild, MDIParrent seperti Winform

    public partial class PopUpKaryawan : Window
    {
        public AddItemDelegate AddItemCallback;

        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;
        private bool isEdit = false;

        //@Bahar : Constructor untuk edit AddNew PopUp
        public PopUpKaryawan()
        {
            InitializeComponent();
            isEdit = false;
            this.Title = "Tambah Data Karyawan";
            btnSimpan.Content = "Simpan";

            //@Bahar : Format Tanggal (id-ID)
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            Thread.CurrentThread.CurrentCulture = ci;

            //@Bahar : Jenis ini static, jadi gk tak simpen di database
            cmbJenis.Items.Add("Dokter");
            cmbJenis.Items.Add("Perawat");

            datePick.SelectedDate = DateTime.Today; 

            //@Bahar : Melakukan Koneksi
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString("localhost", "root", "", 3306), true, System.Data.IsolationLevel.Serializable);
        }

        //@Bahar : Constructor untuk edit popUp
        public PopUpKaryawan(string IdKaryawan)
        {
            InitializeComponent();
            isEdit = true;
            cmbJenis.Items.Add("Dokter");
            cmbJenis.Items.Add("Perawat");

            this.Title = "Ubah Data Karyawan";
            btnSimpan.Content = "Update";
            txtID.Text = IdKaryawan;

            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString("localhost", "root", "", 3306), true, System.Data.IsolationLevel.Serializable);
            FetchEditData();
        
        }

        //@Bahar : Rasah tak jelake nek iki...ahahahhaa
        private void btnBatal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
                MessageBox.Show("Terjadi kesalahan!", "Informasi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
        }

        void FetchEditData()
        {
            DataTable Datatable = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan WHERE mydentist.tbl_karyawan.id_karyawan = " + txtID.Text, null);
            foreach (DataRow row in Datatable.Rows)
            {
                //MessageBox.Show(row["nama_karyawan"].ToString());
                txtNama.Text = row["nama_karyawan"].ToString();
                cmbJenis.SelectedItem = row["jenis_karyawan"].ToString();
                txtAlamat.Text = row["alamat_karyawan"].ToString();
                txtTelp.Text = row["telp_karyawan"].ToString();
                datePick.Text = row["tglmasuk_karyawan"].ToString();
                txtKeterangan.Text = row["keterangan_karyawan"].ToString();
            }
        }

        void SimpanNew()
        {
            //@Bahar : ParameterData dalam bentuk Array (Menyesuakian Database)
            param = new ParameterData[] { new ParameterData("nama_karyawan", txtNama.Text),
                                          new ParameterData("jenis_karyawan", cmbJenis.SelectedItem),
                                          new ParameterData("alamat_karyawan", txtAlamat.Text),
                                          new ParameterData("telp_karyawan", txtTelp.Text), 
                                          new ParameterData("tglmasuk_karyawan", datePick.Text), 
                                          new ParameterData("keterangan_karyawan", txtKeterangan.Text)};

            koneksi.InsertRow("mydentist", "tbl_karyawan", true, param);

            //@Bahar : Penting ketika melakukan fungsi InsertRow, kalau tidak dicommit data gk akan masuk ke database
            koneksi.Commit(true);

            //@Bahar : melaksanakan fungsi delegate
            AddItemCallback();

            MessageBox.Show("Data karyawan berhasil ditambah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

            //@Bahar : Penting, habis melakukan koneksi harus ditutup koneksi.Dispose() !!
            //Jika tidak ditutup akan bertabrakan dengan koneksi lain yang aktif, alhasil Not Respond
            koneksi.Dispose();
        }

        void EditUpdate()
        {
            param = new ParameterData[] { new ParameterData("nama_karyawan", txtNama.Text),
                                          new ParameterData("jenis_karyawan", cmbJenis.SelectedItem),
                                          new ParameterData("alamat_karyawan", txtAlamat.Text),
                                          new ParameterData("telp_karyawan", txtTelp.Text), 
                                          new ParameterData("tglmasuk_karyawan", datePick.Text), 
                                          new ParameterData("keterangan_karyawan", txtKeterangan.Text)};
            koneksi.UpdateRow("mydentist", "tbl_karyawan", "id_karyawan=" + txtID.Text, 0, param);
            koneksi.Commit(true);

            AddItemCallback();

            MessageBox.Show("Data karyawan berhasil diubah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

            koneksi.Dispose();

        }
    }


}
