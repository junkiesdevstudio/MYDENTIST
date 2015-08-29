using MYDENTIST.Class.DatabaseHelper;
using MYDENTIST.Form.AmbilData;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace MYDENTIST.Form.PopUpData
{
    public delegate void AddItemDelegatePopUpDataInkaso();
    public partial class PopUpDataInkaso : Window
    {
        public AddItemDelegatePopUpDataInkaso AddItemCallbackPopInkaso;
        private cds_MYSQLKonektor koneksi;
        private ParameterData[] param;
        private bool isEdit = false;

        InkasoObats rekapInkaso = new InkasoObats();

        public PopUpDataInkaso()
        {
            InitializeComponent();

            datePickDateNow.SelectedDate = DateTime.Today;

            datePickTempo.SelectedDate = DateTime.Today.AddMonths(1);
        }

        private void btnTambahObat_Click(object sender, RoutedEventArgs e)
        {
            FormCariObat formCariPasien = new FormCariObat(true);
            formCariPasien.AddItemCallbackObatBeli = new AddItemDelegateAmbilDataObatHargaBeli(this.AddItemCallbackObatBeli);
            formCariPasien.ShowDialog();
        }

        public void AddItemCallbackObatBeli(int id, string nama, int biayabeli)
        {
            dgObat.ItemsSource = null;
            rekapInkaso.Add(new InkasoObat
                {
                    ID = id,
                    QTY = 1,
                    NamaObat = nama,
                    HargaBeli = biayabeli
                });

            dgObat.ItemsSource = rekapInkaso;
            dgObat.UpdateLayout();
            double totalSub = 0;

            for (int x = 0; x < dgObat.Items.Count; x++)
            {

                DataGridCell cell = DataGridHelper.GetCell(dgObat, x, 4);
                TextBlock tb = cell.Content as TextBlock;

                totalSub += double.Parse(tb.Text);

            }

            txtSubTotal.Text = totalSub.ToString();
        }

        private void btnHapus_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Yakin menghapus item ini?", "Konfirmasi", MessageBoxButton.YesNo);

            if (result == MessageBoxResult.Yes)
            {
                var button = (FrameworkElement)sender;
                var row = (DataGridRow)button.Tag;
                dgObat.Items.RemoveAt(row.GetIndex());
                double totalSub = 0;
                dgObat.UpdateLayout();
                for (int x = 0; x < dgObat.Items.Count; x++)
                {

                    var rows = (DataGridRow)dgObat.ItemContainerGenerator.ContainerFromIndex(x);

                    InkasoObat v = (InkasoObat)dgObat.Items[rows.GetIndex()];
                    totalSub += v.SubTotal;

                }

                txtSubTotal.Text = totalSub.ToString();
               
            }
        }

        public class InkasoObats : List<InkasoObat> { }
        public class InkasoObat : INotifyPropertyChanged
        {
            public int ID { get; set; }
            public int No { get;set; }

            private int qty;
            public int QTY {
                get { return qty; }

                set
                {
                    this.qty = value;
                    OnPropertyChanged("SubTotal");
                }
            }

            public string NamaObat { get; set; }
            public double HargaBeli { get; set; }
            public double SubTotal
            {
                get { return QTY * HargaBeli; }
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

        private void dgObat_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            dgObat.ItemsSource = null;
            dgObat.ItemsSource = rekapInkaso;
            
            dgObat.UpdateLayout();

            double totalSub = 0;
            for (int x = 0; x < dgObat.Items.Count; x++)
            {

                DataGridCell cell = DataGridHelper.GetCell(dgObat, x, 4);
                TextBlock tb = cell.Content as TextBlock;
                MessageBox.Show(tb.Text);
                totalSub += double.Parse(tb.Text);

            }


            txtSubTotal.Text = totalSub.ToString();
        }
    }
}
