using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for FormRekapObat.xaml
    /// </summary>
    public partial class FormRekapObat : UserControl
    {

        RekapObats rekapObats = new RekapObats();
        private bool mulaiFilter;


        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;

        public FormRekapObat()
        {
            InitializeComponent();

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

            cmbTahun.SelectedValue = DateTime.Now.ToString("yyyy");
            cmbBulan.SelectedValue = DateTime.Now.ToString("MMMM");

            mulaiFilter = true;
            ShowData();
        }


        private void ShowData()
        {
            rekapObats.Clear();
            dgRekapData.ItemsSource = null;
            DataTable CmbxData = new DataTable();
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

            CmbxData = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_obat RIGHT JOIN mydentist.tbl_rekapobat ON mydentist.tbl_rekapobat.namaobat_rekapobat=mydentist.tbl_obat.nama_obat WHERE MONTH(mydentist.tbl_rekapobat.tanggal_rekapobat) = " + (cmbBulan.SelectedIndex + 1), null);

            DataTable CmbxDataPerawat = new DataTable();

            for (int i = 0; i < CmbxData.Rows.Count; i++)
            {

                DateTime dt = DateTime.Parse(CmbxData.Rows[i]["tanggal_rekapobat"].ToString());
                string format = "dd MMMM yyyy";

                rekapObats.Add(new RekapObat
                {
                            Tanggal = dt.ToString(format),
                            NamaPasien = CmbxData.Rows[i]["namapasien_rekapobat"].ToString(),
                            NamaObat = CmbxData.Rows[i]["namaobat_rekapobat"].ToString(),
                            Jenis = "Obat",
                            QTY = (int)CmbxData.Rows[i]["qty_rekapobat"],
                            Tarif = (double)CmbxData.Rows[i]["total_rekapobat"]
                });
            }

            dgRekapData.ItemsSource = rekapObats;

            koneksi.Dispose();
        }

        private void cmbTahun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mulaiFilter)
            {
                ShowDataTabelFilter(cmbTahun.SelectedItem.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
            }
        }

        private void cmbBulan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mulaiFilter)
            {
                ShowDataTabelFilter(cmbTahun.SelectedItem.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
            }
        }

        void ShowDataTabelFilter(string tahun, string bulan)
        {

            rekapObats.Clear();
            try{
                dgRekapData.ItemsSource = null;
                DataTable CmbxData = new DataTable();
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

                CmbxData = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_obat RIGHT JOIN mydentist.tbl_rekapobat ON mydentist.tbl_rekapobat.namaobat_rekapobat=mydentist.tbl_obat.nama_obat WHERE MONTH(mydentist.tbl_rekapobat.tanggal_rekapobat) = " + (cmbBulan.SelectedIndex + 1) + " AND YEAR(mydentist.tbl_rekapobat.tanggal_rekapobat) =" + tahun, null);

                DataTable CmbxDataPerawat = new DataTable();

                for (int i = 0; i < CmbxData.Rows.Count; i++)
                {

                    DateTime dt = DateTime.Parse(CmbxData.Rows[i]["tanggal_rekapobat"].ToString());
                    string format = "dd MMMM yyyy";

                    rekapObats.Add(new RekapObat
                    {
                                    Tanggal = dt.ToString(format),
                                    NamaPasien = CmbxData.Rows[i]["namapasien_rekapobat"].ToString(),
                                    NamaObat = CmbxData.Rows[i]["namaobat_rekapobat"].ToString(),
                                    Jenis = "Obat",
                                    QTY = (int)CmbxData.Rows[i]["qty_rekapobat"],
                                    Tarif = (double)CmbxData.Rows[i]["total_rekapobat"]
                    });
                }

                dgRekapData.ItemsSource = rekapObats;

                koneksi.Dispose();

            }
            catch (Exception ex)
            {
                //Warna();

                dgRekapData.ItemsSource = null;
                //dgAppo.Items.Refresh();
                koneksi.Dispose();
            }
            //Warna();


        }

        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                ExportToExcel<RekapObat, RekapObats> s = new ExportToExcel<RekapObat, RekapObats>();
                ICollectionView view = CollectionViewSource.GetDefaultView(dgRekapData.ItemsSource);
                s.dataToPrint = (RekapObats)view.SourceCollection;
                s.GenerateReport();

            }catch(Exception ex){

            }
        }

        private void txtPencarian_TextChanged(object sender, TextChangedEventArgs e)
        {


            rekapObats.Clear();
            try
            {
                dgRekapData.ItemsSource = null;
                DataTable CmbxData = new DataTable();
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

                CmbxData = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_obat RIGHT JOIN mydentist.tbl_rekapobat ON mydentist.tbl_rekapobat.namaobat_rekapobat=mydentist.tbl_obat.nama_obat WHERE (MONTH(mydentist.tbl_rekapobat.tanggal_rekapobat) = " + (cmbBulan.SelectedIndex + 1) + " AND YEAR(mydentist.tbl_rekapobat.tanggal_rekapobat) =" + cmbTahun.SelectedItem.ToString() + ") AND (mydentist.tbl_rekapobat.namapasien_rekapobat LIKE '%" + txtPencarian.Text + "%' OR mydentist.tbl_rekapobat.namaobat_rekapobat LIKE '%" + txtPencarian.Text + "%' )", null);

                DataTable CmbxDataPerawat = new DataTable();

                for (int i = 0; i < CmbxData.Rows.Count; i++)
                {
                    DateTime dt = DateTime.Parse(CmbxData.Rows[i]["tanggal_rekapobat"].ToString());
                    string format = "dd MMMM yyyy";

                    rekapObats.Add(new RekapObat
                    {
                        Tanggal = dt.ToString(format),
                        NamaPasien = CmbxData.Rows[i]["namapasien_rekapobat"].ToString(),
                        NamaObat = CmbxData.Rows[i]["namaobat_rekapobat"].ToString(),
                        Jenis = "Obat",
                        QTY = (int)CmbxData.Rows[i]["qty_rekapobat"],
                        Tarif = (double)CmbxData.Rows[i]["total_rekapobat"]
                    });
                }

                dgRekapData.ItemsSource = rekapObats;

                koneksi.Dispose();

                if (txtPencarian.Text == string.Empty)
                {
                    ShowData();
                }

            }
            catch (Exception ex)
            {
                //Warna();

                dgRekapData.ItemsSource = null;
                //dgAppo.Items.Refresh();
                koneksi.Dispose();

            }
            //Warna();
        }

    }

    public class RekapObats : List<RekapObat> { }

    public class RekapObat
    {
        public int No { get; set; }
        public string Tanggal { get; set; }
        public string NamaPasien { get; set; }
        public string NamaObat { get; set; }
        public string Jenis { get; set; }
        public int QTY { get; set; }
        public double Tarif { get; set; }
        public int Stock { get; set; }
    }


}
