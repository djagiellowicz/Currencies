using Microsoft.Extensions.DependencyInjection;
using WalutyBusinessLogic.DatabaseLoading;
using WalutyBusinessLogic.DatabaseLoading.Updater;
using WalutyBusinessLogic.LoadingFromFile;
using WalutyBusinessLogic.LoadingFromFile.DatabaseLoading;
using WalutyBusinessLogic.Services;

namespace WalutyMVCWebApp.Configuration
{
    public static class ServicesConfiguration
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddSingleton<ILoader, Loader>();
            services.AddTransient<ICurrencyFilesDownloader, CurrencyFilesDownloader>();
            services.AddTransient<ICurrencyFilesUnzipper, CurrencyFilesUnzipper>();
            services.AddTransient<ICurrencyRepository, CurrencyRepository>();
            services.AddTransient<IUserCurrencyRepository, UserCurrencyRepository>();
            services.AddTransient<IExtremesServices, ExtremesServices>();
            services.AddTransient<IDateRange, DateRange>();
            services.AddTransient<IDateChecker, DateChecker>();
            services.AddTransient<ICurrencyConversionService, CurrencyConversionService>();
            services.AddTransient<ICurrencyNameChecker, CurrencyNameChecker>();
            services.AddTransient<ICurrenciesComparator, CurrenciesComparator>();
            services.AddTransient<ICurrenciesSelectList, CurrenciesSelectList>();
            services.AddTransient<IUserServices, UserServices>();
            services.AddTransient<IChartService, ChartService>();
            services.AddTransient<IFavoritesService, FavoritesService>();
            services.AddSingleton<ICurrencyFilesUpdater, CurrencyFilesUpdater>();
        }
    }
}
