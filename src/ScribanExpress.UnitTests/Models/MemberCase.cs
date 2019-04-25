using System;
using System.Collections.Generic;
using System.Text;

namespace ScribanExpress.UnitTests.Models
{
    //examples to help with MemberFinderTests
    public class MemberCase
    {
        public string FirstName { get; set; }

        public void DoStuff() { }

        public static string GetLucky() => "lucky";

        public static string Lucky
        {
            get { return "Lucky"; }
        }

        public T PassThrough<T>(T item)
        {
            return item;
        }

        public T PassIn<T, Y>(T item, string astring, Y awhy)
        {
            return item;
        }
    }
}
