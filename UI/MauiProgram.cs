using UI.Services;

namespace UI
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
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            var appSettings = new AppSettings();
            LocalizationService.Init(appSettings);

            builder.Services.AddSingleton(appSettings);
            builder.Services.AddSingleton<DialogService>();
            builder.Services.AddSingleton<MatrixActionsService>();

            return builder.Build();
        }
    }
}