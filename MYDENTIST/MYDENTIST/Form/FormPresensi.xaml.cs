using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using MYDENTIST.Form.PopUp;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MYDENTIST.Form
{
    /// <summary>
    /// Interaction logic for FormPresensi.xaml
    /// </summary>
    public partial class FormPresensi : UserControl
    {


        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;


        private string StandardMasuk1 = "06:30";
        private string StandardMasuk2 = "14:30";
        
        public FormPresensi()
        {
            InitializeComponent();

            DateTime newsStory = new DateTime();
            


            //if (newsStory.Date != DateTime.Now.Date)
            //{
                GenerateAbsensi(); 
            //}

                ShowDataTabel();

        }

        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {
            PopUpAbsensi popUpAbsensi = new PopUpAbsensi();
            popUpAbsensi.ShowDialog();
        }

        void ShowDataTabel()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgTerapi.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_presensi ORDER BY id_presensi DESC", null).DefaultView;
            string format = "hh:mm";
            ((DataGridTextColumn)dgTerapi.Columns[0]).Binding = new Binding("id_presensi");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
            ((DataGridTextColumn)dgTerapi.Columns[2]).Binding = new Binding("tanggal_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[2]).Binding.StringFormat = "{0:dddd, MMMM yyyy}";
            ((DataGridTextColumn)dgTerapi.Columns[3]).Binding = new Binding("nama_presensi");
            
            ((DataGridTextColumn)dgTerapi.Columns[4]).Binding = new Binding("masuk1_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[4]).Binding.StringFormat = @"hh\:mm";
            ((DataGridTextColumn)dgTerapi.Columns[5]).Binding = new Binding("pulang1_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[5]).Binding.StringFormat = @"hh\:mm";
            ((DataGridTextColumn)dgTerapi.Columns[6]).Binding = new Binding("masuk2_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[6]).Binding.StringFormat = @"hh\:mm";
            ((DataGridTextColumn)dgTerapi.Columns[7]).Binding = new Binding("pulang2_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[7]).Binding.StringFormat = @"hh\:mm";
            ((DataGridTextColumn)dgTerapi.Columns[8]).Binding = new Binding("ot_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[8]).Binding.StringFormat = @"hh\:mm";
            ((DataGridTextColumn)dgTerapi.Columns[9]).Binding = new Binding("lt_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[9]).Binding.StringFormat = @"hh\:mm";
            ((DataGridTextColumn)dgTerapi.Columns[10]).Binding = new Binding("jumlah_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[10]).Binding.StringFormat = @"hh\:mm";
            //@Bahar : Harus ditutup !!!
            koneksi.Dispose();
        }

        void GenerateAbsensi()
        {

            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

            DataTable CmbxDataTanggal = new DataTable();
            CmbxDataTanggal = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_presensi ORDER BY id_presensi DESC LIMIT 1", null);

            if (CmbxDataTanggal.Rows.Count == 0)
                InsertGenerate();
            else
                if (DateTime.Parse(CmbxDataTanggal.Rows[0]["tanggal_presensi"].ToString()) != DateTime.Now.Date)
                {
                    InsertGenerate();
                }
            koneksi.Dispose();



            
        }


        void InsertGenerate()
        {
            DataTable CmbxData = new DataTable();
            CmbxData = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan WHERE mydentist.tbl_karyawan.jenis_karyawan = 'Dokter'", null);
            //cmbNamaDokter.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan WHERE mydentist.tbl_karyawan.jenis_karyawan = 'Dokter'", null).DefaultView;
            //cmbNamaDokter.DisplayMemberPath = "nama_karyawan";
            //cmbNamaDokter.DataContext = "nama_karyawan";
            //cmbNamaDokter..valu = "nama_karyawan";

            List<string> studentList = new List<string>();
            for (int i = 0; i < CmbxData.Rows.Count; i++)
            {
                //cmbNamaDokter.Items.Add(CmbxData.Rows[i]["nama_karyawan"].ToString());
                param = new ParameterData[] { new ParameterData("nama_presensi", CmbxData.Rows[i]["nama_karyawan"].ToString()),
                                              new ParameterData("tanggal_presensi", DateTime.Now.Date)};
                koneksi.InsertRow(SettingHelper.database, "tbl_presensi", true, param);
                koneksi.Commit(true);
            }
        }

        private DataRowView v = null;
        private void dgTerapi_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {

                //dgTerapi.Items.Refresh();

                DataGridRow rowSelect = e.Row;
                v = (DataRowView)dgTerapi.Items[rowSelect.GetIndex()];
                
                
             
        }

        private void dgTerapi_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            //DataGridRow rowSelect = e.Row;
            //DataRowView v = (DataRowView)dgTerapi.Items[rowSelect.GetIndex()];
            //MessageBox.Show((string)v[4].ToString());
            //MessageBox.Show("A");
        }

        private void dgTerapi_CurrentCellChanged(object sender, EventArgs e)
        {
            if (v != null)
            {

                v.EndEdit();


                TimeSpan StandartTime1 = TimeSpan.ParseExact(StandardMasuk1, "c", null);
                TimeSpan StandartTime2 = TimeSpan.ParseExact(StandardMasuk2, "c", null);
                TimeSpan Pulang1 = TimeSpan.ParseExact((string)v[5].ToString(), "c", null);
                TimeSpan Masuk1 = TimeSpan.ParseExact((string)v[4].ToString(), "c", null);
                TimeSpan Pulang2 = TimeSpan.ParseExact((string)v[7].ToString(), "c", null);
                TimeSpan Masuk2 = TimeSpan.ParseExact((string)v[6].ToString(), "c", null);

                TimeSpan OT = new TimeSpan();
                TimeSpan LT = new TimeSpan();


                //Rumus = (Pulang1 - Masuk1) + (Pulang2 - MAsuk2)
                TimeSpan RumusJumlah = (Pulang1.Subtract(Masuk1).Add(Pulang2.Subtract(Masuk2)));

                //Rumus OTLT = ((6.30-(Plg1 - Msk 1)) + (14.30 - (Plg2 - Msk2))
                //TimeSpan RumusOTLT = (StandartTime1.Subtract(Pulang1.Subtract(Masuk1).Add(StandartTime2.Subtract(Pulang2.Subtract(Masuk2)))));
                TimeSpan t1 = Pulang1.Subtract(Masuk1);

                TimeSpan RumusOTLT = (StandartTime1 - t1);
                MessageBox.Show(t1.ToString());

                if (RumusOTLT > TimeSpan.Zero){
                    //LT = RumusOTLT;
                    //OT = TimeSpan.Zero;
                }
                else
                {
                    //OT = RumusOTLT;
                    //LT = TimeSpan.Zero;
                }

                //MessageBox.Show((string)v[4].ToString());
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                param = new ParameterData[] { new ParameterData("masuk1_presensi", TimeSpan.ParseExact((string)v[4].ToString(), "c", null)),
                                          new ParameterData("pulang1_presensi",TimeSpan.ParseExact((string)v[5].ToString(), "c", null)),
                                          new ParameterData("masuk2_presensi", TimeSpan.ParseExact((string)v[6].ToString(), "c", null)),
                                          new ParameterData("pulang2_presensi", TimeSpan.ParseExact((string)v[7].ToString(), "c", null)), 
                                          new ParameterData("ot_presensi", OT), 
                                          new ParameterData("lt_presensi", LT),
                                          new ParameterData("jumlah_presensi", RumusJumlah)};
                koneksi.UpdateRow(SettingHelper.database, "tbl_presensi", "id_presensi=" + (string)v[0].ToString(), 0, param);
                koneksi.Commit(true);

                ShowDataTabel();

                //MessageBox.Show("Data karyawan berhasil diubah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

                koneksi.Dispose();
                v = null;
            }
        }
    }
}
