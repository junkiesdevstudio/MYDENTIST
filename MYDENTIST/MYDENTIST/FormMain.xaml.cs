using System;
using System.Collections.Generic;
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
using MYDENTIST.Form;
using System.Windows.Threading;
using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using System.Data;
using System.Media;
using MySql.Data.MySqlClient;

namespace MYDENTIST
{
    /// <summary>
    /// Interaction logic for FormMain.xaml
    /// </summary>
    /// assssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssssus
    public partial class FormMain : Window
    {
        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;

        private TaskbarNotifierAppointment taskbarNotifier;
        public TaskbarNotifierAppointment TaskbarNotifier
        {
            get { return this.taskbarNotifier; }
        }

        private bool reallyCloseWindow = false;

        public FormMain()
        {

            this.taskbarNotifier = new TaskbarNotifierAppointment();
            InitializeComponent();
       
            //koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
            //koneksi.SetKoneksiString(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port));

            //MySqlConnection mysqlConnection = new MySqlConnection(koneksi.koneksiString);

            //if(!koneksi.OpenKoneksi(mysqlConnection,10));
            //MessageBox.Show("A");

            UIPanel.Children.Clear();
            FormTransaki inputGrid = new FormTransaki();
            UIPanel.Children.Add(inputGrid);


            CheckReminder();
            
        }

        void CheckReminder()
        {
            koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);

            DataTable CmbxDataTanggal = new DataTable();
            CmbxDataTanggal = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_appointment ORDER BY id_appo", null);


            if (CmbxDataTanggal.Rows.Count != 0)
            {
                for(int x=0;x<CmbxDataTanggal.Rows.Count ;x++){
                    if (DateTime.Parse(CmbxDataTanggal.Rows[x]["tanggal_appo"].ToString()) == DateTime.Now.Date && CmbxDataTanggal.Rows[x]["status_appo"].ToString() != "1")
                    {
                        if (!this.taskbarNotifier.Activate())
                        {
                            this.taskbarNotifier.StayOpenMilliseconds = 5000;
                            this.taskbarNotifier.Show();
                            
                            this.taskbarNotifier.Notify();

                            using (var soundPlayer = new SoundPlayer(@"c:\Windows\Media\chimes.wav"))
                            {
                                soundPlayer.Play(); // can also use soundPlayer.PlaySync()
                            }

                            this.taskbarNotifier.ShowDataTabel();
                        }

                    }
                }
            }

            koneksi.Dispose();

        }


        private void btnKaryawan_Click(object sender, RoutedEventArgs e)
        {
            
            UIPanel.Children.Clear();
            FormKaryawan inputGrid = new FormKaryawan();
            UIPanel.Children.Add(inputGrid);
        }

        private void btnObat_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormObat inputGrid = new FormObat();
            UIPanel.Children.Add(inputGrid);
        }

        private void btnPasien_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormPasien inputGrid = new FormPasien();
            UIPanel.Children.Add(inputGrid);
        }

        private void btnTerapi_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormTerapi inputGrid = new FormTerapi();
            UIPanel.Children.Add(inputGrid);
        }
        private void btnRekapTerapi_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormRekapTerapi inputGrid = new FormRekapTerapi();
            UIPanel.Children.Add(inputGrid);
        }
        private void btnAppointment_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormAppointment inputGrid = new FormAppointment();
            UIPanel.Children.Add(inputGrid);
        }

        private void btnPresensi_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormPresensi inputGrid = new FormPresensi();
            UIPanel.Children.Add(inputGrid);
        }


        protected override void OnClosing(System.ComponentModel.CancelEventArgs args)
        {
            // In WPF it is a challenge to hide the window's close box in the title bar.
            // When the user clicks this, we don't want to exit the app, but rather just
            // put it back into hiding.  Unfortunately, this is a challenge too.
            // The follow code works around the issue.

            if (!this.reallyCloseWindow)
            {
                // Don't close, just Hide.
                args.Cancel = true;
                // Trying to hide
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, (DispatcherOperationCallback)delegate(object o)
                {
                    this.reallyCloseWindow = true;
                    this.Close();
                    return null;
                }, null);
            }
            else
            {
                // Actually closing window.

                this.NotifyIcon.Visibility = Visibility.Collapsed;

                // Close the taskbar notifier too.
                if (this.taskbarNotifier != null)
                    this.taskbarNotifier.Close();
            }
        }

        private void NotifyIcon_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                // Open the TaskbarNotifier
                this.taskbarNotifier.Notify();
            }
        }

        private void NotifyIconOpen_Click(object sender, RoutedEventArgs e)
        {
            // Open the TaskbarNotifier
            this.taskbarNotifier.Notify();
        }

        private void NotifyIconConfigure_Click(object sender, RoutedEventArgs e)
        {
            // Show this window
            this.Show();
            this.Activate();          
        }

        private void NotifyIconExit_Click(object sender, RoutedEventArgs e)
        {
            // Close this window.
            this.reallyCloseWindow = true;
            this.Close();
        }

        private void btnTransaksi_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormTransaki inputGrid = new FormTransaki();
            UIPanel.Children.Add(inputGrid);
        }


        private void btnRekapObat_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormRekapObat inputGrid = new FormRekapObat();
            UIPanel.Children.Add(inputGrid);
        }

        private void btnRekapInkaso_Click(object sender, RoutedEventArgs e)
        {
            UIPanel.Children.Clear();
            FormRekapInkaso inputGrid = new FormRekapInkaso();
            UIPanel.Children.Add(inputGrid);
        }
    }
}
