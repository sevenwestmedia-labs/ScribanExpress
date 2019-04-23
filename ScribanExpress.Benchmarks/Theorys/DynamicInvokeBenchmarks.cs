using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace ScribanExpress.Benchmarks.Theorys
{
    public class DynamicInvokeBenchMarks
    {
        readonly Func<string> Native;
        readonly Func<string> TypedExpressionFunc;
        Delegate Dynamic;
        int iterations = 1000;
        public DynamicInvokeBenchMarks()
        {
            Native = () => "done";


            var doneExp = Expression.Constant("done");
            var lambda = Expression.Lambda<Func<string>>(doneExp);
            TypedExpressionFunc = lambda.Compile();

            var unknownlambda = Expression.Lambda(doneExp);
            Dynamic  =  unknownlambda.Compile();
           
        }

        [Benchmark]
        public void NativeDelegate()
        {
            for (int i = 0; i < iterations; i++)
            {
                Native();
            }
        }

        [Benchmark]
        public void CompiledTypedDelegate()
        {
            for (int i = 0; i < iterations; i++)
            {
                TypedExpressionFunc();
             }
        }


        [Benchmark]
        public void DynamicInvoke()
        {
            for (int i = 0; i < iterations; i++)
            {
                Dynamic.DynamicInvoke();
            }
        }

    }
}
