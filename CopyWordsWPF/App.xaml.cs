using System.Windows;
using CopyWordsWPF.ViewModel;
using Microsoft.Extensions.DependencyInjection;

namespace CopyWordsWPF
{
    /// <summary>
    /// Interaction logic for App.
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Services = ConfigureServices();

            this.InitializeComponent();
        }

        /// <summary>
        /// Gets the current <see cref="App"/> instance in use
        /// </summary>
        public static new App Current => (App)Application.Current;

        /// <summary>
        /// Gets the <see cref="IServiceProvider"/> instance to resolve application services.
        /// </summary>
        public IServiceProvider Services { get; }

        /// <summary>
        /// Configures the services for the application.
        /// </summary>
        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            //services.AddSingleton<ISettingsService, SettingsService>();
            //services.AddHttpClient<ISaveSoundFileService, SaveSoundFileService>();
            services.AddHttpClient();

            // Viewmodels
            services.AddTransient<MainViewModel>();
            services.AddTransient<WordViewModel>();

            return services.BuildServiceProvider();
        }
    }
}
