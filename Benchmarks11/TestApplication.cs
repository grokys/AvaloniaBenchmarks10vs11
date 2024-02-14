using Avalonia;
using Avalonia.Headless;
using Avalonia.Styling;
using Avalonia.Themes.Simple;

namespace AvaloniaBenchmarks;

public class TestApplication : Application
{
    public TestApplication()
    {
        Styles.Add(new SimpleTheme());
    }

    public static AppBuilder BuildAvaloniaApp() => AppBuilder.Configure<TestApplication>()
        .UseSkia()
        .UseHeadless(new AvaloniaHeadlessPlatformOptions
        {
            UseHeadlessDrawing = false
        });
}