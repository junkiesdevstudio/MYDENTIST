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
        private bool mulaiFilter;
        public FormPresensi()
        {
            InitializeComponent();

            DateTime newsStory = new DateTime();

            for (int i = DateTime.UtcNow.Year; i >= 1950; i--)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i;
                string t = item.Content.ToString();
                cmbTahun.Items.Add(t);
                //Console.WriteLine(t);
            }


            DateTime now = Convert.ToDateTime("01/01/0001");
            for (int i = 0; i < 12; i++)
            {
                cmbBulan.Items.Add(now.ToString("MMMM"));
                now = now.AddMonths(1);


            }

            ShowDataKaryawan();
            

            cmbTahun.SelectedValue = DateTime.Now.ToString("yyyy");
            cmbBulan.SelectedValue = DateTime.Now.ToString("MMMM");
            cmbKaryawan.SelectedValue = "Semua";

            //if (newsStory.Date != DateTime.Now.Date)
            //{
                GenerateAbsensi(); 
            //}

                ShowDataTabel();
                mulaiFilter = true;
        }

        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {
            PopUpAbsensi popUpAbsensi = new PopUpAbsensi();
            popUpAbsensi.ShowDialog();
        }

        void ShowDataKaryawan()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

            DataTable CmbxData = new DataTable();
            CmbxData = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan WHERE mydentist.tbl_karyawan.jenis_karyawan = 'Dokter'", null);
            //cmbNamaDokter.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan WHERE mydentist.tbl_karyawan.jenis_karyawan = 'Dokter'", null).DefaultView;
            //cmbNamaDokter.DisplayMemberPath = "nama_karyawan";
            //cmbNamaDokter.DataContext = "nama_karyawan";
            //cmbNamaDokter..valu = "nama_karyawan";

            //List<string> studentList = new List<string>();
            cmbKaryawan.Items.Add("Semua");
            for (int i = 0; i < CmbxData.Rows.Count; i++)
            {
                cmbKaryawan.Items.Add(CmbxData.Rows[i]["nama_karyawan"].ToString());
            }

            koneksi.Dispose();
        }




        void ShowDataTabel()
        {

            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgTerapi.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_presensi WHERE MONTH(mydentist.tbl_presensi.tanggal_presensi) = " + (cmbBulan.SelectedIndex + 1) + " AND YEAR(mydentist.tbl_presensi.tanggal_presensi) =" + cmbTahun.SelectedItem.ToString() + " ORDER BY id_presensi DESC", null).DefaultView;
            //dgTerapi.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_appointment WHERE MONTH(mydentist.tbl_appointment.tanggal_appo) = " + (cmbBulan.SelectedIndex + 1) + " AND YEAR(mydentist.tbl_appointment.tanggal_appo) =" + cmbTahun.SelectedItem.ToString() + " ORDER BY CAST(mydentist.tbl_appointment.tanggal_appo as datetime),CAST(mydentist.tbl_appointment.jam_appo as time) ASC", null).DefaultView;
            
            
            string format = "hh:mm";
            ((DataGridTextColumn)dgTerapi.Columns[0]).Binding = new Binding("id_presensi");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
            ((DataGridTextColumn)dgTerapi.Columns[1]).Binding = new Binding("tanggal_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[1]).Binding.StringFormat = "{0:dd}";
            ((DataGridTextColumn)dgTerapi.Columns[2]).Binding = new Binding("tanggal_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[2]).Binding.StringFormat = "{0:dddd}";
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

                string StandartMasuk1 = "06:30:00";
                DateTime s1 = DateTime.Parse(StandartMasuk1);
                string StandartMasuk2 = "4:30:00";
                DateTime s2 = DateTime.Parse(StandartMasuk2);

                string Msk1 = (string)v[4].ToString();
                DateTime m1 = DateTime.Parse(Msk1);
                string Plg1 = (string)v[5].ToString();
                DateTime p1 = DateTime.Parse(Plg1);


                string Msk2 = (string)v[6].ToString();
                DateTime m2 = DateTime.Parse(Msk2);
                string Plg2 = (string)v[7].ToString();
                DateTime p2 = DateTime.Parse(Plg2);

                double TotalP1_M1 = (p1.TimeOfDay - m1.TimeOfDay).TotalHours;
                double TotalP2_M2 = (p2.TimeOfDay - m2.TimeOfDay).TotalHours;

                double total = 0;
                double jumlah = TotalP1_M1 + TotalP2_M2;
                TimeSpan LT;// = TimeSpan.FromHours(total);
                TimeSpan OT;// = TimeSpan.FromHours(total);
                TimeSpan RumusJumlah = TimeSpan.FromHours(jumlah);

                //MessageBox.Show((TotalP2_M2).ToString());
                if (TotalP2_M2 == 0)
                {
                    total = ((s1.TimeOfDay.TotalHours - TotalP1_M1));
                }

                if (TotalP1_M1 == 0)
                {
                    total = ((s2.TimeOfDay.TotalHours - TotalP2_M2));
                }

                if (TotalP2_M2 != 0 && TotalP1_M1 != 0)
                {
                    total = ((s1.TimeOfDay.TotalHours - TotalP1_M1) + (s2.TimeOfDay.TotalHours - TotalP2_M2));
                }

                if (total >= 0){
                    LT = TimeSpan.FromHours(total);
                    OT = TimeSpan.Zero;
                }
                else
                {
                    OT = TimeSpan.FromHours(total);
                    LT = TimeSpan.Zero;
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

                /*
                double TotalJumlah = 0;
                DateTime d = new DateTime();
                for (int x = 0; x < dgTerapi.Items.Count; x++)
                {

                    var rows = (DataGridRow)dgTerapi.ItemContainerGenerator.ContainerFromIndex(x);

                    DataRowView vx = (DataRowView)dgTerapi.Items[x];
                    DateTime t = DateTime.Parse((string)vx[10].ToString());

                    //d = d + t.TimeOfDay;

                    TotalJumlah += (d.TimeOfDay + t.TimeOfDay).TotalHours;
                }

                totalJumlah.Text = TimeSpan.FromHours(TotalJumlah).ToString();
                */

                //MessageBox.Show("Data karyawan berhasil diubah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);
                
                koneksi.Dispose();
                v = null;
            }
        }

        private void txtPencarian_TextChanged(object sender, TextChangedEventArgs e)
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgTerapi.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_presensi WHERE mydentist.tbl_presensi.nama_presensi LIKE '%" + txtPencarian.Text + "%' ORDER BY id_presensi DESC", null).DefaultView;
            string format = "hh:mm";
            ((DataGridTextColumn)dgTerapi.Columns[0]).Binding = new Binding("id_presensi");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
            ((DataGridTextColumn)dgTerapi.Columns[1]).Binding = new Binding("tanggal_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[1]).Binding.StringFormat = "{0:dddd}";
            ((DataGridTextColumn)dgTerapi.Columns[2]).Binding = new Binding("tanggal_presensi");
            ((DataGridTextColumn)dgTerapi.Columns[2]).Binding.StringFormat = "{0:MMMM yyyy}";

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

            /*
            double TotalJumlah = 0;
            DateTime d = new DateTime();
            for (int x = 0; x < dgTerapi.Items.Count; x++)
            {

                var rows = (DataGridRow)dgTerapi.ItemContainerGenerator.ContainerFromIndex(x);

                DataRowView v = (DataRowView)dgTerapi.Items[x];
                DateTime t = DateTime.Parse((string)v[10].ToString());

                TotalJumlah = (d.Add(t.TimeOfDay).TimeOfDay).TotalHours;
            }

            totalJumlah.Text = TimeSpan.FromHours(TotalJumlah).ToString();
            */
            koneksi.Dispose();
        }

        private void DataGrid_CellGotFocus(object sender, RoutedEventArgs e)
        {
            // Lookup for the source to be DataGridCell
            if (e.OriginalSource.GetType() == typeof(DataGridCell))
            {
                // Starts the Edit on the row;
                DataGrid grd = (DataGrid)sender;
                grd.BeginEdit(e);

                Control control = GetFirstChildByType<Control>(e.OriginalSource as DataGridCell);
                if (control != null)
                {
                    control.Focus();
                }
            }
        }

        private T GetFirstChildByType<T>(DependencyObject prop) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(prop); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild((prop), i) as DependencyObject;
                if (child == null)
                    continue;

                T castedProp = child as T;
                if (castedProp != null)
                    return castedProp;

                castedProp = GetFirstChildByType<T>(child);

                if (castedProp != null)
                    return castedProp;
            }
            return null;
        }

        private void cmbTahun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mulaiFilter)
            {
                if ((cmbKaryawan.SelectedItem).ToString() == "Semua")
                {
                    ShowDataTabel();
                }
                else
                {
                    ShowDataTabelFilter(cmbTahun.SelectedItem.ToString(), (cmbBulan.SelectedIndex + 1).ToString(), (cmbKaryawan.SelectedItem).ToString());
                }

                //Warna();
            }
        }

        void ShowDataTabelFilter(string tahun, string bulan, string namakaryawan)
        {

            try
            {
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                dgTerapi.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_presensi WHERE MONTH(mydentist.tbl_presensi.tanggal_presensi) = " + (cmbBulan.SelectedIndex + 1) + " AND YEAR(mydentist.tbl_presensi.tanggal_presensi) =" + cmbTahun.SelectedItem.ToString() + " AND mydentist.tbl_presensi.nama_presensi='" + (cmbKaryawan.SelectedItem).ToString() + "' ORDER BY id_presensi DESC", null).DefaultView;
                //dgTerapi.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_appointment WHERE MONTH(mydentist.tbl_appointment.tanggal_appo) = " + (cmbBulan.SelectedIndex + 1) + " AND YEAR(mydentist.tbl_appointment.tanggal_appo) =" + cmbTahun.SelectedItem.ToString() + " ORDER BY CAST(mydentist.tbl_appointment.tanggal_appo as datetime),CAST(mydentist.tbl_appointment.jam_appo as time) ASC", null).DefaultView;


                string format = "hh:mm";
                ((DataGridTextColumn)dgTerapi.Columns[0]).Binding = new Binding("id_presensi");
                //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
                ((DataGridTextColumn)dgTerapi.Columns[1]).Binding = new Binding("tanggal_presensi");
                ((DataGridTextColumn)dgTerapi.Columns[1]).Binding.StringFormat = "{0:dd}";
                ((DataGridTextColumn)dgTerapi.Columns[2]).Binding = new Binding("tanggal_presensi");
                ((DataGridTextColumn)dgTerapi.Columns[2]).Binding.StringFormat = "{0:dddd}";
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
                // Harus ditutup !!!
               
                /*
                double TotalJumlah = 0;
                DateTime d = new DateTime();
                for (int x = 0; x < dgTerapi.Items.Count; x++)
                {

                    var rows = (DataGridRow)dgTerapi.ItemContainerGenerator.ContainerFromIndex(x);

                    DataRowView v = (DataRowView)dgTerapi.Items[x];
                    DateTime t = DateTime.Parse((string)v[10].ToString());

                    TotalJumlah = (d.Add(t.TimeOfDay).TimeOfDay).TotalHours;
                }


                totalJumlah.Text = TimeSpan.FromHours(TotalJumlah).ToString();
                */

                //double TotalP1_M1 = (p1.TimeOfDay - m1.TimeOfDay).TotalHours;
                koneksi.Dispose();

            }
            catch (Exception e)
            {
                //Warna();
                MessageBox.Show(e.Message);
                dgTerapi.ItemsSource = null;
                //dgAppo.Items.Refresh();
                koneksi.Dispose();
            }
            //Warna();


        }

        private void cmbBulan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mulaiFilter)
            {
                if ((cmbKaryawan.SelectedItem).ToString() == "Semua")
                {
                    ShowDataTabel();
                }
                else
                {
                    ShowDataTabelFilter(cmbTahun.SelectedItem.ToString(), (cmbBulan.SelectedIndex + 1).ToString(), (cmbKaryawan.SelectedItem).ToString());
                }

                //Warna();
            }
        }

        private void cmbKaryawan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mulaiFilter)
            {
                if((cmbKaryawan.SelectedItem).ToString() == "Semua"){
                    ShowDataTabel();
                }
                else
                {
                    ShowDataTabelFilter(cmbTahun.SelectedItem.ToString(), (cmbBulan.SelectedIndex + 1).ToString(), (cmbKaryawan.SelectedItem).ToString());
                }
                
                //Warna();
            }
        }
    }
}
