using System;
using System.Linq;


namespace ChunkyConsoleExamples.Commands
{
    class ReflectionExample : ChunkyConsole.Commands.ReflectionMenuPrompterCommand
    {
        [ChunkyConsole.IncludeMethod]
        public string MyFiledProperty { get; set; }
        [ChunkyConsole.IncludeMethod]
        public string MyStringProperty { get; set; }

        [ChunkyConsole.IncludeMethod]
        public int MyIntProperty { get; set; }

        [ChunkyConsole.IncludeMethod]
        public decimal MyDecimalProperty { get; set; }

        [ChunkyConsole.IncludeMethod]
        public DateTime MyDateProperty { get; set; }


        [ChunkyConsole.IncludeMethod(true)]
        public string MyPasswordProperty { get; set; }




        public ReflectionExample()
            : base()
        {
            base.Prompter.First(f => f.Prompt == "MyFiledProperty").Validator = new ChunkyConsole.Validators.File(true);
            base.Prompter.First(f => f.Prompt == "MyFiledProperty").ErrorPrompt = "File must exist";


        }
        public override void Execute()
        {
            Console.WriteLine("you have assigned {0} to MyStringProperty", MyStringProperty);
            Console.WriteLine("you have assigned {0} to MyIntProperty", MyIntProperty);
            Console.WriteLine("you have assigned {0} to MyDecimalProperty", MyDecimalProperty);
            Console.WriteLine("you have assigned {0} to MyDateProperty", MyDateProperty);
            Console.WriteLine("you have assigned {0} to MyPasswordProperty", MyPasswordProperty);
            Console.WriteLine("you have assigned {0} to MyFiledProperty", MyFiledProperty);


            Console.Read();
        }
    }
}
