using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChunkyConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the items to be shown 
            UI.Menu m = new UI.Menu();
            m.MenuItems.Add(new UI.MenuItem() { Title = "Misc", Key = ConsoleKey.D1, Command = new command1() });
            m.MenuItems.Add(new UI.MenuItem() { Title = "Date Time", Key = ConsoleKey.D2, Command = new command2() });
            m.MenuItems.Add(new UI.MenuItem() { Title = "nothig", Key = ConsoleKey.N, Command = new command3() });
            m.MenuItems.Add(new UI.MenuItem() { Title = "MP", Key = ConsoleKey.M, Command = new command4() });
            m.MenuItems.Add(new UI.MenuItem() { Title = "Auto", Key = ConsoleKey.A, Command = new ClassTemplate() });
            m.Print();

        }
    }
    class command3 : Commands.Command
    {
        public override void Execute()
        {
            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
    class command1 : Commands.PrompterCommand
    {
        public command1() : base() {
            Add("prompt 1", "default value1");
            Add("age?", new Validators.IntRange(1, 34));
        }
        public override void Execute()
        {
         
            Console.WriteLine("you entered 0: " + Prompter.Get<string>(0));
            Console.WriteLine("you entered 1: " + Prompter.Get<int>(1));
            Console.WriteLine("done");
            Console.ReadKey();
        }
    }

    class command2 : Commands.MenuCommand
    {
        public command2()
        {
            base.Menu.BackColor = ConsoleColor.DarkBlue;
            base.Menu.ForeColor = ConsoleColor.Yellow;
            Add("What time is it", ConsoleKey.T, () => { Console.WriteLine(DateTime.Now.ToShortTimeString()); Console.ReadKey(); });
            Add("What Date is it", ConsoleKey.D, () => {Console.WriteLine(DateTime.Now.ToShortDateString()); Console.ReadKey();});
            Add("exit", ConsoleKey.X, () => this.Menu.ExitOnBadSelection = true);
            
            
        }
        public override void Execute()
        {
            Console.WriteLine("done");
            Console.ReadKey();
        }
    }

    class command4:Commands.MenuPromptCommand
    {
        
        public command4():base()
        {
            Menu.BackColor = ConsoleColor.Gray;
            Menu.ForeColor = ConsoleColor.Blue;
            
            Add("Name", new Validators.NonBlankString());
            Add("Age", "12", new Validators.IntRange(1, 90));

            Add("Mutiply buy 2", ConsoleKey.M, () => { Console.WriteLine(Age * 2); Console.ReadKey(); });
            Add("Add 5", ConsoleKey.A, () => { Console.WriteLine(Age + 5); Console.ReadKey(); });
            Add("String X", ConsoleKey.F, () => { Console.WriteLine(Name + " is " + Age); Console.ReadKey(); });
            
            base.MenuOrder = Commands.MenuOrder.AfterPrompt;
            
        }

        public string Name { get { return Prompter.Get<string>(0); } }
        public int Age { get { return Prompter.Get<int>(1); } }

        public override void Execute()
        {
            Console.WriteLine(Prompter.ToString());
            Utils.ConsoleSpinner.WrapAction(coundto100, "my message");
            Console.ReadKey();
        }
        private void coundto100()
        {
            for (int i = 0; i < 100; i++)
            {
                System.Threading.Thread.Sleep(10);
            }
        }
    }



}
