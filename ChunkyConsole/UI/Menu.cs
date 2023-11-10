using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChunkyConsole.UI
{
    public class Menu
    {
        public List<MenuItem> MenuItems { get; set; }
        private int Longest { get { return MenuItems.Count > 0 ? MenuItems.Max(m => m.Title.Length) : 0; } }
        public bool ShowBorder { get; set; }
        public bool CenterHor { get; set; }
        public bool ExitOnBadSelection { get; set; }
        public bool ShowOnce { get; set; }
        public char BorderCharecter { get; set; }
        public ConsoleColor ForeColor { get; set; }
        public ConsoleColor BackColor { get; set; }


        public Menu()
        {
            this.MenuItems = new List<MenuItem>();
            this.BackColor = ConsoleColor.Black;
            this.ForeColor = ConsoleColor.White;
        }
        public static void Print(IEnumerable<MenuItem> items)
        {
            Menu m = new Menu();
            m.MenuItems.AddRange(items);
            m.Print();
        }
        public void Print()
        {
            int maxWidth = Longest;
            string Border = (from s in Enumerable.Range(0, maxWidth + 20) select BorderCharecter.ToString()).Aggregate((a, b) => a += b);
            int iLeft = CenterHor ? 0 : ((Console.WindowWidth / 2) - Border.Length / 2);
            Utils.ConsoleHelper.SetColor(ForeColor, BackColor);
            Utils.ConsoleHelper.Clear();

            do
            {
                Utils.ConsoleHelper.Clear();
                print(maxWidth, iLeft, Border);

                var k = Console.ReadKey(true);
                var mi = MenuItems.FirstOrDefault(p => p.Key == k.Key);
                if (mi == null && k.Key == ConsoleKey.Escape)
                    break;


                HandleKey(mi);

                if (ExitOnBadSelection)
                    break;

            } while (ShowOnce ? false : true);



        }
        private void print(int maxWidth, int iLeft, string Border)
        {

            if (iLeft > 0)
                Console.CursorTop = Math.Max((Console.WindowHeight / 2) - MenuItems.Count, 0);

            if (ShowBorder)
            {
                Console.CursorLeft = iLeft;
                Console.WriteLine(Border);
            }

            MenuItems.ForEach(m =>
            {
                Console.CursorLeft = iLeft;
                Console.WriteLine(string.Format("{3}{0,5}{1," + maxWidth + "}{0,5}[{2}]{0,5}{3}", ' ', m.Title.PadRight(maxWidth, ' '), Char.ConvertFromUtf32((int)m.Key), ShowBorder ? BorderCharecter : '\0'));
            });

            if (ShowBorder)
            {
                Console.CursorLeft = iLeft;
                Console.WriteLine(Border);
            }
            Console.WriteLine();
            Console.CursorLeft = iLeft;
            Console.Write(string.Format("{0}:", "Enter selection:".PadRight(maxWidth, ' ')));
        }
        public static List<Func<Commands.ICommand, bool>> ExecuteOrder = new List<Func<Commands.ICommand, bool>>() { EXE_exe, EXE_mnu, EXE_Prompt };

        static bool EXE_exe(Commands.ICommand cmd)
        {
            Console.Clear();
            cmd.Execute();
            return true;
        }
        static bool EXE_mnu(Commands.ICommand cmd)
        {
            if (cmd is Commands.IHasMenu && (cmd as Commands.IHasMenu).Menu.MenuItems.Count > 0)
                (cmd as Commands.IHasMenu).Menu.Print();

            return true;
        }
        static bool EXE_Prompt(Commands.ICommand cmd)
        {
            if (cmd is Commands.IHasPrompter)
            {
                Console.Clear();
                return (cmd as Commands.IHasPrompter).Prompter.Prompt();
            }
            return true;
        }

        private static void HandleKey(MenuItem mi)
        {
            List<string> order = new List<string>();
            if (mi != null && mi.Action != null)
            {
                Console.Clear();
                mi.Action();
            }

            if (mi != null && mi.Command != null)
            {
                //order = GetExecutionOrder(mi, order);

                List<Func<Commands.ICommand, bool>> mylst = (from x in GetExecutionOrder(mi, order)
                                                             select ExecuteOrder.First(f => f.Method.Name == x)).ToList();

                for (int i = 1; i < mylst.Count + 1; i++)
                {
                    if (!mylst[i - 1](mi.Command))
                        i++;
                }

            }

            if (mi != null && mi.Command != null)
            {
                if (mi.Command is Commands.IHasPrompter && (mi.Command as Commands.IHasPrompter).KeepLastAnswers)
                    (mi.Command as Commands.IHasPrompter).Prompter.All(p => { p.DefaultValue = p.UserValue; return true; });
            }
        }

        private static List<string> GetExecutionOrder(MenuItem mi, List<string> order)
        {
            if (mi.Command is Commands.IHasMenu)
            {
                switch ((mi.Command as Commands.IHasMenu).MenuOrder)
                {
                    case Commands.MenuOrder.none:
                    case Commands.MenuOrder.BeforeExecute:
                        order = new List<string>() { "EXE_mnu", "EXE_Prompt", "EXE_exe" };
                        break;
                    case Commands.MenuOrder.AfterExecute:
                        order = new List<string>() { "EXE_Prompt", "EXE_exe", "EXE_mnu" };
                        break;
                    case Commands.MenuOrder.BeforePrompt:
                        order = new List<string>() { "EXE_mnu", "EXE_Prompt", "EXE_exe" };
                        break;
                    case Commands.MenuOrder.AfterPrompt:
                        order = new List<string>() { "EXE_Prompt", "EXE_mnu", "EXE_exe" };
                        break;
                    default:
                        order = new List<string>() { "EXE_Prompt", "EXE_mnu", "EXE_exe" };
                        break;
                }
            }
            else if (mi.Command is Commands.IHasPrompter)
                order = new List<string>() { "EXE_Prompt", "EXE_exe" };
            else
                order = new List<string>() { "EXE_exe" };
            return order;
        }
    }
}
