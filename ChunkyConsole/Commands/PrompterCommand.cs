using System;


namespace ChunkyConsole.Commands
{
    public abstract class PrompterCommand : Command, IHasPrompter
    {
        public PrompterCommand()
        {
            this.Prompter = new Prompts.Prompter();
            this.KeepLastAnswers = true;
        }
        public Prompts.Prompter Prompter { get; set; }
        public bool KeepLastAnswers { get; set; }

        public abstract override void Execute();


        /// <summary>
        /// Adds a Prompt
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="defaultValue"></param>
        /// <param name="errorPrompt"></param>
        /// <param name="validator"></param>
        protected void Add(string prompt, string defaultValue, string errorPrompt, Validators.IValidator validator = null, bool isPassword = false)
        {
            if (validator == null)
                validator = Validators.BaseValidator<string>.InstanceByType(this, prompt);


            Prompter.AddLast(new Prompts.PromptItem() { DefaultValue = defaultValue, ErrorPrompt = string.IsNullOrEmpty(errorPrompt) ? validator.ErrorMessage : errorPrompt, Prompt = prompt, Validator = validator, IsPassword = isPassword, UseFileAutoComplete = UserAutoComplete(validator) });
        }
        private bool UserAutoComplete(Validators.IValidator validator)
        {
            return ((validator is Validators.File && (validator as Validators.File).MustExist)) || ((validator is Validators.Directory && (validator as Validators.Directory).MustExist));
        }
        /// <summary>
        /// Adds a Prompt
        /// </summary>
        /// <param name="prompt"></param>
        protected void Add(string prompt, bool isPassword = false)
        {
            Add(prompt, string.Empty, string.Empty, null, isPassword);
        }

        /// <summary>
        /// Adds a Prompt
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="defaultValue"></param>
        protected void Add(string prompt, string defaultValue)
        {
            Add(prompt, defaultValue, string.Empty);
        }


        /// <summary>
        /// Adds a Prompt
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="validator"></param>
        protected void Add(string prompt, Validators.IValidator validator, bool isPassword = false)
        {
            Add(prompt, string.Empty, validator.ErrorMessage, validator, isPassword);
        }


        /// <summary>
        /// Adds a Prompt
        /// </summary>
        /// <param name="prompt"></param>
        /// <param name="defaultValue"></param>
        /// <param name="validator"></param>
        protected void Add(string prompt, string defaultValue, Validators.IValidator validator)
        {
            Add(prompt, defaultValue, validator.ErrorMessage, validator);
        }

        /// <summary>
        /// Adds a prompt for each enum value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumrable"></param>
        protected void Add<T>() where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Type must be an Enum");

            foreach (var item in Enum.GetNames(typeof(T)))
            {
                Add(item);
            }
        }
    }
}
