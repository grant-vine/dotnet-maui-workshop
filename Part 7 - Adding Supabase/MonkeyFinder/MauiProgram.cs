using Java.Net;
using Microsoft.Extensions.Logging;
using MonkeyFinder.Providers;
using MonkeyFinder.Services;
using MonkeyFinder.View;

namespace MonkeyFinder;

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
#if DEBUG
		builder.Logging.AddDebug();
#endif

        var url = "https://nqdbztbjvfnswzuceysw.supabase.co";
        var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6Im5xZGJ6dGJqdmZuc3d6dWNleXN3Iiwicm9sZSI6ImFub24iLCJpYXQiOjE2NzYwMjUzNjMsImV4cCI6MTk5MTYwMTM2M30.1d4Zwa9OHSLWCDvlBOVevUC4AqTkhZ1IFsnqVzQCdzk";
        
        //builder.Services.AddSingleton<>();
        builder.Services.AddSingleton<Supabase.Client>(
			provider => new Supabase.Client(
				url,
				key,
				new Supabase.SupabaseOptions
				{
					AutoRefreshToken = true,
					AutoConnectRealtime = true,
					PersistSession = true,
					SessionHandler = new CustomSupabaseSessionHandler(
						provider.GetRequiredService<ILogger<CustomSupabaseSessionHandler>>()
					)
				}
			)
		);

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
		builder.Services.AddSingleton<IGeolocation>(Geolocation.Default);
		builder.Services.AddSingleton<IMap>(Map.Default);

		builder.Services.AddSingleton<MonkeyService>();
		builder.Services.AddSingleton<MonkeysViewModel>();
		builder.Services.AddSingleton<MainPage>();

		builder.Services.AddTransient<MonkeyDetailsViewModel>();
		builder.Services.AddTransient<DetailsPage>();

		return builder.Build();
	}
}
