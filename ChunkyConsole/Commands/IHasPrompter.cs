using System;
using System.Collections.Generic;
using System.Text;

namespace ChunkyConsole.Commands
{
    public interface IHasPrompter
    {
        Prompts.Prompter Prompter { get; }
        bool KeepLastAnswers { get; }
    }
}
