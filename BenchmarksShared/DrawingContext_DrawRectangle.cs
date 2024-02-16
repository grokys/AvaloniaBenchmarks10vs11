using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using BenchmarkDotNet.Attributes;
using SkiaSharp;

namespace AvaloniaBenchmarks
{
    [MemoryDiagnoser]
    public class DrawingContext_DrawRectangle
    {
        private readonly RenderTargetBitmap _target = new(new(1000, 1000));
        private readonly SKCanvas _skCanvas = null!;
        private readonly SKPaint _paint = new() { Color = new(0, 0, 0) };
        private readonly DrawingContext _drawingContext = null!;

        public DrawingContext_DrawRectangle()
        {
#if AVALONIA10
            _drawingContext = new DrawingContext(_target.CreateDrawingContext(null));
#else
            _drawingContext = _target.CreateDrawingContext(false);
#endif
            _drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, 1000, 1000));
            _skCanvas = new SKCanvas(new SKBitmap(1000, 1000));
        }

        [Benchmark(Baseline = true)]
        public void DrawRectangle_Skia()
        {
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    _skCanvas.DrawRect(x * 100, y * 100, 100, 100, _paint);
                }
            }
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
    }
}
