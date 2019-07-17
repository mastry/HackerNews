using System;
using System.Windows;
using SimpleInjector;

namespace HackerNews
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            var container = ConfigureContainer();

            RunApplication(container);
        }

        static Container ConfigureContainer()
        {
            var container = new Container();

            container.Register<IHackerNewsClient, HackerNewsClient>(SimpleInjector.Lifestyle.Singleton);
            container.Register<MainWindow>();
            container.Register<MainViewModel>();

            container.Verify();

            return container;
        }

        static void RunApplication(Container container)
        {
            try
            {
                var app = new App();
                var mainWindow = container.GetInstance<MainWindow>();
                app.Run(mainWindow);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
