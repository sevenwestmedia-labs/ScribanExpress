using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.Benchmarks.Theorys
{
    public class DynamicTypeBenchmarks
    {
        // https://stackoverflow.com/questions/14859854/dynamic-binding-vs-type-inference


        [Benchmark]
        public bool NativeWrite()
        {
            var x = "aaa";
            var y = x.ToUpper();
            var z = y.ToLower();
            var a = new List<string>() { "a", "b","c","aaa"};
            var b = a.Contains(z);
            return b;
        }


        [Benchmark]
        public bool DynamicWrite()
        {
            dynamic x = "aaa";
            dynamic y = x.ToUpper();
            dynamic z = y.ToLower();
            dynamic a = new List<string>() { "a", "b", "c", "aaa" };
            dynamic b = a.Contains(z);
            return b;
        }
    }
}
