using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;

namespace MYDENTIST
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void ApplicationStartup(object sender, StartupEventArgs e)
        {

        }

        protected override void OnStartup(StartupEventArgs e)
        {

            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("id-ID"); ;

            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("id-ID"); ;



            FrameworkElement.LanguageProperty.OverrideMetadata(

              typeof(FrameworkElement),

              new FrameworkPropertyMetadata(

                    XmlLanguage.GetLanguage(CultureInfo.CurrentCulture.IetfLanguageTag)));



            base.OnStartup(e);

        }
    }
}
