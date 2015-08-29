using MYDENTIST.Form.PopUpData;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MYDENTIST.Form
{
    /// <summary>
    /// Interaction logic for FormRekapInkaso.xaml
    /// </summary>
    public partial class FormRekapInkaso : UserControl
    {
        public FormRekapInkaso()
        {
            InitializeComponent();
        }

        private void btn_tambah_Click(object sender, RoutedEventArgs e)
        {
            PopUpDataInkaso popInkaso = new PopUpDataInkaso();
            popInkaso.AddItemCallbackPopInkaso = new AddItemDelegatePopUpDataInkaso(this.AddItemCallbackPopUpInkaso);
            popInkaso.ShowDialog();
        }

        private void AddItemCallbackPopUpInkaso()
        {
            //ShowDataTabel();
        }
    }
}
