using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

    public partial class FormRekapTerapi : UserControl
    {
        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;

        RekapTerapis rekapTerapi = new RekapTerapis();
        private bool mulaiFilter;
        public FormRekapTerapi()
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
        /*
        SELECT Shippers.ShipperName,COUNT(Orders.OrderID) AS NumberOfOrders FROM Orders
        LEFT JOIN Shippers
        ON Orders.ShipperID=Shippers.ShipperID
        GROUP BY ShipperName;
        
        select * From tbl_terapi RIGHT JOIN tbl_rekapterapi ON tbl_terapi.nama_terapi=tbl_rekapterapi.namaterapi_rekapterapi WHERE tbl_rekapterapi.invoice_rekapterapi=1440416276
            */



        /*
        SELECT * FROM mydentist.tbl_terapi RIGHT JOIN mydentist.tbl_rekapterapi ON mydentist.tbl_terapi.nama_terapi=mydentist.tbl_rekapterapi.namaterapi_rekapterapi JOIN mydentist.tbl_rekapperawat rp ON rp.nokwt_rekapperawat = mydentist.tbl_rekapterapi.nokwt_rekapterapi
         */


        void ShowData()
        {
            dgRekapTerapi.ItemsSource = null;
            DataTable CmbxData = new DataTable();
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

            CmbxData = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_terapi RIGHT JOIN mydentist.tbl_rekapterapi ON mydentist.tbl_terapi.nama_terapi=mydentist.tbl_rekapterapi.namaterapi_rekapterapi WHERE MONTH(mydentist.tbl_rekapterapi.tanggal_rekapterapi) = " + (cmbBulan.SelectedIndex + 1), null);

            DataTable CmbxDataPerawat = new DataTable();


            int kj = 0;

            for (int i = 0; i < CmbxData.Rows.Count; i++)
            {
                double rumusTotal1Diskon = (double)CmbxData.Rows[i]["biaya_terapi"] - (((double)CmbxData.Rows[i]["diskon_rekapterapi"] / 100) * (double)CmbxData.Rows[i]["biaya_terapi"]);
                double rumusTotal1Card = rumusTotal1Diskon + (((double)CmbxData.Rows[i]["card_rekapterapi"] / 100) * rumusTotal1Diskon);
                double rumusBiayaBahan = (double)CmbxData.Rows[i]["biaya_terapi"] * ((double)CmbxData.Rows[i]["jenis_terapi"] / 100);
                double totalAll =  rumusTotal1Card;

                double MYHasilDkter = (totalAll - rumusBiayaBahan) / 2;
                string listperawat = string.Empty;

                CmbxDataPerawat = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_rekapperawat WHERE mydentist.tbl_rekapperawat.nokwt_rekapperawat = " + CmbxData.Rows[i]["nokwt_rekapterapi"].ToString(), null);
                {
                    for (int x = 0; x < CmbxDataPerawat.Rows.Count; x++)
                    {
                        listperawat += CmbxDataPerawat.Rows[x]["nama_perawat"].ToString() + ", ";
                    }
                }

                rekapTerapi.Add(new RekapTerapi { 
                                                  KJ = kj,
                                                  NoKWT = CmbxData.Rows[i]["nokwt_rekapterapi"].ToString(), 
                                                  Tanggal = CmbxData.Rows[i]["tanggal_rekapterapi"].ToString(),
                                                  NoRM = CmbxData.Rows[i]["nokwt_rekapterapi"].ToString(),
                                                  NamaPasien = CmbxData.Rows[i]["namapasien_rekapterapi"].ToString(),
                                                  NamaTerapi = CmbxData.Rows[i]["namaterapi_rekapterapi"].ToString(),
                                                  Jenis = (double)CmbxData.Rows[i]["jenis_terapi"],
                                                  Biaya = (double)CmbxData.Rows[i]["biaya_terapi"],
                                                  Diskon = (double)CmbxData.Rows[i]["diskon_rekapterapi"],
                                                  Card = (double)CmbxData.Rows[i]["card_rekapterapi"],
                                                  Total = totalAll,
                                                  NamaDokter = CmbxData.Rows[i]["namadokter_rekapterapi"].ToString(),
                                                  Keterangan = "",
                                                  NamaPerawat = listperawat,
                                                  BiayaBahan = rumusBiayaBahan,
                                                  MY = MYHasilDkter,
                                                  HasilDokter = MYHasilDkter


                });
            }

            dgRekapTerapi.ItemsSource = rekapTerapi;

            koneksi.Dispose();
        }

        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcel<RekapTerapi, RekapTerapis> s = new ExportToExcel<RekapTerapi, RekapTerapis>();
            ICollectionView view = CollectionViewSource.GetDefaultView(dgRekapTerapi.ItemsSource);
            s.dataToPrint = (RekapTerapis)view.SourceCollection;
            s.GenerateReport();
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

            rekapTerapi.Clear();

            try
            {
                
                dgRekapTerapi.ItemsSource = null;
                DataTable CmbxData = new DataTable();
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

                CmbxData = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_terapi RIGHT JOIN mydentist.tbl_rekapterapi ON mydentist.tbl_terapi.nama_terapi=mydentist.tbl_rekapterapi.namaterapi_rekapterapi WHERE MONTH(mydentist.tbl_rekapterapi.tanggal_rekapterapi) = " + (cmbBulan.SelectedIndex + 1) + " AND YEAR(mydentist.tbl_rekapterapi.tanggal_rekapterapi) =" + tahun, null);

                DataTable CmbxDataPerawat = new DataTable();
                int kj = 0;
                for (int i = 0; i < CmbxData.Rows.Count; i++)
                {
                    double rumusTotal1Diskon = (double)CmbxData.Rows[i]["biaya_terapi"] - (((double)CmbxData.Rows[i]["diskon_rekapterapi"] / 100) * (double)CmbxData.Rows[i]["biaya_terapi"]);
                    double rumusTotal1Card = rumusTotal1Diskon + (((double)CmbxData.Rows[i]["card_rekapterapi"] / 100) * rumusTotal1Diskon);
                    double rumusBiayaBahan = (double)CmbxData.Rows[i]["biaya_terapi"] * ((double)CmbxData.Rows[i]["jenis_terapi"] / 100);
                    double totalAll = rumusTotal1Card;

                    double MYHasilDkter = (totalAll - rumusBiayaBahan) / 2;
                    string listperawat = string.Empty;

                    CmbxDataPerawat = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_rekapperawat WHERE mydentist.tbl_rekapperawat.nokwt_rekapperawat = " + CmbxData.Rows[i]["nokwt_rekapterapi"].ToString(), null);
                    {
                        for (int x = 0; x < CmbxDataPerawat.Rows.Count; x++)
                        {
                            listperawat += CmbxDataPerawat.Rows[x]["nama_perawat"].ToString() + ", ";
                        }
                    }

                    rekapTerapi.Add(new RekapTerapi
                    {
                        KJ = kj,
                        NoKWT = CmbxData.Rows[i]["nokwt_rekapterapi"].ToString(),
                        Tanggal = CmbxData.Rows[i]["tanggal_rekapterapi"].ToString(),
                        NoRM = CmbxData.Rows[i]["nokwt_rekapterapi"].ToString(),
                        NamaPasien = CmbxData.Rows[i]["namapasien_rekapterapi"].ToString(),
                        NamaTerapi = CmbxData.Rows[i]["namaterapi_rekapterapi"].ToString(),
                        Jenis = (double)CmbxData.Rows[i]["jenis_terapi"],
                        Biaya = (double)CmbxData.Rows[i]["biaya_terapi"],
                        Diskon = (double)CmbxData.Rows[i]["diskon_rekapterapi"],
                        Card = (double)CmbxData.Rows[i]["card_rekapterapi"],
                        Total = totalAll,
                        NamaDokter = CmbxData.Rows[i]["namadokter_rekapterapi"].ToString(),
                        Keterangan = "",
                        NamaPerawat = listperawat,
                        BiayaBahan = rumusBiayaBahan,
                        MY = MYHasilDkter,
                        HasilDokter = MYHasilDkter


                    });
                }

                dgRekapTerapi.ItemsSource = rekapTerapi;

                koneksi.Dispose();

            }
            catch (Exception e)
            {
                //Warna();

                dgRekapTerapi.ItemsSource = null;
                //dgAppo.Items.Refresh();
                koneksi.Dispose();
            }
            //Warna();


        }

        private void txtPencarian_TextChanged(object sender, TextChangedEventArgs e)
        {
            rekapTerapi.Clear();

            try
            {

                dgRekapTerapi.ItemsSource = null;
                DataTable CmbxData = new DataTable();
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

                CmbxData = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_terapi RIGHT JOIN mydentist.tbl_rekapterapi ON mydentist.tbl_terapi.nama_terapi=mydentist.tbl_rekapterapi.namaterapi_rekapterapi WHERE (MONTH(mydentist.tbl_rekapterapi.tanggal_rekapterapi) = " + (cmbBulan.SelectedIndex + 1) + " AND YEAR(mydentist.tbl_rekapterapi.tanggal_rekapterapi) =" + cmbTahun.SelectedItem.ToString() + ") AND (mydentist.tbl_rekapterapi.namaterapi_rekapterapi LIKE %'"+ txtPencarian.Text +"'%)", null);

                DataTable CmbxDataPerawat = new DataTable();

                int kj = 0;
                for (int i = 0; i < CmbxData.Rows.Count; i++)
                {
                    kj++;
                    double rumusTotal1Diskon = (double)CmbxData.Rows[i]["biaya_terapi"] - (((double)CmbxData.Rows[i]["diskon_rekapterapi"] / 100) * (double)CmbxData.Rows[i]["biaya_terapi"]);
                    double rumusTotal1Card = rumusTotal1Diskon + (((double)CmbxData.Rows[i]["card_rekapterapi"] / 100) * rumusTotal1Diskon);
                    double rumusBiayaBahan = (double)CmbxData.Rows[i]["biaya_terapi"] * ((double)CmbxData.Rows[i]["jenis_terapi"] / 100);
                    double totalAll = rumusTotal1Card;

                    double MYHasilDkter = (totalAll - rumusBiayaBahan) / 2;
                    string listperawat = string.Empty;


                    CmbxDataPerawat = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_rekapperawat WHERE mydentist.tbl_rekapperawat.nokwt_rekapperawat = " + CmbxData.Rows[i]["nokwt_rekapterapi"].ToString(), null);
                    for (int x = 0; x < CmbxDataPerawat.Rows.Count; x++)
                    {
                        listperawat += CmbxDataPerawat.Rows[x]["nama_perawat"].ToString() + ", ";
                    }




                    rekapTerapi.Add(new RekapTerapi
                    {
                        KJ = kj,
                        NoKWT = CmbxData.Rows[i]["nokwt_rekapterapi"].ToString(),
                        Tanggal = CmbxData.Rows[i]["tanggal_rekapterapi"].ToString(),
                        NoRM = CmbxData.Rows[i]["nokwt_rekapterapi"].ToString(),
                        NamaPasien = CmbxData.Rows[i]["namapasien_rekapterapi"].ToString(),
                        NamaTerapi = CmbxData.Rows[i]["namaterapi_rekapterapi"].ToString(),
                        Jenis = (double)CmbxData.Rows[i]["jenis_terapi"],
                        Biaya = (double)CmbxData.Rows[i]["biaya_terapi"],
                        Diskon = (double)CmbxData.Rows[i]["diskon_rekapterapi"],
                        Card = (double)CmbxData.Rows[i]["card_rekapterapi"],
                        Total = totalAll,
                        NamaDokter = CmbxData.Rows[i]["namadokter_rekapterapi"].ToString(),
                        Keterangan = "",
                        NamaPerawat = listperawat,
                        BiayaBahan = rumusBiayaBahan,
                        MY = MYHasilDkter,
                        HasilDokter = MYHasilDkter


                    });
                }

                dgRekapTerapi.ItemsSource = rekapTerapi;

                koneksi.Dispose();

            }
            catch (Exception ex)
            {
                //Warna();

                dgRekapTerapi.ItemsSource = null;
                //dgAppo.Items.Refresh();
                koneksi.Dispose();
            }
            //Warna();
        }


    }

    public class RekapTerapis : List<RekapTerapi> { }

    public class RekapTerapi
    {
        public int KJ { get; set; }
        public string NoKWT { get; set; }
        public string Tanggal { get; set; }
        public string NoRM { get; set; }
        public string NamaPasien { get; set; }
        public string NamaTerapi { get; set; }
        public double Jenis { get; set; }
        public double Biaya { get; set; }
        public double Diskon { get; set; }
        public double Card { get; set; }
        public double Total { get; set; }
        public string NamaDokter { get; set; }
        public string  NamaPerawat { get; set; }
        public string Keterangan { get; set; }
        public double BiayaBahan { get; set; }
        public double MY { get; set; }
        public double HasilDokter { get; set; }
    }

    public class ExportToExcel<T, U>
        where T : class
        where U : List<T>
    {
        public List<T> dataToPrint;
        // Excel object references.
        private Microsoft.Office.Interop.Excel.Application _excelApp = null;
        private Microsoft.Office.Interop.Excel.Workbooks _books = null;
        private Microsoft.Office.Interop.Excel._Workbook _book = null;
        private Microsoft.Office.Interop.Excel.Sheets _sheets = null;
        private Microsoft.Office.Interop.Excel._Worksheet _sheet = null;
        private Microsoft.Office.Interop.Excel.Range _range = null;
        private Microsoft.Office.Interop.Excel.Font _font = null;
        // Optional argument variable
        private object _optionalValue = Missing.Value;

        /// <summary>
        /// Generate report and sub functions
        /// </summary>
        public void GenerateReport()
        {
            try
            {
                if (dataToPrint != null)
                {
                    if (dataToPrint.Count != 0)
                    {
                        Mouse.SetCursor(Cursors.Wait);
                        CreateExcelRef();
                        FillSheet();
                        OpenReport();
                        Mouse.SetCursor(Cursors.Arrow);
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while generating Excel report");
            }
            finally
            {
                ReleaseObject(_sheet);
                ReleaseObject(_sheets);
                ReleaseObject(_book);
                ReleaseObject(_books);
                ReleaseObject(_excelApp);
            }
        }
        /// <summary>
        /// Make MS Excel application visible
        /// </summary>
        private void OpenReport()
        {
            _excelApp.Visible = true;
        }
        /// <summary>
        /// Populate the Excel sheet
        /// </summary>
        private void FillSheet()
        {
            object[] header = CreateHeader();
            WriteData(header);
        }
        /// <summary>
        /// Write data into the Excel sheet
        /// </summary>
        /// <param name="header"></param>
        private void WriteData(object[] header)
        {
            object[,] objData = new object[dataToPrint.Count, header.Length];

            for (int j = 0; j < dataToPrint.Count; j++)
            {
                var item = dataToPrint[j];
                for (int i = 0; i < header.Length; i++)
                {
                    var y = typeof(T).InvokeMember(header[i].ToString(),
                    BindingFlags.GetProperty, null, item, null);
                    objData[j, i] = (y == null) ? "" : y.ToString();
                }
            }

            AddExcelRows("A2", dataToPrint.Count, header.Length, objData);
            AutoFitColumns("A1", dataToPrint.Count + 1, header.Length);
        }
        /// <summary>
        /// Method to make columns auto fit according to data
        /// </summary>
        /// <param name="startRange"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        private void AutoFitColumns(string startRange, int rowCount, int colCount)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.Columns.AutoFit();
        }
        /// <summary>
        /// Create header from the properties
        /// </summary>
        /// <returns></returns>
        private object[] CreateHeader()
        {
            PropertyInfo[] headerInfo = typeof(T).GetProperties();

            // Create an array for the headers and add it to the
            // worksheet starting at cell A1.
            List<object> objHeaders = new List<object>();
            for (int n = 0; n < headerInfo.Length; n++)
            {
                objHeaders.Add(headerInfo[n].Name);
            }

            var headerToAdd = objHeaders.ToArray();
            AddExcelRows("A1", 1, headerToAdd.Length, headerToAdd);
            SetHeaderStyle();

            return headerToAdd;
        }
        /// <summary>
        /// Set Header style as bold
        /// </summary>
        private void SetHeaderStyle()
        {
            _font = _range.Font;
            _font.Bold = true;
        }
        /// <summary>
        /// Method to add an excel rows
        /// </summary>
        /// <param name="startRange"></param>
        /// <param name="rowCount"></param>
        /// <param name="colCount"></param>
        /// <param name="values"></param>
        private void AddExcelRows(string startRange, int rowCount,
        int colCount, object values)
        {
            _range = _sheet.get_Range(startRange, _optionalValue);
            _range = _range.get_Resize(rowCount, colCount);
            _range.set_Value(_optionalValue, values);
        }
        /// <summary>
        /// Create Excel application parameters instances
        /// </summary>
        private void CreateExcelRef()
        {
            _excelApp = new Microsoft.Office.Interop.Excel.Application();
            _books = (Microsoft.Office.Interop.Excel.Workbooks)_excelApp.Workbooks;
            _book = (Microsoft.Office.Interop.Excel._Workbook)(_books.Add(_optionalValue));
            _sheets = (Microsoft.Office.Interop.Excel.Sheets)_book.Worksheets;
            _sheet = (Microsoft.Office.Interop.Excel._Worksheet)(_sheets.get_Item(1));
        }
        /// <summary>
        /// Release unused COM objects
        /// </summary>
        /// <param name="obj"></param>
        private void ReleaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
                MessageBox.Show(ex.Message.ToString());
            }
            finally
            {
                GC.Collect();
            }
        }
    }
}
