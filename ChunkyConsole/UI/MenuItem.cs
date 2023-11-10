using System;
using System.Collections.Generic;
using System.Text;

namespace ChunkyConsole.UI
{
    public class MenuItem
    {
        public ConsoleKey Key { get; set; }
        public string Title { get; set; }
        public Commands.ICommand Command { get; set; }

        public Action Action { get; set; }
    }
}
