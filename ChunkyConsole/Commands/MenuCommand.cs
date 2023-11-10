using System;

namespace ChunkyConsole.Commands
{
    public abstract class MenuCommand : Command, IHasMenu
    {
        public UI.Menu Menu { get; set; }
        public ExecuteOrder Order { get; set; }
        public MenuOrder MenuOrder { get; set; }

        public MenuCommand()
        {
            this.Menu = new UI.Menu();
        }

        public abstract override void Execute();

        /// <summary>
        /// Add menu item
        /// </summary>
        /// <param name="title"></param>
        /// <param name="key"></param>
        /// <param name="action"></param>
        public void Add(string title, ConsoleKey key, Action action)
        {
            Add(title, key, null, action);
        }
        /// <summary>
        /// Add menu item
        /// </summary>
        /// <param name="title"></param>
        /// <param name="key"></param>
        /// <param name="command"></param>
        public void Add(string title, ConsoleKey key, ICommand command)
        {
            Add(title, key, command, null);
        }
        /// <summary>
        /// Add menu item
        /// </summary>
        /// <param name="title"></param>
        /// <param name="key"></param>
        /// <param name="command"></param>
        /// <param name="action"></param>
        protected void Add(string title, ConsoleKey key, ICommand command, Action action)
        {
            this.Menu.MenuItems.Add(new UI.MenuItem() { Title = title, Key = key, Command = command, Action = action });
        }
    }
}
