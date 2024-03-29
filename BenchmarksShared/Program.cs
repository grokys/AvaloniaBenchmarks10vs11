using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia.Headless;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;

namespace Avalonia.Benchmarks
{
    class Program
    {
        static void Main(string[] args)
        {
            // Use reflection for a more maintainable way of creating the benchmark switcher,
            // Benchmarks are listed in namespace order first (e.g. BenchmarkDotNet.Samples.CPU,
            // BenchmarkDotNet.Samples.IL, etc) then by name, so the output is easy to understand
            var benchmarks = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                             .Any(m => m.GetCustomAttributes(typeof(BenchmarkAttribute), false).Any()))
                .OrderBy(t => t.Namespace)
                .ThenBy(t => t.Name)
                .ToArray();
            var benchmarkSwitcher = new BenchmarkSwitcher(benchmarks);
            IConfig config = null;

            if (args.Contains("--debug"))
            {
                config = new DebugInProcessConfig();
                var a = new List<string>(args);
                a.Remove("--debug");
                args = a.ToArray();
            }

            AppBuilder.Configure<Application>()
#if AVALONIA10
                .UseHeadless(headlessDrawing: false)
                .UseSkia()
#else
                .UseHeadless(new() { UseHeadlessDrawing = false })
                .UseSkia()
#endif
                .SetupWithoutStarting();

            benchmarkSwitcher.Run(args, config);
        }
    }
}
