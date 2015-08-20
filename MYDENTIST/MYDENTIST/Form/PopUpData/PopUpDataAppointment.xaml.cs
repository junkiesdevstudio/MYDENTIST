using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using MYDENTIST.Form.AmbilData;
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

namespace MYDENTIST.Form.PopUpData
{
    public delegate void AddItemDelegatePopUpDataAppointment();
    public partial class PopUpDataAppointment : Window
    {
        public AddItemDelegatePopUpDataAppointment AddItemCallback;

        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;

        private string IDPasien;
        private string IDDokter;
        private string IDAppoinment;
        private bool isEdit;
        private string status;

        private DateTime dJam;
        public PopUpDataAppointment()
        {
            InitializeComponent();


            datePick.SelectedDate = DateTime.Today;

            DataDokter();

            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

            koneksi.Dispose();
        }


        public PopUpDataAppointment(string IdAppointment)
        {
            InitializeComponent();
            isEdit = true;
            DataDokter();

            this.Title = "Ubah Data Appointment";
            btnSimpan.Content = "Update";
            IDAppoinment = IdAppointment;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("id-ID");
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            FetchEditData();
            koneksi.Dispose();
        }

        void FetchEditData()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            DataTable Datatable = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_appointment WHERE mydentist.tbl_appointment.id_appo = " + IDAppoinment, null);
            foreach (DataRow row in Datatable.Rows)
            {
                //MessageBox.Show(row["nama_karyawan"].ToString());
                IDPasien = row["id_pasien"].ToString();
                status = row["status_appo"].ToString();
                datePick.Text = row["tanggal_appo"].ToString();
                txtNoRm.Text = row["norm_appo"].ToString();
                txtNamaPasien.Text = row["namapasien_appo"].ToString();
                cmbNamaDokter.SelectedValue = row["namadokter_appo"].ToString();
                //txtAlamat.Text = row["alamat_karyawan"].ToString();
                //txtTelp.Text = row["telp_karyawan"].ToString();
                
                txtKeterangan.Text = row["keterangan_appo"].ToString();
            }
        }

        void DataDokter()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

            DataTable CmbxData = new DataTable();
            CmbxData = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan WHERE mydentist.tbl_karyawan.jenis_karyawan = 'Dokter'", null);
            //cmbNamaDokter.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan WHERE mydentist.tbl_karyawan.jenis_karyawan = 'Dokter'", null).DefaultView;
            //cmbNamaDokter.DisplayMemberPath = "nama_karyawan";
            //cmbNamaDokter.DataContext = "nama_karyawan";
            //cmbNamaDokter..valu = "nama_karyawan";

            List<string> studentList = new List<string>();
            for (int i = 0; i < CmbxData.Rows.Count; i++)
            {
                cmbNamaDokter.Items.Add(CmbxData.Rows[i]["nama_karyawan"].ToString());
            }

            koneksi.Dispose();
        }

        private void btnBatal_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSimpan_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                dJam = DateTime.Parse(txtJam.Text);

                try
                {
                    if (!isEdit) SimpanNew();
                    else EditUpdate();

                }
                catch (Exception ex)
                {

                    koneksi.Dispose();
                    //MessageBox.Show("Terjadi kesalahan!", "Informasi", MessageBoxButton.OK, MessageBoxImage.Error);
                    MessageBox.Show(ex.Message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cek penulisan waktu! Contoh format 00:00", "Informasi", MessageBoxButton.OK, MessageBoxImage.Error);
            }


        }



        void SimpanNew()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            //DataRowView drv = (DataRowView)cmbNamaDokter.SelectedItem;
            //String valueOfItem = drv["nama_karyawan"].ToString();

            //@Bahar : ParameterData dalam bentuk Array (Menyesuakian Database)
            param = new ParameterData[] { new ParameterData("id_pasien", IDPasien),
                                          new ParameterData("jam_appo", dJam),
                                          new ParameterData("tanggal_appo", datePick.SelectedDate),
                                          new ParameterData("norm_appo", txtNoRm.Text),
                                          new ParameterData("namapasien_appo", txtNamaPasien.Text), 
                                          new ParameterData("namadokter_appo", cmbNamaDokter.SelectedItem), 
                                          new ParameterData("status_appo", 0), 
                                          new ParameterData("keterangan_appo", txtKeterangan.Text)};

            koneksi.InsertRow(SettingHelper.database, "tbl_appointment", true, param);

            //@Bahar : Penting ketika melakukan fungsi InsertRow, kalau tidak dicommit data gk akan masuk ke database
            koneksi.Commit(true);

            //@Bahar : melaksanakan fungsi delegate
            AddItemCallback();

            MessageBox.Show("Data appointment berhasil ditambah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

            //@Bahar : Penting, habis melakukan koneksi harus ditutup koneksi.Dispose() !!
            //Jika tidak ditutup akan bertabrakan dengan koneksi lain yang aktif, alhasil Not Respond
            koneksi.Dispose();
        }

        void EditUpdate()
        {

            

            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            param = new ParameterData[] { new ParameterData("id_pasien", IDPasien),
                                          new ParameterData("jam_appo", dJam),
                                          new ParameterData("tanggal_appo", datePick.SelectedDate),
                                          new ParameterData("norm_appo", txtNoRm.Text),
                                          new ParameterData("namapasien_appo", txtNamaPasien.Text), 
                                          new ParameterData("namadokter_appo", cmbNamaDokter.SelectedItem), 
                                          new ParameterData("status_appo", status), 
                                          new ParameterData("keterangan_appo", txtKeterangan.Text)};

            koneksi.UpdateRow(SettingHelper.database, "tbl_appointment", "id_appo =" + IDAppoinment, 0, param);
            koneksi.Commit(true);

            AddItemCallback();
            koneksi.Dispose();

            this.Close();
            MessageBox.Show("Data appointment berhasil diubah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

            

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
            
            IDPasien = idPasien;
            txtNoRm.Text = noRM;
            txtNamaPasien.Text = namapasien;
           
        }

        private void txtJam_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        
    }
}
