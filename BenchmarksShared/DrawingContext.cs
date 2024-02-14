using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting;
using Avalonia.Platform;
using BenchmarkDotNet.Attributes;

namespace AvaloniaBenchmarks
{
    [MemoryDiagnoser]
    public class DrawingContext_Skia
    {
        private RenderTargetBitmap _target = new(new(1000, 1000));
        private DrawingContext _drawingContext = null!;
        private Typeface typeface = new Typeface("Arial");

        public DrawingContext_Skia()
        {
#if AVALONIA10
            _drawingContext = new DrawingContext(_target.CreateDrawingContext(null));
#else
            _drawingContext = _target.CreateDrawingContext(false);
#endif
            _drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, 1000, 1000));
        }

        [Benchmark]
        public void DrawRectangle()
        {
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    _drawingContext.DrawRectangle(Brushes.Blue, null, new Rect(x * 100, y * 100, 100, 100));
                }
            }
        }

        [Benchmark]
        public void DrawText_Short()
        {
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
#if AVALONIA10
                    var text = new FormattedText
                    {
                        Text = $"{x},{y}",
                        Typeface = typeface,
                        FontSize = 48,
                        TextAlignment = TextAlignment.Left,
                    };

                    _drawingContext.DrawText(Brushes.Black, new Point(x * 100, y * 100), text);
#else
                    var text = new TextLayout(
                        $"{x},{y}",
                        typeface,
                        48,
                        Brushes.Black);

                    text.Draw(_drawingContext, new Point(x * 100, y * 100));
#endif
                }
            }
        }
    }
}
