using System;
using System.Collections.Generic;
using System.Text;

namespace ChunkyConsole.Commands
{
    public enum ExecuteOrder { none, BeforeMenu, AfterMenu }
    public enum MenuOrder { none, BeforeExecute, AfterExecute, BeforePrompt, AfterPrompt }

    public interface IHasMenu
    {
        UI.Menu Menu { get; }
        ExecuteOrder Order { get; }
        MenuOrder MenuOrder { get; }
    }
}
