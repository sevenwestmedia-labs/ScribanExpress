using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Benchmarks.Theorys
{
   public class StringConcatBenchmarks
    {
        int iterations = 1000;

        [Benchmark]
        public void Interpolation()
        {
            for (int i = 0; i < iterations; i++)
            {
                var result = $"{i}blah";
            }
        }

        [Benchmark]
        public void Concat()
        {
            for (int i = 0; i < iterations; i++)
            {
                var result = i + "blah";
            }
        }

        [Benchmark]
        public void StringBuilder()
        {
            for (int i = 0; i < iterations; i++)
            {
                var sb = new StringBuilder();
                sb.Append(i);
                sb.Append("blah");

                var result = sb.ToString();
            }
        }
    }
}
