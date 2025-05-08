using System;

namespace ChunkyConsoleExamples.Commands
{
    class ClassExample : ChunkyConsole.Commands.ReflectionMenuPrompterCommand
    {

        public int DontPromptDays { get; set; }
        [ChunkyConsole.IncludeProperty]
        public int SomeNumber { get; set; }

        [ChunkyConsole.IncludeProperty(true)]
        public string SomeString { get; set; }

        [ChunkyConsole.IncludeProperty]
        public DateTime SomeDate { get; set; }

        [ChunkyConsole.IncludeProperty]
        public bool SomeBool { get; set; }

        public ClassExample() : base()
        {
            this.MenuOrder = ChunkyConsole.Commands.MenuOrder.AfterPrompt;
        }

        public override void Execute()
        {
            PrintMyProperties();
        }

        [ChunkyConsole.IncludeMethod]
        public void PrintTheDate()
        {
            Console.WriteLine("the date is:" + DateTime.Now.ToString());
            Console.ReadKey();
        }
        [ChunkyConsole.IncludeMethod]
        public void PrintDaysAlive(DateTime birthdate, bool IncludeStff)
        {
            Console.WriteLine("you entered: " + birthdate);
            Console.WriteLine("you included: " + IncludeStff);
            Console.WriteLine(DateTime.Now.Subtract(birthdate).TotalDays);
            Console.ReadKey();
        }
        [ChunkyConsole.IncludeMethod]
        public void Printrandomnumber()
        {
            Console.WriteLine(new Random().Next());
            Console.ReadKey();
        }

        [ChunkyConsole.IncludeMethod]
        public void InvokeWithParameter(string name)
        {
            Console.WriteLine(new Random().Next() + name);
            Console.ReadKey();
        }

        
        public void PrintMyProperties()
        {
            Console.WriteLine("number: " + SomeNumber);
            Console.WriteLine("bool: " + SomeBool);
            Console.WriteLine("date: " + SomeDate);
            Console.WriteLine("str: " + SomeString);
            Console.ReadKey();
        }
    }
}
