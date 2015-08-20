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

namespace MYDENTIST.Form.PopUp
{
    public delegate void AddItemDelegatePasien();
    public partial class PopUpPasien : Window
    {

        public AddItemDelegatePasien AddItemCallback;

        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;
        private bool isEdit = false; 
        public PopUpPasien()
        {
            InitializeComponent();

            isEdit = false;
            this.Title = "Tambah Data Pasien";
            btnSimpan.Content = "Simpan";

            
        }


        public PopUpPasien(string IdPasien)
        {
            InitializeComponent();
            isEdit = true;

            this.Title = "Ubah Data Pasien";
            btnSimpan.Content = "Update";
            txtID.Text = IdPasien;

            
            FetchEditData();

            //MessageBox.Show(IdPasien);
        
        }
        private void btnSimpan_Click(object sender, RoutedEventArgs e)
        {

            if (txtNoRM.Text != string.Empty && txtNama.Text != string.Empty && txtAlamat.Text != string.Empty && txtTelp.Text != string.Empty)
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
            else
            {
                MessageBox.Show("Mohon data field dengan tanda * diisi !", "Informasi", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            
           
        }

        private void btnBatal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        void FetchEditData()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            DataTable Datatable = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_pasien WHERE mydentist.tbl_pasien.id_pasien = " + txtID.Text, null);
            foreach (DataRow row in Datatable.Rows)
            {
                //MessageBox.Show(row["nama_karyawan"].ToString());
                txtNoRM.Text = row["norm_pasien"].ToString();
                txtNama.Text = row["nama_pasien"].ToString();
                txtAlamat.Text = row["alamat_pasien"].ToString();
                txtTelp.Text = row["telp_pasien"].ToString();
                txtKeterangan.Text = row["keterangan_pasien"].ToString();
            }

            koneksi.Dispose();
        }

        void SimpanNew()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            param = new ParameterData[] { new ParameterData("norm_pasien", txtNoRM.Text),
                                          new ParameterData("nama_pasien",  txtNama.Text),
                                          new ParameterData("alamat_pasien", txtAlamat.Text),
                                          new ParameterData("telp_pasien", txtTelp.Text), 
                                          new ParameterData("keterangan_pasien", txtKeterangan.Text)};

            koneksi.InsertRow(SettingHelper.database, "tbl_pasien", true, param);

            //@Bahar : Penting ketika melakukan fungsi InsertRow, kalau tidak dicommit data gk akan masuk ke database
            koneksi.Commit(true);

            //@Bahar : melaksanakan fungsi delegate
            AddItemCallback();
            koneksi.Dispose();

            this.Close();
            MessageBox.Show("Data pasien berhasil ditambah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        void EditUpdate()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            param = new ParameterData[] { new ParameterData("norm_pasien", txtNoRM.Text),
                                          new ParameterData("nama_pasien",  txtNama.Text),
                                          new ParameterData("alamat_pasien", txtAlamat.Text),
                                          new ParameterData("telp_pasien", txtTelp.Text), 
                                          new ParameterData("keterangan_pasien", txtKeterangan.Text)};

            koneksi.UpdateRow(SettingHelper.database, "tbl_pasien", "id_pasien=" + txtID.Text, 0, param);
            koneksi.Commit(true);

            AddItemCallback();
            koneksi.Dispose();

            this.Close();
            MessageBox.Show("Data pasien berhasil diubah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
