using System.Globalization;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting;
using BenchmarkDotNet.Attributes;
using SkiaSharp;

namespace AvaloniaBenchmarks
{
    public class Text_Rendering
    {
        private readonly RenderTargetBitmap _target = new(new(100, 100));
        private readonly SKCanvas _skCanvas = null!;
        private readonly SKPaint _paint = new() { Color = new(0, 0, 0) };
        private readonly DrawingContext _drawingContext = null!;
        private readonly Typeface typeface = new("Arial");
        private readonly TextLayout _shortText;
        private readonly FormattedText _shortFormattedText;

        public Text_Rendering()
        {
#if AVALONIA10
            _drawingContext = new DrawingContext(_target.CreateDrawingContext(null));
#else
            _drawingContext = _target.CreateDrawingContext(false);
#endif
            _drawingContext.DrawRectangle(Brushes.White, null, new Rect(0, 0, 1000, 1000));
            _shortText = new TextLayout("text", typeface, 48, Brushes.Black);
#if AVALONIA10
            _shortFormattedText = new FormattedText { Text = "text", Typeface = typeface, FontSize = 48 };
#else
            _shortFormattedText = new FormattedText("text", CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, 48, Brushes.Black);
#endif
            _skCanvas = new SKCanvas(new SKBitmap(1000, 1000));
        }

        [Benchmark(Baseline = true)]
        public void Render_Short_Text_Skia()
        {
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
                    _skCanvas.DrawText($"{x},{y}", x * 10, y * 10, _paint);
                }
            }
        }

        [Benchmark]
        public void Render_Short_TextLayout()
        {
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
#if AVALONIA10
                    using var p = _drawingContext.PushPostTransform(Matrix.CreateTranslation(x * 10, y * 10));
                    _shortText.Draw(_drawingContext);    
#else
                    _shortText.Draw(_drawingContext, new Point(x * 10, y * 10));
#endif
                }
            }
        }

        [Benchmark]
        public void Render_Short_FormattedText()
        {
            for (var y = 0; y < 10; y++)
            {
                for (var x = 0; x < 10; x++)
                {
#if AVALONIA10
                    _drawingContext.DrawText(Brushes.Black, new(x * 10, y * 10), _shortFormattedText);
#else
                    _drawingContext.DrawText(_shortFormattedText, new(x * 10, y * 10));
#endif
                }
            }
        }
    }
}
