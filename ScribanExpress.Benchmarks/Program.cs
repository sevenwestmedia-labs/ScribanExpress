﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
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
            // To debug (ONLY USE FOR DEBUGGING):
            // BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args, new DebugInProcessConfig());

            // To Run Specifc Benchmark
            //var summary = BenchmarkRunner.Run<RenderSimpleTemplate>();

            BenchmarkSwitcher.FromAssembly(typeof(Program).Assembly).Run(args);
        }
    }
}
