using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using MYDENTIST.Form.AmbilData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for FormTransaki.xaml
    /// </summary>
    public partial class FormTransaki : UserControl
    {
        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;

        private string IDPasien;

        private decimal totalObat;
        private decimal totalTerapi;

        public FormTransaki()
        {
            InitializeComponent();
            datePick.SelectedDate = DateTime.Today;
            DataDokter();
            ShowPerawat();
            ((DataGridTextColumn)dgTerapi.Columns[3]).Binding.StringFormat = "{0:C2}";
            ((DataGridTextColumn)dgObat.Columns[4]).Binding.StringFormat = "{0:C2}";
            ((DataGridTextColumn)dgObat.Columns[5]).Binding.StringFormat = "{0:C2}";
        }

        private void btnCari_Click(object sender, RoutedEventArgs e)
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


        void ShowPerawat()
        {
            try
            {
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                dgPerawat.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_karyawan WHERE mydentist.tbl_karyawan.jenis_karyawan = 'Perawat'", null).DefaultView;

                ((DataGridTextColumn)dgPerawat.Columns[0]).Binding = new Binding("id_karyawan");
                //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_karyawan");
                ((DataGridTextColumn)dgPerawat.Columns[1]).Binding = new Binding("nama_karyawan");
                //((DataGridCheckBoxColumn)dgPerawat.Columns[3]).Binding = new Binding("jenis_karyawan");


                // Harus ditutup !!!
                koneksi.Dispose();
            }
            catch (Exception e)
            {

            }
        }

        int xNomor;
        private void AddItemCallbackAmbilDataTerapi(string persen, string nama, double biaya)
        {
            /*
            DataTable dt = new DataTable();
            dt.Columns.Add("NamaTerapi");
            dt.Columns.Add("Biaya");
            DataRow newRow = dt.NewRow();

            newRow["NamaTerapi"] = nama;
            newRow["Biaya"] = biaya;
            //dt.Rows.Add(newRow);
            dt.ImportRow(newRow);
            dgTerapi.ItemsSource = dt.DefaultView;
             * */
            double TotalTerapi = 0;
            dgTerapi.Items.Add(new DataTerapi { NamaTerapi = nama, Biaya = biaya  });
            dgTerapi.UpdateLayout();
            for (int x = 0; x < dgTerapi.Items.Count;x++ )
            {

                var row = (DataGridRow)dgTerapi.ItemContainerGenerator.ContainerFromIndex(x);

                DataTerapi v = (DataTerapi)dgTerapi.Items[row.GetIndex()];
                //DataGridCell cell = DataGridHelper.GetCell(dgTerapi, x, 3);

                //Console.WriteLine(v.Row.ItemArray[0]);
                TotalTerapi += v.Biaya;
                //Console.WriteLine(v.Biaya);
            }



            txtTotalTerapi.Text = TotalTerapi.ToString();
        }


        List<DataObat> myDataItems = new List<DataObat>(); 
        private void AddItemCallbackAmbilDataObat(int id, int persen, string nama, int biaya)
        {
            int TotalObat = 0;
            dgObat.ItemsSource = null;
            myDataItems.Add(new DataObat {ID = id, QTY = 1, NamaObat = nama, Biaya = biaya});
            


            //dgObat.Items.Add(new DataObat { QTY = 1, NamaObat = nama, Biaya = biaya });
            
            dgObat.ItemsSource = myDataItems;
            //DataGridCell cell = DataGridHelper.GetCell(dgObat, 0, 5);
            //MessageBox.Show(cell.va);
            //MessageBox.Show(dgObat.Items.Count.ToString());
            dgObat.UpdateLayout();
            for (int x = 0; x < dgObat.Items.Count; x++)
            {

                //var row = (DataGridRow)dgObat.ItemContainerGenerator.ContainerFromIndex(x);

                //DataRowView v = (DataRowView)dgObat.Items[row.GetIndex()];
                DataGridCell cell = DataGridHelper.GetCell(dgObat, x, 5);
                TextBlock tb = cell.Content as TextBlock;
                ///Console.WriteLine(cell);
                ///
                //MessageBox.Show(cell.ToString());
                string value = tb.Text.Replace("Rp", "").Replace(".", "").Replace(",00", "");
                //MessageBox.Show(value);
                TotalObat += int.Parse(value);
                //Console.WriteLine(tb.Text);
            }


            txtTotalObat.Text = TotalObat.ToString();


        }


        private void dgObat_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            int TotalObat  = 0;

            dgObat.UpdateLayout();
            for (int x = 0; x < dgObat.Items.Count; x++)
            {

                //var row = (DataGridRow)dgObat.ItemContainerGenerator.ContainerFromIndex(x);

                //DataRowView v = (DataRowView)dgObat.Items[row.GetIndex()];
                DataGridCell cell = DataGridHelper.GetCell(dgObat, x, 5);
                TextBlock tb = cell.Content as TextBlock;
                ///Console.WriteLine(cell);
                ///
                //MessageBox.Show(cell.ToString());

                string value = tb.Text.Replace("Rp", "").Replace(".", "").Replace(",00", "");
                TotalObat += int.Parse(value);
                
            }


            txtTotalObat.Text = TotalObat.ToString();

        }


        DataTable CmbxData = new DataTable();
        void DataDokter()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

            
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

        private void btn_tambahTerapi_Click(object sender, RoutedEventArgs e)
        {
            


            FormCariTerapi formCariPasien = new FormCariTerapi();
            formCariPasien.AddItemCallbackTerapi = new AddItemDelegateAmbilDataTerapi(this.AddItemCallbackAmbilDataTerapi);
            formCariPasien.ShowDialog();
        }

        private void btnHapus_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Yakin menghapus item ini?", "Konfirmasi", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var button = (FrameworkElement)sender;
                var row = (DataGridRow)button.Tag;
                dgTerapi.Items.RemoveAt(row.GetIndex());
                double TotalTerapi = 0;
                dgTerapi.UpdateLayout();
                for (int x = 0; x < dgTerapi.Items.Count; x++)
                {

                    var rows = (DataGridRow)dgTerapi.ItemContainerGenerator.ContainerFromIndex(x);

                    DataTerapi v = (DataTerapi)dgTerapi.Items[rows.GetIndex()];
                    //DataGridCell cell = DataGridHelper.GetCell(dgTerapi, x, 3);

                    //Console.WriteLine(v.Row.ItemArray[0]);
                    TotalTerapi += v.Biaya;
                    //Console.WriteLine(v.Biaya);
                }



                txtTotalTerapi.Text = TotalTerapi.ToString();

                decimal TotalGrand = totalTerapi + totalObat;
                //txtGrandTotal.Text = TotalGrand.ToString();
                txtGrandTotal.Text = HitungGrandTotal(Convert.ToDouble(TotalGrand), double.Parse(txtDiskon.Text), double.Parse(txtCard.Text)).ToString();
            }
        }

        private void btnHapusObat_Click(object sender, RoutedEventArgs e)
        {

            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            MessageBoxResult result = MessageBox.Show("Yakin menghapus item ini?", "Konfirmasi", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                DataObat v = ((DataObat)(dgObat.SelectedItem));
                myDataItems.Remove(v);
                dgObat.ItemsSource = null;
                int TotalObat = 0;
                dgObat.ItemsSource = myDataItems;
                dgObat.UpdateLayout();
                for (int x = 0; x < dgObat.Items.Count; x++)
                {

                    //var row = (DataGridRow)dgObat.ItemContainerGenerator.ContainerFromIndex(x);

                    //DataRowView v = (DataRowView)dgObat.Items[row.GetIndex()];
                    DataGridCell cell = DataGridHelper.GetCell(dgObat, x, 5);
                    TextBlock tb = cell.Content as TextBlock;
                    ///Console.WriteLine(cell);
                    ///
                    //MessageBox.Show(cell.ToString());
                    string value = tb.Text.Replace("Rp", "").Replace(".", "").Replace(",00", "");
                    TotalObat += int.Parse(value);

                }

                txtTotalObat.Text = TotalObat.ToString();

                decimal TotalGrand = totalTerapi + totalObat;
                //txtGrandTotal.Text = TotalGrand.ToString();

                txtGrandTotal.Text = HitungGrandTotal(Convert.ToDouble(TotalGrand), double.Parse(txtDiskon.Text), double.Parse(txtCard.Text)).ToString();

            }

        }
        public struct DataTerapi
        {
            public int No { set; get; }
            public string NamaTerapi { set; get; }
            public double Biaya { set; get; }
        }

        public class DataObat : INotifyPropertyChanged
        {
            public int ID { set; get; }

            private int qty;
            private int biaya;
            public string NamaObat { set; get; }
            
            public int QTY {
                get { return qty; }
                set
                {
                    this.qty = value;
                    OnPropertyChanged("SubTotal");
                }
            }

            public int Biaya
            {
                get { return biaya; }
                set
                {
                    this.biaya = value;
                    OnPropertyChanged("SubTotal");
                }
            }


            public int SubTotal
            {
                get { return QTY * Biaya; }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler != null)
                    handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }


        static class DataGridHelper
        {
            static public DataGridCell GetCell(DataGrid dg, int row, int column)
            {
                DataGridRow rowContainer = GetRow(dg, row);

                if (rowContainer != null)
                {
                    DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);

                    // try to get the cell but it may possibly be virtualized
                    DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                    if (cell == null)
                    {
                        // now try to bring into view and retreive the cell
                        dg.ScrollIntoView(rowContainer, dg.Columns[column]);
                        cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(column);
                    }
                    return cell;
                }
                return null;
            }

            static public DataGridRow GetRow(DataGrid dg, int index)
            {
                DataGridRow row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(index);
                if (row == null)
                {
                    // may be virtualized, bring into view and try again
                    dg.ScrollIntoView(dg.Items[index]);
                    row = (DataGridRow)dg.ItemContainerGenerator.ContainerFromIndex(index);
                }
                return row;
            }

            static T GetVisualChild<T>(Visual parent) where T : Visual
            {
                T child = default(T);
                int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
                for (int i = 0; i < numVisuals; i++)
                {
                    Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                    child = v as T;
                    if (child == null)
                    {
                        child = GetVisualChild<T>(v);
                    }
                    if (child != null)
                    {
                        break;
                    }
                }
                return child;
            }
        }

        void dgTerapi_LoadingRow(object sender, System.Windows.Controls.DataGridRowEventArgs e)
        {
            dgTerapi.ScrollIntoView(e.Row.Item);
        }

        private void txtTotalTerapi_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value = txtTotalTerapi.Text.Replace("Rp", "").Replace(".", "").Replace(",", "");
            //.Replace("Rp", "").Replace(",", "").TrimStart('0');

            decimal ul;
            //Check we are indeed handling a number
            if (decimal.TryParse(value, out ul))
            {
                totalTerapi = ul;
                //biayaBeliAngka = ul;
                //ul /=  100;
                //Unsub the event so we don't enter a loop
                txtTotalTerapi.TextChanged -= txtTotalTerapi_TextChanged;
                //Format the text as currency
                txtTotalTerapi.Text = string.Format(CultureInfo.CreateSpecificCulture("id-ID"), "{0:C0}", ul);
                txtTotalTerapi.TextChanged += txtTotalTerapi_TextChanged;
                txtTotalTerapi.Select(txtTotalTerapi.Text.Length, 0);
            }

            decimal TotalGrand = totalTerapi + totalObat;

            if (TotalGrand > 0)
                txtGrandTotal.Text = HitungGrandTotal(Convert.ToDouble(TotalGrand), double.Parse(txtDiskon.Text), double.Parse(txtCard.Text)).ToString();
               // txtGrandTotal.Text = TotalGrand.ToString();
        }

        private void btn_tambahObat_Click(object sender, RoutedEventArgs e)
        {
            FormCariObat formCariPasien = new FormCariObat();
            formCariPasien.AddItemCallbackObat = new AddItemDelegateAmbilDataObat(this.AddItemCallbackAmbilDataObat);
            formCariPasien.ShowDialog();
        }

        private void txtTotalObat_TextChanged(object sender, TextChangedEventArgs e)
        {
            string value = txtTotalObat.Text.Replace("Rp", "").Replace(".", "").Replace(",", "");
            //.Replace("Rp", "").Replace(",", "").TrimStart('0');

            decimal ul;
            //Check we are indeed handling a number
            if (decimal.TryParse(value, out ul))
            {
                totalObat = ul;
                //ul /=  100;
                //Unsub the event so we don't enter a loop
                txtTotalObat.TextChanged -= txtTotalObat_TextChanged;
                //Format the text as currency
                txtTotalObat.Text = string.Format(CultureInfo.CreateSpecificCulture("id-ID"), "{0:C0}", ul);
                txtTotalObat.TextChanged += txtTotalObat_TextChanged;
                txtTotalObat.Select(txtTotalObat.Text.Length, 0);
            }

            decimal TotalGrand = totalTerapi + totalObat;

            if (TotalGrand > 0)
                txtGrandTotal.Text = HitungGrandTotal(Convert.ToDouble(TotalGrand), double.Parse(txtDiskon.Text), double.Parse(txtCard.Text)).ToString();

                //txtGrandTotal.Text = TotalGrand.ToString();

        }

        private void txtGrandTotal_TextChanged(object sender, TextChangedEventArgs e)
        {


            string value = txtGrandTotal.Text.Replace("Rp", "").Replace(".", "").Replace(",", "");
            //.Replace("Rp", "").Replace(",", "").TrimStart('0');

            decimal ul;
            //Check we are indeed handling a number
            if (decimal.TryParse(value, out ul))
            {
                //totalObat = ul;
                //ul /=  100;
                //Unsub the event so we don't enter a loop
                txtGrandTotal.TextChanged -= txtGrandTotal_TextChanged;
                //Format the text as currency
                txtGrandTotal.Text = string.Format(CultureInfo.CreateSpecificCulture("id-ID"), "{0:C0}", ul);
                txtGrandTotal.TextChanged += txtGrandTotal_TextChanged;
                txtGrandTotal.Select(txtGrandTotal.Text.Length, 0);
            }
        }


        private double HitungGrandTotal(double total, double diskon, double card)
        {
            // (total - diskon(dari total) + card(total - diskon) )
            double hitungDiskon = ((diskon / 100) * total);
            double hitungCard = ((card / 100) * (total - hitungDiskon));

            //grandTotal = (total - hitungDiskon + hitungCard);

            return Math.Round((total - hitungDiskon + hitungCard),0,MidpointRounding.AwayFromZero);
        }

        private void txtDiskon_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal TotalGrand = totalTerapi + totalObat;

            if (txtDiskon.Text == string.Empty)
                txtDiskon.Text = "0";

            if (TotalGrand > 0)
             txtGrandTotal.Text = HitungGrandTotal(Convert.ToDouble(TotalGrand), double.Parse(txtDiskon.Text), double.Parse(txtCard.Text)).ToString();
        }

        private void txtCard_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal TotalGrand = totalTerapi + totalObat;

            if (txtCard.Text == string.Empty)
                txtCard.Text = "0";

            if (TotalGrand > 0)
                txtGrandTotal.Text = HitungGrandTotal(Convert.ToDouble(TotalGrand), double.Parse(txtDiskon.Text), double.Parse(txtCard.Text)).ToString();
        }

        private void Status_Click(object sender, RoutedEventArgs e)
        {

            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;
            ((CheckBox)sender).IsChecked = ((CheckBox)sender).IsChecked;
            
            for (int i = 0; i < dgPerawat.Items.Count; i++)
            {
                DataGridRow rows = (DataGridRow)dgPerawat.ItemContainerGenerator.ContainerFromIndex(i);
                CheckBox checkBox = FindChild<CheckBox>(rows, "chkSelectAll");
                checkBox.IsChecked = ((CheckBox)sender).IsChecked;

                if (checkBox != null && checkBox.IsChecked == true)
                {
                    //e.Row.Background = new SolidColorBrush(Colors.Red);
                    //row.Background = new SolidColorBrush(Colors.GreenYellow);
                    //MessageBox.Show("AAA");
                }
            }
        }

        public static T FindChild<T>(DependencyObject parent, string childName)
           where T : DependencyObject
        {
            // Confirm parent is valid.  
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                // If the child is not of the request child type child 
                T childType = child as T;
                if (childType == null)
                {
                    // recursively drill down the tree 
                    foundChild = FindChild<T>(child, childName);

                    // If the child is found, break so we do not overwrite the found child.  
                    if (foundChild != null) break;
                }
                else if (!string.IsNullOrEmpty(childName))
                {
                    var frameworkElement = child as FrameworkElement;
                    // If the child's name is set for search 
                    if (frameworkElement != null && frameworkElement.Name == childName)
                    {
                        // if the child's name is of the request name 
                        foundChild = (T)child;
                        break;
                    }
                }
                else
                {
                    // child element found. 
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }

        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {

            if (txtKWT.Text != string.Empty && txtNamaPasien.Text != string.Empty && cmbNamaDokter.SelectedIndex != -1)
            {

                if (dgTerapi.Items.Count > 0 || dgObat.Items.Count > 0)
                {
                    string invoice_order = UnixTimeNow().ToString();
                    string grandTotal = txtGrandTotal.Text.Replace("Rp", "").Replace(".", "").Replace(",", "");
                    string totalObat = txtTotalObat.Text.Replace("Rp", "").Replace(".", "").Replace(",", "");
                    string totalRekap = txtTotalTerapi.Text.Replace("Rp", "").Replace(".", "").Replace(",", "");

                    //Terapi
                    for (int x = 0; x < dgTerapi.Items.Count; x++)
                    {

                        var row = (DataGridRow)dgTerapi.ItemContainerGenerator.ContainerFromIndex(x);
                        DataTerapi v = (DataTerapi)dgTerapi.Items[row.GetIndex()];


                        koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                        // ParameterData dalam bentuk Array (Menyesuakian Database)
                        param = new ParameterData[] { new ParameterData("invoice_rekapterapi", invoice_order),
                                          new ParameterData("tanggal_rekapterapi",  datePick.SelectedDate),
                                          new ParameterData("nokwt_rekapterapi",  txtKWT.Text),
                                          new ParameterData("namaterapi_rekapterapi", v.NamaTerapi),
                                          new ParameterData("namapasien_rekapterapi", txtNamaPasien.Text),
                                          new ParameterData("namadokter_rekapterapi", cmbNamaDokter.Text),
                                          new ParameterData("qty_rekapterapi", 1),
                                          new ParameterData("card_rekapterapi", txtCard.Text),
                                          new ParameterData("diskon_rekapterapi", txtDiskon.Text),
                                          new ParameterData("total_rekapterapi", totalRekap),
                                          new ParameterData("grandtotal_rekapterapi", grandTotal)};

                        koneksi.InsertRow(SettingHelper.database, "tbl_rekapterapi", true, param);

                        // Penting ketika melakukan fungsi InsertRow, kalau tidak dicommit data gk akan masuk ke database
                        koneksi.Commit(true);

                        // melaksanakan fungsi delegate

                        koneksi.Dispose();

                    }

                    for (int x = 0; x < dgObat.Items.Count; x++)
                    {

                        var row = (DataGridRow)dgObat.ItemContainerGenerator.ContainerFromIndex(x);
                        DataObat v = (DataObat)dgObat.Items[row.GetIndex()];


                        koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                        // ParameterData dalam bentuk Array (Menyesuakian Database)
                        param = new ParameterData[] { new ParameterData("invoice_rekapobat", invoice_order),
                                          new ParameterData("tanggal_rekapobat",  datePick.SelectedDate),
                                          new ParameterData("nokwt_rekapobat",  txtKWT.Text),
                                          new ParameterData("namapasien_rekapobat", txtNamaPasien.Text),
                                          new ParameterData("namadokter_rekapobat", cmbNamaDokter.Text),
                                          new ParameterData("namaobat_rekapobat", v.NamaObat),
                                          new ParameterData("qty_rekapobat", v.QTY),
                                          new ParameterData("card_rekapobat", txtCard.Text),
                                          new ParameterData("diskon_rekapobat", txtCard.Text),
                                          new ParameterData("total_rekapobat", totalObat ),
                                          new ParameterData("grandtotal_rekapobat", grandTotal)};

                        koneksi.InsertRow(SettingHelper.database, "tbl_rekapobat", true, param);

                        // Penting ketika melakukan fungsi InsertRow, kalau tidak dicommit data gk akan masuk ke database
                        koneksi.Commit(true);


                        //pengurangan stok
                        DataTable Datatable = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_obat WHERE mydentist.tbl_obat.id_obat =" + v.ID, null);

                        int stokakhir = 0;

                        foreach (DataRow rows in Datatable.Rows)
                        {
                            stokakhir = (int)rows["stok_obat"] - v.QTY;
                        }

                        param = new ParameterData[] { new ParameterData("stok_obat", stokakhir) };
                        koneksi.UpdateRow(SettingHelper.database, "tbl_obat", "id_obat=" + v.ID, 0, param);
                        koneksi.Commit(true);

                        koneksi.Dispose();

                    }

                    for (int i = 0; i < dgPerawat.Items.Count; i++)
                    {
                        DataGridRow rows = (DataGridRow)dgPerawat.ItemContainerGenerator.ContainerFromIndex(i);
                        CheckBox checkBox = FindChild<CheckBox>(rows, "chkSelectAll");
                        //checkBox.IsChecked = ((CheckBox)sender).IsChecked;

                        if (checkBox != null && checkBox.IsChecked == true)
                        {
                            DataRowView v = (DataRowView)dgPerawat.Items[rows.GetIndex()];

                            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                            // ParameterData dalam bentuk Array (Menyesuakian Database)
                            param = new ParameterData[] { new ParameterData("nama_perawat", (string)v[1].ToString()),
                                          new ParameterData("id_perawat",  (string)v[0].ToString()),
                                          new ParameterData("tanggal_rekapperawat",  datePick.SelectedDate),
                                          new ParameterData("invoice_rekapperawat", invoice_order),
                                          new ParameterData("nokwt_rekapperawat", txtKWT.Text)};

                            koneksi.InsertRow(SettingHelper.database, "tbl_rekapperawat", true, param);

                            // Penting ketika melakukan fungsi InsertRow, kalau tidak dicommit data gk akan masuk ke database
                            koneksi.Commit(true);
                            koneksi.Dispose();
                        }
                    }

                    MessageBox.Show("Data transaki berhasil disimpan", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearAll();
                }
                else
                {
                    MessageBox.Show("Mohon data field diisi !", "Informasi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("Mohon data field diisi !", "Informasi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void btnBatal_Click(object sender, RoutedEventArgs e)
        {
             MessageBoxResult result = MessageBox.Show("Yakin membatalkan transaksi?", "Konfirmasi", MessageBoxButton.YesNo);

             if (result == MessageBoxResult.Yes)
             {

                 ClearAll();

             }


        }


        public long UnixTimeNow()
        {
            var timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalSeconds;
        }

        void ClearAll()
        {
            txtKWT.Text = string.Empty;
            txtNamaPasien.Text = string.Empty;
            cmbNamaDokter.SelectedIndex = -1;
            dgTerapi.Items.Clear();
            myDataItems.Clear();
            dgObat.ItemsSource = null;
            txtTotalTerapi.Text = "0";
            txtTotalObat.Text = "0";
            txtGrandTotal.Text = "0";
            txtNoRm.Text = string.Empty;

            for (int i = 0; i < dgPerawat.Items.Count; i++)
            {
                DataGridRow rows = (DataGridRow)dgPerawat.ItemContainerGenerator.ContainerFromIndex(i);
                CheckBox checkBox = FindChild<CheckBox>(rows, "chkSelectAll");
                checkBox.IsChecked = false;
            }
        }

    }


}
