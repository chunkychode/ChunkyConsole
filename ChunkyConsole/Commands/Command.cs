using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//  @todo expand this command
//  @body need to add more stuff text
//
namespace ChunkyConsole.Commands
{
    public abstract class Command:ICommand
    {
        public string Title { get; set; }
        public abstract void Execute();
    }
}
