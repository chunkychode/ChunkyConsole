using System;

namespace ChunkyConsoleExamples.Commands
{
    class PromptExample : ChunkyConsole.Commands.PrompterCommand
    {
        public enum MyFakeProperties { Name, RandomNumber }

        public int Age { get { return Prompter.Get<int>("Age"); } }

        public PromptExample()
        {
            AddPrompts();
        }
        private void AddPrompts()
        {
            Add("Must be a value from the System.DayOfWeek enum", new ChunkyConsole.Validators.Enum<System.DayOfWeek>());
            Add<MyFakeProperties>();
            Add("How old are you", "12", new ChunkyConsole.Validators.IntRange(1, 90));
            Add("Please specify an existing directory", new ChunkyConsole.Validators.Directory(false, true));
            Add("Email Address", string.Empty, "value must be a valid email", new ChunkyConsole.Validators.RegEx(ChunkyConsole.Validators.RegEx.EMAIL));
        }
        public override void Execute()
        {


            var MyFakePropertiesRandomNumber = this.Prompter.Get<string>(MyFakeProperties.RandomNumber.ToString());
            var myemail = this.Prompter["Email Address"];

            Console.WriteLine("this.Prompter.Get<string>(MyFakeProperties.RandomNumber.ToString());-->" + MyFakePropertiesRandomNumber);
            Console.WriteLine("this.Prompter[\"Email Address\"]==>" + myemail);


            Console.WriteLine("All entered values");

            Console.WriteLine("Press any key to continue");
            Console.Read();
        }
    }
}
