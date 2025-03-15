using Microsoft.Extensions.Logging;
using SuperMarket.DataAccess.Data;
using SuperMarket.Services;
using SuperMarket.ViewModels;
using SuperMarket.Views;
using Microsoft.EntityFrameworkCore;
using SuperMarket.Core.Interfaces;

namespace SuperMarket
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Bold.ttf", "OpenSansBold");
                });

            // Setup logging
#if DEBUG
            builder.Logging.AddDebug();
#endif

            // Register database context
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite("Data Source=supermarket.db"),
                ServiceLifetime.Singleton);

            // Register services
            RegisterServices(builder.Services);

            // Register view models
            RegisterViewModels(builder.Services);

            // Register views
            RegisterViews(builder.Services);

            return builder.Build();
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // Register interfaces with their implementations
            services.AddSingleton<IAuthService, AuthService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<ICartService, CartService>();
            services.AddSingleton<IOrderService, OrderService>();
        }

        private static void RegisterViewModels(IServiceCollection services)
        {
            // Register all view models
            services.AddTransient<SignInViewModel>();
            services.AddTransient<RegistrationViewModel>();
            services.AddTransient<MainViewModel>();
            services.AddTransient<ProductViewModel>();
            services.AddTransient<ProductDetailsViewModel>();
            services.AddTransient<CartViewModel>();
            services.AddTransient<OrderViewModel>();
            services.AddTransient<ProfileViewModel>();
        }

        private static void RegisterViews(IServiceCollection services)
        {
            // Register all views
            services.AddTransient<SignInPage>();
            services.AddTransient<RegistrationPage>();
            services.AddTransient<MainPage>();
            services.AddTransient<ProductPage>();
            services.AddTransient<ProductDetailsPage>();
            services.AddTransient<CartPage>();
            services.AddTransient<OrderPage>();
            services.AddTransient<ProfilePage>();
        }
    }
}