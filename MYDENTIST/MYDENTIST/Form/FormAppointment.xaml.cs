using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using MYDENTIST.Form.PopUpData;
using System;
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

                //@Bahar : Harus ditutup !!!
                koneksi.Dispose();

            }
            catch (Exception e)
            {
                dgAppo.ItemsSource = null; 
                //dgAppo.Items.Refresh();
                koneksi.Dispose();
            }
        }

        private void cmbTahun_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mulaiFilter)
            {
                //ShowDataTabelFilter(((ComboBoxItem)cmbTahun.SelectedItem).Content.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
                ShowDataTabelFilter(cmbTahun.SelectedItem.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
            }
        }

        private void cmbBulan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mulaiFilter)
            {
                //ShowDataTabelFilter(((ComboBoxItem)cmbTahun.SelectedItem).Content.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
                ShowDataTabelFilter(cmbTahun.SelectedItem.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
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
    }
}
