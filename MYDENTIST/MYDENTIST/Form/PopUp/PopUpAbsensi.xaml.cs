using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MYDENTIST.Form.PopUp
{
    /// <summary>
    /// Interaction logic for PopUpAbsensi.xaml
    /// </summary>
    public partial class PopUpAbsensi : Window
    {
        public PopUpAbsensi()
        {
            InitializeComponent();
            DateTime now = DateTime.Now;
            //CultureInfo ci = Thread.CurrentThread.CurrentCulture;
            txtTanggal.Text = now.ToString("dddd, dd MMMM yyyy"); 
 

           // Console.WriteLine("Long date pattern: \"{0}\"", ci.Name);
        }
    }
}
