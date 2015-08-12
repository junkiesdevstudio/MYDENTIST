using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using MYDENTIST.Form.PopUpData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
    /// Interaction logic for FormAppointment.xaml
    /// </summary>
    public partial class FormAppointment : UserControl
    {

        private cds_MYSQLKonektor koneksi;
        private ParameterData param;
        private bool mulaiFilter;
        public FormAppointment()
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

            ShowDataTabel();
            mulaiFilter = true;
        }

        void ShowDataTabel()
        {
            try
            {
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                dgAppo.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_appointment WHERE MONTH(mydentist.tbl_appointment.tanggal_appo) = " + (cmbBulan.SelectedIndex + 1) + " AND YEAR(mydentist.tbl_appointment.tanggal_appo) =" + cmbTahun.SelectedItem.ToString(), null).DefaultView;

                ((DataGridTextColumn)dgAppo.Columns[0]).Binding = new Binding("id_appo");
                //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
                ((DataGridTextColumn)dgAppo.Columns[2]).Binding = new Binding("tanggal_appo");
                ((DataGridTextColumn)dgAppo.Columns[2]).Binding.StringFormat = "{0:dd MMMM yyyy}";
                ((DataGridTextColumn)dgAppo.Columns[3]).Binding = new Binding("jam_appo");
                ((DataGridTextColumn)dgAppo.Columns[4]).Binding = new Binding("norm_appo");
                ((DataGridTextColumn)dgAppo.Columns[5]).Binding = new Binding("namapasien_appo");
                ((DataGridTextColumn)dgAppo.Columns[6]).Binding = new Binding("namadokter_appo");



                //((DataGridCheckBoxColumn)dgAppo.Columns[7]).Binding = new Binding("status_appo") { Converter = new ItemCodeToBoolConverter() };
                ((DataGridTextColumn)dgAppo.Columns[8]).Binding = new Binding("keterangan_appo");

                //@Bahar : Harus ditutup !!!
                koneksi.Dispose();
            }
            catch (Exception e)
            {
                dgAppo.ItemsSource = null;
                //dgAppo.Items.Refresh();
                koneksi.Dispose();
            }

            //Warna();
        }

        void ShowDataTabelFilter(string tahun, string bulan)
        {

            try
            {
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                dgAppo.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_appointment WHERE MONTH(mydentist.tbl_appointment.tanggal_appo) = " + bulan + " AND YEAR(mydentist.tbl_appointment.tanggal_appo) =" + tahun, null).DefaultView;

                ((DataGridTextColumn)dgAppo.Columns[0]).Binding = new Binding("id_appo");
                //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
                ((DataGridTextColumn)dgAppo.Columns[2]).Binding = new Binding("tanggal_appo");
                ((DataGridTextColumn)dgAppo.Columns[2]).Binding.StringFormat = "{0:dd MMMM yyyy}";
                ((DataGridTextColumn)dgAppo.Columns[3]).Binding = new Binding("jam_appo");
                ((DataGridTextColumn)dgAppo.Columns[4]).Binding = new Binding("norm_appo");
                ((DataGridTextColumn)dgAppo.Columns[5]).Binding = new Binding("namapasien_appo");
                ((DataGridTextColumn)dgAppo.Columns[6]).Binding = new Binding("namadokter_appo");
                //((DataGridCheckBoxColumn)dgAppo.Columns[7]).Binding = new Binding("status_appo") { Converter = new ItemCodeToBoolConverter() };
                ((DataGridTextColumn)dgAppo.Columns[8]).Binding = new Binding("keterangan_appo");
                //Warna();
                //@Bahar : Harus ditutup !!!
                koneksi.Dispose();
                

            }
            catch (Exception e)
            {
                //Warna();

                dgAppo.ItemsSource = null; 
                //dgAppo.Items.Refresh();
                koneksi.Dispose();
            }
            //Warna();


        }

        private void cmbTahun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mulaiFilter)
            {
                //ShowDataTabelFilter(((ComboBoxItem)cmbTahun.SelectedItem).Content.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
                ShowDataTabelFilter(cmbTahun.SelectedItem.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
                //Warna();
            }
        }

        private void cmbBulan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mulaiFilter)
            {
                //ShowDataTabelFilter(((ComboBoxItem)cmbTahun.SelectedItem).Content.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
                ShowDataTabelFilter(cmbTahun.SelectedItem.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
               // Warna();
            }
        }

        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {
            PopUpDataAppointment popUpDataAppointment = new PopUpDataAppointment();
            popUpDataAppointment.AddItemCallback = new AddItemDelegatePopUpDataAppointment(this.AddItemCallbackPopUpDataAppointment);
            popUpDataAppointment.ShowDialog();
        }

        private void AddItemCallbackPopUpDataAppointment()
        {
            ShowDataTabel();
            //MessageBox.Show("Test");
        }


        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            if (dgAppo.SelectedCells.Count > 0)
            {
                //MessageBox.Show(GetIndexKaryawan(row));

                PopUpDataAppointment popUpDataAppointment = new PopUpDataAppointment(GetIndexKaryawan(row));
                popUpDataAppointment.AddItemCallback = new AddItemDelegatePopUpDataAppointment(this.AddItemCallbackPopUpDataAppointment);
                popUpDataAppointment.ShowDialog();
            }

            //Warna();
        }

        private void btnHapus_Click(object sender, RoutedEventArgs e)
        {
            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;

            if (dgAppo.SelectedCells.Count > 0)
            {
                MessageBoxResult result = MessageBox.Show("Hapus Data Appointment?", "Konfirmasi", MessageBoxButton.YesNo);


                if (result == MessageBoxResult.Yes)
                {

                    koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                    koneksi.SendQuery("DELETE FROM mydentist.tbl_appointment WHERE mydentist.tbl_appointment.id_appo = " + GetIndexKaryawan(row), null);
                    koneksi.Commit(true);

                    ShowDataTabel();

                    koneksi.Dispose();
                }

            }

            //Warna();
        }

        private string GetIndexKaryawan(DataGridRow row)
        {
            DataRowView v = (DataRowView)dgAppo.Items[row.GetIndex()];
            return (string)v[0].ToString();
        }



        private void txtPencarian_TextChanged(object sender, TextChangedEventArgs e)
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            dgAppo.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_appointment WHERE " +
                                  "mydentist.tbl_appointment.norm_appo LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_appointment.namapasien_appo LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_appointment.namadokter_appo LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_appointment.tanggal_appo LIKE '%" + txtPencarian.Text + "%' OR " +
                                  "mydentist.tbl_appointment.keterangan_appo LIKE '%" + txtPencarian.Text + "%'", null).DefaultView;

            ((DataGridTextColumn)dgAppo.Columns[0]).Binding = new Binding("id_appo");
            //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
            ((DataGridTextColumn)dgAppo.Columns[2]).Binding = new Binding("tanggal_appo");
            ((DataGridTextColumn)dgAppo.Columns[2]).Binding.StringFormat = "{0:dd MMMM yyyy}";
            ((DataGridTextColumn)dgAppo.Columns[3]).Binding = new Binding("jam_appo");
            ((DataGridTextColumn)dgAppo.Columns[4]).Binding = new Binding("norm_appo");
            ((DataGridTextColumn)dgAppo.Columns[5]).Binding = new Binding("namapasien_appo");
            ((DataGridTextColumn)dgAppo.Columns[6]).Binding = new Binding("namadokter_appo");
            //((DataGridCheckBoxColumn)dgAppo.Columns[7]).Binding = new Binding("status_appo") { Converter = new ItemCodeToBoolConverter() };
            ((DataGridTextColumn)dgAppo.Columns[8]).Binding = new Binding("keterangan_appo");


            
            //@Bahar : Harus ditutup !!!
            koneksi.Dispose();

        }




        private void Status_Click(object sender, RoutedEventArgs e)
        {

            var button = (FrameworkElement)sender;
            var row = (DataGridRow)button.Tag;
            ((CheckBox)sender).IsChecked = !((CheckBox)sender).IsChecked;

            //MessageBox.Show(((CheckBox)sender).IsChecked.ToString());
            MessageBoxResult result = MessageBox.Show("Ubah Status Appointment?", "Konfirmasi", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                //Update Status database
                ((CheckBox)sender).IsChecked = !((CheckBox)sender).IsChecked;

                bool statusbool = (bool)((CheckBox)sender).IsChecked;
                int status;
                if (statusbool)
                    status = 1;
                else
                    status = 0;

                ParameterData[] parameter = new ParameterData[] { new ParameterData("status_appo", status) };

                koneksi.UpdateRow(SettingHelper.database, "tbl_appointment", "id_appo=" + GetIndexKaryawan(row), 0, parameter);
                koneksi.Commit(true);

                //AddItemCallback();

                MessageBox.Show("Data status appointment berhasil diubah", "Informasi", MessageBoxButton.OK, MessageBoxImage.Information);

                koneksi.Dispose();

                //Warna();
                //MessageBox.Show(GetIndexKaryawan(row));
            }
        }

        public class ItemCodeToBoolConverter : IValueConverter
        {
            public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                int itemCode = (int)value;

                return (itemCode == 1);
            }

            public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            {
                bool toBeInvoiced = (bool)value;
                
                return toBeInvoiced ? 1 : 0;
            }
        }

        private void dgAppo_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = (e.Row.GetIndex() + 1).ToString(); 
        }

        public IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }

        private void dgAppo_Loaded(object sender, RoutedEventArgs e)
        {

           // Warna();

        }

        void Warna()
        {
            for (int i = 0; i < dgAppo.Items.Count; i++)
            {
                DataGridRow row = (DataGridRow)dgAppo.ItemContainerGenerator.ContainerFromIndex(i);
                CheckBox checkBox = FindChild<CheckBox>(row, "RowFilterButton");
                if (checkBox != null && checkBox.IsChecked == true)
                {
                    //e.Row.Background = new SolidColorBrush(Colors.Red);
                    row.Background = new SolidColorBrush(Colors.GreenYellow);
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

        private void dgAppo_LayoutUpdated(object sender, EventArgs e)
        {
            Warna();
        }
    }
}
