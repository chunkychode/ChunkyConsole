using System;
using System.Collections.Generic;
using System.Text;

namespace ChunkyConsole
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple =false,Inherited =true)]
    public class IncludePropertyAttribute : Attribute
    {
        public IncludePropertyAttribute() { }
        public IncludePropertyAttribute(bool isPassword) { this.IsPassword = isPassword; }

        public bool IsPassword { get; set; }
    }

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IncludeMethodAttribute : Attribute
    {
        public IncludeMethodAttribute() { }
        public IncludeMethodAttribute(bool isPassword) { this.IsPassword = isPassword; }

        public bool IsPassword { get; set; }
    }
}
