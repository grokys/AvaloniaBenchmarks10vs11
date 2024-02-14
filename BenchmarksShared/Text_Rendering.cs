using Avalonia;
using Avalonia.Controls;
using Avalonia.Headless;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using BenchmarkDotNet.Attributes;

namespace BenchmarksShared
{
    [MemoryDiagnoser]
    public class Text_Rendering
    {
        private static bool _initialized = false;
        private RenderTargetBitmap _rtb = null!;
        private readonly TextBlock _shortTextBlock = new()
        {
            Text = "123",
            FontSize = 24,
            Foreground = Brushes.Black,
        };

        private readonly TextBlock _longTextBlock = new()
        {
            Text = string.Concat(Enumerable.Range(0, 100).Select(x => $"String{x}")),
            FontSize = 24,
            Foreground = Brushes.Black,
            TextWrapping = TextWrapping.Wrap,
        };

        [GlobalSetup]
        public void SetupSkia()
        {
            AppBuilder.Configure<Application>()
#if AVALONIA10
                .UseHeadless(headlessDrawing: false)
                .UseSkia()
#else
                .UseHeadless(new() { UseHeadlessDrawing = false })
                .UseSkia()
#endif
                .SetupWithoutStarting();
            _rtb = new RenderTargetBitmap(new(1000, 1000));
            _shortTextBlock.Measure(new Size(1000, 1000));
            _shortTextBlock.Arrange(new(_shortTextBlock.DesiredSize));
            _longTextBlock.Measure(new Size(1000, 1000));
            _longTextBlock.Arrange(new(_shortTextBlock.DesiredSize));
        }

        //[Benchmark]
        //public void DrawShortText()
        //{
        //    for (var i = 0; i < 100; i++)
        //        _rtb.Render(_shortTextBlock);
        //}

        [Benchmark]
        public void DrawLongText()
        {
            for (var i = 0; i < 100; i++)
                _rtb.Render(_longTextBlock);
        }
    }
}
