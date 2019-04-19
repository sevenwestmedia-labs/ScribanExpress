using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using ScribanExpress.Benchmarks.Comparison;
using System;
using System.Security.Cryptography;

namespace ScribanExpress.Benchmarks
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var summary = BenchmarkRunner.Run<RenderSimpleTemplate>();
            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
