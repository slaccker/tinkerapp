using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Web.Interfaces;
using Web.NewsSources;
using Web.NewsSources.HackerNews;
using Firebase.Database;
using Web.Settings;

namespace Web.Extensions
{
    public static class AddNewsSourceExtension
    {
        public static IServiceCollection AddNewsSources(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<DefaultsOptions>(configuration.GetSection(DefaultsOptions.Defaults));

            services.AddSingleton<INewsSourceDataSource<FirebaseClient>, HackerNewsDataSource>();

            services.AddSingleton<INewsSource, NewsSourceHackerNews>();

            return services;
        }
    }
}