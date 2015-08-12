using MYDENTIST.Class;
using MYDENTIST.Class.DatabaseHelper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using WPFTaskbarNotifier;

namespace MYDENTIST.Form
{
    public delegate void AddItemDelegateReminder();

    public class NotifyObject
    {
        public AddItemDelegateReminder AddItemCallback;

        public NotifyObject(string message, string title)
        {
            this.message = message;
            this.title = title;

            //AddItemCallback();
        }

        private string title;
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        private string message;
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
    }
    public partial class TaskbarNotifierAppointment : TaskbarNotifier
    {
        private cds_MYSQLKonektor koneksi;
        private ParameterData param;

        public TaskbarNotifierAppointment()
        {
            InitializeComponent();

            
        }

        private ObservableCollection<NotifyObject> notifyContent;
        /// <summary>
        /// A collection of NotifyObjects that the main window can add to.
        /// </summary>
        public ObservableCollection<NotifyObject> NotifyContent
        {
            get
            {
                if (this.notifyContent == null)
                {
                    // Not yet created.
                    // Create it.
                    this.NotifyContent = new ObservableCollection<NotifyObject>();
                }

                return this.notifyContent;
            }
            set
            {
                this.notifyContent = value;
            }


        }

        private void Item_Click(object sender, EventArgs e)
        {
            Hyperlink hyperlink = sender as Hyperlink;

            if(hyperlink == null)
                return;

            NotifyObject notifyObject = hyperlink.Tag as NotifyObject;
            if(notifyObject != null)
            {
                MessageBox.Show("\"" + notifyObject.Message + "\"" + " clicked!");
            }
        }

        private void HideButton_Click(object sender, EventArgs e)
        {
            //this.ForceHidden();
            this.Close();
        }

        //public virtual void Show(){
            //ShowDataTabel();
        //}

        public void ShowDataTabel()
        {


            try
            {
                koneksi = new cds_MYSQLKonektor(new cds_KoneksiString(SettingHelper.host, SettingHelper.user, SettingHelper.pass, SettingHelper.port), true, System.Data.IsolationLevel.Serializable);
                dgAppo.ItemsSource = koneksi.GetDataTable("SELECT * FROM mydentist.tbl_appointment WHERE mydentist.tbl_appointment.tanggal_appo <= NOW()", null).DefaultView;

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

                //MessageBox.Show();
                
            }
            catch (Exception e)
            {
                dgAppo.ItemsSource = null;
                //dgAppo.Items.Refresh();
                koneksi.Dispose();
            }
        }
    }
}
