using Avalonia;
using Avalonia.Controls.Shapes;
using Avalonia.Headless;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using BenchmarkDotNet.Attributes;

namespace BenchmarksShared
{
    [MemoryDiagnoser]
    public class Shapes_Rendering
    {
        private RenderTargetBitmap _rtb = null!;
        private readonly Rectangle _rectangle = new()
        {
            Width = 100,
            Height = 100,
            Fill = Brushes.Red,
        };

        [GlobalSetup]
        public void Setup()
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
            _rtb = new RenderTargetBitmap(new(100, 100));
            _rectangle.Measure(Size.Infinity);
            _rectangle.Arrange(new(_rectangle.DesiredSize));
        }

        [Benchmark]
        public void DrawRectangle()
        {
            for (var i = 0; i < 100; i++)
            {
                _rtb.Render(_rectangle);
                _rtb.Save(@"d:\temp\output.png");
            }
        }
    }
}
