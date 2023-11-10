using System;
using System.Collections.Generic;
using System.Text;

namespace ChunkyConsole
{
    public class IncludeMethodAttribute : Attribute
    {
        public IncludeMethodAttribute() { }
        public IncludeMethodAttribute(bool isPassword) { this.IsPassword = isPassword; }

        public bool IsPassword { get; set; }
    }
}
