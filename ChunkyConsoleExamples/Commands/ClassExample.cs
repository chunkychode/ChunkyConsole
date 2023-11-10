using System;

namespace ChunkyConsoleExamples.Commands
{
    class ClassExample : ChunkyConsole.Commands.ReflectionMenuPrompterCommand
    {

        public int DontPromptDays { get; set; }
        [ChunkyConsole.IncludeMethod]
        public int SomeNumber { get; set; }

        [ChunkyConsole.IncludeMethod(true)]
        public string SomeString { get; set; }

        [ChunkyConsole.IncludeMethod]
        public DateTime SomeDate { get; set; }

        [ChunkyConsole.IncludeMethod]
        public bool SomeBool { get; set; }

        public ClassExample() : base()
        {
            this.MenuOrder = ChunkyConsole.Commands.MenuOrder.AfterPrompt;
        }
        [ChunkyConsole.IncludeMethod]
        public void PrintTheDate()
        {
            Console.WriteLine("the date is:" + DateTime.Now.ToString());

            Console.WriteLine("number: " + SomeNumber);
            Console.WriteLine("bool: " + SomeBool);
            Console.WriteLine("date: " + SomeDate);
            Console.WriteLine("str: " + SomeString);
            Console.Read();
        }
        [ChunkyConsole.IncludeMethod]
        public void PrintDaysAlive(DateTime birthdate, bool IncludeStff)
        {
            Console.WriteLine("you entered: " + birthdate);
            Console.WriteLine("you included: " + IncludeStff);
            Console.WriteLine(DateTime.Now.Subtract(birthdate).TotalDays);
            Console.Read();
        }
    }
}
