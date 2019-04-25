using ScribanExpress.Helpers;
using ScribanExpress.UnitTests.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Xunit;

namespace ScribanExpress.UnitTests
{
    public class MemberFinderTests
    {
        MemberFinder memberFinder = new MemberFinder();
        public MemberFinderTests()
        {
            this.memberFinder = new MemberFinder();
        }

        [Fact]
        public void FindProperty()
        {
            memberFinder.FindMember(typeof(MemberCase), "FirstName", null)
                .ShouldNotBeNull();
        }

        [Fact]
        public void FindProperty_WithIncorrectCasing()
        {
            memberFinder.FindMember(typeof(MemberCase), "firStname", null)
            .ShouldNotBeNull();
        }

        [Fact]
        public void FindMember_WithNoArguments()
        {
            var result = memberFinder.FindMember(typeof(MemberCase), "DoStuff", null);
            result.ShouldBeAssignableTo<MethodInfo>();
            result.ShouldNotBeNull();
        }



        [Fact]
        public void FindStaticMethod_WithNoArguments()
        {
            var result = memberFinder.FindMember(typeof(MemberCase), "GetLucky", null);
            result.ShouldNotBeNull();
            result.ShouldBeAssignableTo<MethodInfo>();
        }

        [Fact]
        public void FindStaticProperty()
        {
            var result = memberFinder.FindMember(typeof(MemberCase), "Lucky", null);
            result.ShouldNotBeNull();
            result.ShouldBeAssignableTo<PropertyInfo>();
        }

        [Fact]
        public void FindGenericMethod()
        {
            Type stringType = typeof(string);
            var argArray = new[] { stringType };

            var result = memberFinder.FindMember(typeof(MemberCase), "PassThrough", argArray);
            result.ShouldNotBeNull();


            var methodInfo = result as MethodInfo;
            var abcconst = Expression.Constant("abc");
            var isvalid = ExpressionHelpers.CallMember(Expression.Constant(new MemberCase()), result,new[] { abcconst });
        [Fact]
        public void FindGenericMultiMethod()
        {
            Type stringType = typeof(string);
            var argArray = new[] { stringType, stringType, stringType };

            var result = memberFinder.FindMember(typeof(MemberCase), "PassIn", argArray);
            result.ShouldNotBeNull();

            var methodInfo = result as MethodInfo;

            var methodparams = methodInfo.GetParameters();
            var gargs = methodInfo.GetGenericArguments();


            var abcconst = Expression.Constant("abc");

             ExpressionHelpers.CallMember(Expression.Constant(new MemberCase()), result, new[] { abcconst, abcconst, abcconst });
        }
    }
}
