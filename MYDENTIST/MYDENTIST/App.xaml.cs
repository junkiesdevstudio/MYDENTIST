using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

      EventManager.RegisterClassHandler(typeof(TextBox), UIElement.PreviewMouseLeftButtonDownEvent,
           new MouseButtonEventHandler(SelectivelyHandleMouseButton), true);
        EventManager.RegisterClassHandler(typeof(TextBox), UIElement.GotKeyboardFocusEvent,
          new RoutedEventHandler(SelectAllText), true);

            base.OnStartup(e);

        }

private static void SelectivelyHandleMouseButton(object sender, MouseButtonEventArgs e)
    {
        var textbox = (sender as TextBox);
        if (textbox != null && !textbox.IsKeyboardFocusWithin)
        {
            if( e.OriginalSource.GetType().Name == "TextBoxView" )
            {
                e.Handled = true;
                textbox.Focus();
            }
        }
    }

    private static void SelectAllText(object sender, RoutedEventArgs e)
    {
        var textBox = e.OriginalSource as TextBox;
        if (textBox != null)
            textBox.SelectAll();
    } 


    }
}
