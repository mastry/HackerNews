using System.Windows;
using System.Windows.Input;

namespace HackerNews.MVVM
{
    static class ElementLoadCommand
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "LoadCommand",
                typeof(ICommand),
                typeof(ElementLoadCommand),
                new UIPropertyMetadata(null, LoadCommandChanged));

        static void LoadCommandChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var element = sender as FrameworkElement;
            if (element == null) return;

            var command = (ICommand)e.NewValue;
            if (command != null)
                element.Loaded += OnLoaded;
            else
                element.Loaded -= OnLoaded;
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var obj = sender as DependencyObject;
            if (obj == null) return;

            var command = obj.GetValue(CommandProperty) as ICommand;
            if (command == null) return;

            command.Execute(null);
        }

        public static ICommand GetLoadCommand(DependencyObject obj)
        {
            return (ICommand)obj.GetValue(CommandProperty);
        }

        public static void SetLoadCommand(DependencyObject obj, ICommand command)
        {
            obj.SetValue(CommandProperty, command);
        }
    }
}
