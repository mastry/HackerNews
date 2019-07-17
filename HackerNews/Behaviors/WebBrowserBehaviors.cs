using System;
using System.Windows;
using System.Windows.Controls;

namespace HackerNews.Behaviors
{
    static class WebBrowserBehaviors
    {
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached(
                "BindableSource",
                typeof(string),
                typeof(WebBrowserBehaviors),
                new UIPropertyMetadata(null, BindableSourceChanged));

        static void BindableSourceChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var webBrowser = sender as WebBrowser;
            if (webBrowser == null) return;

            var url = e.NewValue as string;
            if (url != null)
            {
                webBrowser.Source = new Uri(url);
            }
        }

        public static string GetBindableSource(DependencyObject obj)
        {
            return obj.GetValue(BindableSourceProperty) as string;
        }

        public static void SetBindableSource(DependencyObject obj, string value)
        {
            obj.SetValue(BindableSourceProperty, value);
        }
    }
}
