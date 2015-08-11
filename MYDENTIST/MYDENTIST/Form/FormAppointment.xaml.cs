using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using System;
using System.Collections.Generic;
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

            for (int i = 1950; i <= DateTime.UtcNow.Year; i++)
            {
                ComboBoxItem item = new ComboBoxItem();
                item.Content = i;
                cmbTahun.Items.Add(item);
                cmbTahun.SelectedValue = item;
            }


            DateTime now = Convert.ToDateTime("01/01/0001");
            for (int i = 0; i < 12; i++)
            {
                cmbBulan.Items.Add(now.ToString("MMMM"));
                now = now.AddMonths(1);

                cmbBulan.SelectedValue = DateTime.Now.ToString("MMMM");
            }

            ShowDataTabel();
            mulaiFilter = true;
        }

        void ShowDataTabel()
        {
            try
            {
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                dgAppo.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_appointment WHERE MONTH(mydentist.tbl_appointment.tanggal_appo) = " + (cmbBulan.SelectedIndex + 1) + " AND YEAR(mydentist.tbl_appointment.tanggal_appo) =" + ((ComboBoxItem)cmbTahun.SelectedItem).Content.ToString(), null).DefaultView;

                ((DataGridTextColumn)dgAppo.Columns[0]).Binding = new Binding("id_appo");
                //((DataGridTextColumn)dgUsers.Columns[1]).Binding = new Binding("id_pasien");
                ((DataGridTextColumn)dgAppo.Columns[2]).Binding = new Binding("tanggal_appo");
                ((DataGridTextColumn)dgAppo.Columns[2]).Binding.StringFormat = "{0:dd MMMM yyyy}";
                ((DataGridTextColumn)dgAppo.Columns[3]).Binding = new Binding("jam_appo");
                ((DataGridTextColumn)dgAppo.Columns[4]).Binding = new Binding("norm_appo");
                ((DataGridTextColumn)dgAppo.Columns[5]).Binding = new Binding("namapasien_appo");
                ((DataGridTextColumn)dgAppo.Columns[6]).Binding = new Binding("namadokter_appo");
                ((DataGridTextColumn)dgAppo.Columns[7]).Binding = new Binding("status_appo");
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
                ((DataGridTextColumn)dgAppo.Columns[7]).Binding = new Binding("status_appo");
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
                ShowDataTabelFilter(((ComboBoxItem)cmbTahun.SelectedItem).Content.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
            }
        }

        private void cmbBulan_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (mulaiFilter)
            {
                ShowDataTabelFilter(((ComboBoxItem)cmbTahun.SelectedItem).Content.ToString(), (cmbBulan.SelectedIndex + 1).ToString());
            }
        }
    }
}
