using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Markup;
using Plugin.LocalNotification;

namespace Capstone
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseLocalNotification()
                .UseMauiCommunityToolkit()
                .UseMauiCommunityToolkitCore()
                .UseMauiCommunityToolkitMarkup()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            
            #if DEBUG
		    builder.Logging.AddDebug();
            #endif
                 
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<ViewProductPage>();
            builder.Services.AddTransient<ViewSprintPage>();
            builder.Services.AddTransient<ViewRequirementPage>();
            builder.Services.AddTransient<AddProductPage>();
            builder.Services.AddTransient<AddSprintPage>();
            builder.Services.AddTransient<AddRequirementPage>();
            builder.Services.AddTransient<EditProductPage>();
            builder.Services.AddTransient<EditSprintPage>();
            builder.Services.AddTransient<EditRequirementPage>();                
            builder.Services.AddTransient<LoginPage>();                

            return builder.Build();
        }
    }
}